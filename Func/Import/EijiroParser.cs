using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.DataModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;


namespace SimpleTranslationLocal.Func.Import {
    class EijiroParser : IDictionaryParser {

        #region Declaration
        private FileOperator _operator;

        // 名前つけるの面倒すぎるので reg1、reg2～とする。。
        /// <summary>
        /// 単語と品詞を取得 →「:」より左側の情報。品詞はない場合あり
        /// </summary>
        private RegExUtil _reg1 = new RegExUtil(@"^■(?<k1>.+) {(?<k2>.+)}\s:\s|^■(?<k1>.+)\s:\s");

        private class WordInfo {
            public const string Pronunciation = "【発音】";
            public const string Pronunciation2 = "【発音！】";
            public const string Kana = "【＠】";
            public const string Level = "【レベル】";
            public const string Syllable = "【分節】";
            public const string Change = "【変化】";
        }
        #endregion

        #region Public Property
        /// <summary>
        /// 現在の行
        /// </summary>
        public override long CurrentLine {  get { return this._operator.CurrentLine; } }
        #endregion

        #region Constructor
        public EijiroParser(string file) : base(file) {
            this._operator = new FileOperator(file, FileOperator.OpenMode.Read, FileOperator.EncodingType.ShiftJIS);
        }
        #endregion

        #region Public Method

        public override long GetRowCount(GetRowCountCallback callback) {
            var rowCount = 0;
            using(var op = new FileOperator(base._file, FileOperator.OpenMode.Read, FileOperator.EncodingType.ShiftJIS)) {
                while(op.ReadLine() != null) {
                    callback(++rowCount);
                }
            }
            
            return rowCount;
        }

        public override WordData Read() {
            WordData data = null;
            WordData current = null;
            string line;
            while((line = this._operator.ReadLine()) != null) {
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
            var wordData = new WordData();
            wordData.Meanings = new List<MeaningData>();
            var meaningData = new MeaningData();
            wordData.Meanings.Add(meaningData);

            var tmp = line.Trim();
            
            // 単語・品詞を取得
            if (this._reg1.Match(tmp)) {
                wordData.Word = this._reg1.GroupValue("k1");
                meaningData.PartOfSpeach = this._reg1.GroupValue("k2").Replace("-1","").Replace("-2","");
                tmp = this._reg1.Remain;
            }

            const string SignEx = "■・";
            const string SignSp = "◆";

            int GetMinPos() {
                var p1 = tmp.IndexOf(SignEx);
                var p2 = tmp.IndexOf(SignSp);
                return Math.Max(p1, p2);
            }

            // 単語情報を設定
            void SplitIds() {
                string[] s = { "、" };
                var data = tmp.Split(s, StringSplitOptions.None);
                foreach(var datum in data) {
                    if (datum.StartsWith(WordInfo.Pronunciation)) {
                        wordData.Pronumciation = datum.Substring(WordInfo.Pronunciation.Length);
                        continue;
                    }
                    if (datum.StartsWith(WordInfo.Pronunciation2)) {
                        wordData.Pronumciation = datum.Substring(WordInfo.Pronunciation2.Length);
                        continue;
                    }
                    if (datum.StartsWith(WordInfo.Kana)) {
                        wordData.Kana = datum.Substring(WordInfo.Kana.Length);
                        continue;
                    }
                    if (datum.StartsWith(WordInfo.Level)) {
                        wordData.Level = int.Parse(datum.Substring(WordInfo.Level.Length));
                        continue;
                    }
                    if (datum.StartsWith(WordInfo.Syllable)) {
                        wordData.Syllable = datum.Substring(WordInfo.Syllable.Length);
                        continue;
                    }
                    if (datum.StartsWith(WordInfo.Change)) {
                        wordData.Change = datum.Substring(WordInfo.Change.Length);
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
                var data = "";
                while (0 <= pos) {
                    if (0 == pos) {
                        data = tmp;
                        tmp = "";
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
            if (0 < src.Pronumciation.Length) {
                dest.Pronumciation = src.Pronumciation;
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
        }
        #endregion
    }
}
