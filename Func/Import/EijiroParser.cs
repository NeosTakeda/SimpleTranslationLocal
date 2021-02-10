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
                    AppenData(current, data);
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

            var other = "";
            var examples = new List<string>();
            var supplements = new List<string>();

            var tmp = line.Trim();

            
            // 単語・品詞を取得
            if (this._reg1.Match(tmp)) {
                wordData.Word = this._reg1.GroupValue("k1");
                meaningData.PartOfSpeach = this._reg1.GroupValue("k2");
                tmp = this._reg1.Remain;
            }

            // 意味・用例・補足でいったん情報を分割する。
            this.SplitDef(data, ref other, ref examples, ref supplements);


            // = から始まるやつ
            if (this._reg2.Match(tmp)) {
                var data = this._reg2.Value.Replace("＝", "").Replace("<", "").Replace(">", "");
                tmp = this._reg2.Remain;


                this.SplitDef(data, ref other, ref examples, ref supplements);
                if (0 == tmp.Length) {
                    goto EXIT;
                }
            }


EXIT:
            return wordData;
        }

        /// <summary>
        /// 用語情報を追加
        /// </summary>
        /// <param name="dest">追加先</param>
        /// <param name="src">追加元</param>
        private void AppenData(WordData dest, WordData src) {

        }

        /// <summary>
        /// 用例・補足・それ以外に分割する
        /// </summary>
        /// <param name="val">対象データ</param>
        /// <param name="other">その他</param>
        /// <param name="examples">用例</param>
        /// <param name="supplements">補足</param>
        private void SplitDef(string val, ref string other, ref List<string>examples, ref List<string>supplements) {

            // 用例に対する補足、補足に対する用例といった関係は無視する。。

            string[] SIGN_EX = { "■・" };
            string[] SIGN_SP = { "◆" };

            other = "";
            examples.Clear();
            supplements.Clear();



            int p1 = val.IndexOf(SIGN_EX);
            int p2 = val.IndexOf(SIGN_SP);

            if (-1 == p1 && -1 == p2) {
                other = val;
                return;
            }
            
            // 用例のみ存在する場合
            if (p2 == -1) {
                var tmp = val.Split(SIGN_EX, StringSplitOptions.None);
            }

            var p = System.Math.Min(p1, p2);
        }


        
        #endregion
    }
}
