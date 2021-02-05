using OsnCsLib.File;
using SimpleTranslationLocal.Data.DataModel;
using System;

namespace SimpleTranslationLocal.Func.Import {
    class EijiroParser : IDictionaryParser {

        #region Declaration
        private FileOperator _operator;
        #endregion

        #region Constructor
        public EijiroParser(string file) : base(file) {
            this._operator = new FileOperator(file, FileOperator.OpenMode.Read, System.Text.Encoding.ASCII);
        }
        #endregion

        #region Public Method

        public override long GetRowCount(GetRowCountCallback callback) {
            var rowCount = 0;
            using(var op = new FileOperator(base._file, FileOperator.OpenMode.Read, System.Text.Encoding.ASCII)) {
                while(op.ReadLine() != null) {
                    callback(++rowCount);
                }
            }
            
            return rowCount;
        }

        public override WordData Read() {
            WordData data = null;
            WordData current = null;
            WordData prev = null;

            string line;
            while((line = this._operator.ReadLine()) != null) {
                // パースけっかをcurrentに入れ込み
                if (null == data) {
                    current = data;
                    continue;
                }
                if (data.Word == current.Word) {
                    // 用語以降の情報を入れ込み
                    continue;
                }
                // 一行戻しておく
                this._operator.UndoRead();
            }
            return data;
        }
        #endregion

    }
}
