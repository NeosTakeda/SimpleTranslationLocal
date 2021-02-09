using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.DataModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Func.Import {
    class EijiroParser : IDictionaryParser {

        #region Declaration
        private FileOperator _operator;

        // 名前つけるの面倒すぎるので reg1、reg2～とする。。
        /// <summary>
        /// 単語と品詞を取得 →「:」より左側の情報。品詞はない場合あり
        /// </summary>
        private RegExUtil _reg1 = new RegExUtil(@"^■(?<k1>.+) {(?<k2>.+)}\s:\s|^■(?<k1>.+)\s:\s");

        /// <summary>
        /// <→～> 
        /// </summary>
        private RegExUtil _reg2 = new RegExUtil(@"^〈.+〉＝<→.+|^＝<→.+");
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

            var tmp = line.Trim();

            
            // 単語・品詞を取得
            if (this._reg1.Match(tmp)) {
                wordData.Word = this._reg1.GroupValue("k1");
                meaningData.PartOfSpeach = this._reg1.GroupValue("k2");
                tmp = this._reg1.Remain;
            }


            // 略語・別の言い回しを参照
            if (this._reg2.Match(tmp)) {
                meaningData.Meaning = this._reg2.Value.Replace("＝", "").Replace("<", "").Replace(">", "");
                tmp = this._reg2.Remain;
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
        /// <returns></returns>

        private string[] SplitDef(string val) {

        }
        #endregion
    }
}
