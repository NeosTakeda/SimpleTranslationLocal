using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using System.Collections.Generic;
using System;
using System.Text;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Text.RegularExpressions;

namespace SimpleTranslationLocal.Func.Import {
    /// <summary>
    /// Webster parser
    /// </summary>
    internal class WebsterParser: IDictionaryParser {

        #region Declaration
        private FileOperator _operator;
        #endregion

        #region Public Property
        /// <summary>
        /// 現在の行
        /// </summary>
        internal override long CurrentLine { get { return this._operator.CurrentLine; } }
        #endregion

        #region Constructor
        public WebsterParser(string file) : base(file) {
            this._operator = new FileOperator(file, FileOperator.OpenMode.Read);
        }
        #endregion

        #region Public Method
        internal override long GetRowCount(GetRowCountCallback callback) {
            var rowCount = 0;
            using (var op = new FileOperator(base._file, FileOperator.OpenMode.Read)) {
                while (!op.Eof) {
                    op.ReadLine();
                    callback(++rowCount);
                }
            }

            return rowCount;
        }

        internal override WordData Read() {
            WordData data = null;
            string line;
            while (!this._operator.Eof) {
                line = this._operator.ReadLine();
                if (0 == line.Length) {
                    continue;
                }

                data = this.Parse(line);
                if (null == data) {
                    // first row or last row
                    continue;
                }

                return data;
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
            var tmp = line.Trim();
            if (tmp == "{" || tmp == "}") {
                return null;
            }

            var wordData = new WordData();
            wordData.Meanings = new List<MeaningData>();
            var meaningData = new MeaningData();
            wordData.Meanings.Add(meaningData);

            var quote = "\"".ToCharArray();
            var pos = tmp.IndexOf(":");
            wordData.Word = TrimJsonData(tmp.Substring(0, pos-1));
            meaningData.Meaning = TrimJsonData(tmp.Substring(pos + 1));
            return wordData;
        }

        /// <summary>
        /// trim json data
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string TrimJsonData(string val) {
            const string yen = "\\";
            const string quote = "\"";
            var result = val.Trim();
            if (result.StartsWith(quote)) {
                result = result.Substring(1);
            }
            if (result.EndsWith($"{quote},")) {
                result = result.Substring(0, result.Length - 2);
            }
            if (result.EndsWith(quote)) {
                result = result.Substring(0, result.Length - 1);
            }
            result = result.Replace(@"\n\n", "<li></li>");
            result = result.Replace(yen + quote, quote);
            result = Regex.Replace(result, @"\s(?<num>\d\d?\.)", "<br/>$1");
            result = Regex.Replace(result, @"<br/>(?<num>\d\d?\.)<br/>", " $1<br/>");
            return result;
        }
        #endregion
    }
}
