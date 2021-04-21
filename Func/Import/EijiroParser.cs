using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleTranslationLocal.Func.Import {
    /// <summary>
    /// Eijiro Parser
    /// </summary>
    internal class EijiroParser : IDictionaryParser {

        #region Declaration
        private readonly FileOperator _operator;

        // 名前つけるの面倒すぎるので reg1、reg2～とする。。
        /// <summary>
        /// 単語と品詞を取得 →「:」より左側の情報。品詞はない場合あり
        /// </summary>
        private readonly RegExUtil _reg1 = new RegExUtil(@"^■(?<k1>.+) {(?<k2>.+)}\s:\s|^■(?<k1>.+)\s:\s");


        private class WordInfo {
            public const string Pronunciation = "【発音】";
            public const string Pronunciation2 = "【発音！】";
            public const string Kana = "【＠】";
            public const string Level = "【レベル】";
            public const string Syllable = "【分節】";
            public const string Change = "【変化】";
            // 以降は取り込む予定はないので変数名は設定しない(下記の３つに人tするのは全部で４件のみ)
            //public const string xxx = "【発音の地域差】";
            //public const string xxx = "【同音】";
            //public const string xxx = "【スペリングに注意】";
        }
        #endregion

        #region Public Property
        /// <summary>
        /// 現在の行
        /// </summary>
        internal override long CurrentLine {  get { return this._operator.CurrentLine; } }
        #endregion

        #region Constructor
        public EijiroParser(string file) : base(file) {
            this._operator = new FileOperator(file, FileOperator.OpenMode.Read, FileOperator.EncodingType.ShiftJIS);
        }
        #endregion

        #region Public Method
        internal override long GetRowCount(GetRowCountCallback callback) {
            var rowCount = 0;
            using(var op = new FileOperator(base._file, FileOperator.OpenMode.Read, FileOperator.EncodingType.ShiftJIS)) {
                while(!op.Eof) {
                    op.ReadLine();
                    callback(++rowCount);
                }
            }
            
            return rowCount;
        }

        internal override WordData Read() {
            WordData data = null;
            WordData current;
            string line;
            while(!this._operator.Eof) {
                line = this._operator.ReadLine();
                if (0 == line.Length) {
                    continue;
                }
                current = this.Parse(line);
                if (null == current) {
                    LogUtil.DebugLog("パース失敗");
                }
                if (null == data) {
                    data = current;
                    continue;
                }
                if (data.Word == current.Word) {
                    AppenData(data, current);
                    continue;
                }
                // 一行戻しておく
                this._operator.UndoRead();
                break;
            }
            return data;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 用語情報(１行)をパース
        /// </summary>
        /// <param name="line">パース対象</param>
        /// <returns>パース結果を格納したWordDataオブジェクト</returns>
        private WordData Parse(string line) {
            var wordData = new WordData {
                Meanings = new List<MeaningData>()
            };
            var meaningData = new MeaningData();
            wordData.Meanings.Add(meaningData);

            var tmp = line.Trim();
            
            // 単語・品詞を取得
            if (this._reg1.Match(tmp)) {
                wordData.Word = this._reg1.GroupValue("k1");
                //                meaningData.PartOfSpeach = this._reg1.GroupValue("k2").Replace("-1", "").Replace("-2", "");
                meaningData.PartOfSpeach = Regex.Replace(this._reg1.GroupValue("k2"), @"-\d\d?", "");
                meaningData.PartOfSpeach = Regex.Replace(meaningData.PartOfSpeach, @"^\d\d?-", "")
                                            .Replace("自動", "自動詞")
                                            .Replace("他動", "他動詞")
                                            .Replace("句動", "句動詞")
                                            .Replace("句自動", "句自動詞")
                                            .Replace("句他動", "句他動詞")
                                            .Replace("名", "名詞")
                                            .Replace("代名", "代名詞")
                                            .Replace("形", "形容詞")
                                            .Replace("副", "副詞")
                                            .Replace("助動", "助動詞")
                                            .Replace("前","前置詞")
                                            .Replace("接続", "接続詞")
                                            .Replace("間投", "間投詞")
                                            .Replace("接頭", "接頭辞")
                                            .Replace("接尾", "接尾辞");
                tmp = this._reg1.Remain;
            }

            const string SignEx = "■・";
            const string SignSp = "◆";

            int GetMinPos(int start = 0) {
                var p1 = tmp.IndexOf(SignEx, start);
                var p2 = tmp.IndexOf(SignSp, start);

                if (0 <= p1 && 0 <= p2) {
                    return Math.Min(p1, p2);
                } else if (0 <= p1) {
                    return p1;
                } else {
                    return p2;
                }
            }

            // 単語情報を設定
            void SplitIds() {
                string[] s = { "、【" };
                var data = tmp.Split(s, StringSplitOptions.None);
                foreach(var datum in data) {
                    var info = datum;
                    if (!info.StartsWith("【")) {
                        info = "【" + datum;
                    }
                    if (info.StartsWith(WordInfo.Pronunciation)) {
                        wordData.Pronunciation = info.Substring(WordInfo.Pronunciation.Length);
                        continue;
                    }
                    if (info.StartsWith(WordInfo.Pronunciation2)) {
                        wordData.Pronunciation = info.Substring(WordInfo.Pronunciation2.Length);
                        continue;
                    }
                    if (info.StartsWith(WordInfo.Kana)) {
                        wordData.Kana = info.Substring(WordInfo.Kana.Length);
                        continue;
                    }
                    if (info.StartsWith(WordInfo.Level)) {
                        wordData.Level = int.Parse(info.Substring(WordInfo.Level.Length));
                        continue;
                    }
                    if (info.StartsWith(WordInfo.Syllable)) {
                        wordData.Syllable = info.Substring(WordInfo.Syllable.Length);
                        continue;
                    }
                    if (info.StartsWith(WordInfo.Change)) {
                        wordData.Change = info.Substring(WordInfo.Change.Length);
                        continue;
                    }
                    LogUtil.DebugLog("unknown id : " + datum);
                }
            }

            // 定義・意味・用例・補足
            void SplitData() {
                var pos = GetMinPos();

                // 用例・補足が存在しない
                if (pos < 0) {
                    // 【 から始まるのは発音・レベルなどのみ定義している行、のはず
                    if (!tmp.StartsWith("【")) {
                        meaningData.Meaning = tmp;
                    } else {
                        SplitIds();
                    }
                    return;
                }

                // 意味・用例・補足を設定
                string data;
                while (0 <= pos) {
                    if (0 == pos) {
                        var nextPos = GetMinPos(1);
                        if (nextPos < 0) {
                            data = tmp;
                            tmp = "";
                        } else {
                            data = tmp.Substring(0, nextPos);
                            tmp = tmp.Substring(nextPos);
                        }
                    } else {
                        data = tmp.Substring(0, pos);
                        tmp = tmp.Substring(pos);
                    }

                    if (data.StartsWith(SignEx)) {
                        meaningData.Additions.Add(
                            new AdditionData(Constants.AdditionType.Example,
                            data.Substring(SignEx.Length)));
                    } else if (data.StartsWith(SignSp)) {
                        meaningData.Additions.Add(
                            new AdditionData(Constants.AdditionType.Supplement,
                            data.Substring(SignSp.Length)));
                    } else {
                        meaningData.Meaning = data;
                    }
                    pos = GetMinPos();
                }

            }

            SplitData();
            if (0 == meaningData.Meaning.Length) {
                wordData.Meanings.Clear();
            }
            return wordData;
        }

        /// <summary>
        /// 用語情報を追加
        /// </summary>
        /// <param name="dest">追加先</param>
        /// <param name="src">追加元</param>
        private void AppenData(WordData dest, WordData src) {
            foreach (var meaning in src.Meanings) {
                dest.Meanings.Add(meaning);
            }
            if (0 < src.Pronunciation.Length) {
                dest.Pronunciation = src.Pronunciation;
            }
            if (0 < src.Syllable.Length) {
                dest.Syllable = src.Syllable;
            }
            if (0 < src.Kana.Length) {
                dest.Kana = src.Kana;
            }
            if (0 < src.Level) {
                dest.Level = src.Level;
            }
            if (0 < src.Change.Length) {
                dest.Change = src.Change;
            }
        }
        #endregion
    }
}
