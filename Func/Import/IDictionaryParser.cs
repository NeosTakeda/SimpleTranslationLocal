using OsnCsLib.File;
using SimpleTranslationLocal.Data.DataModel;

namespace SimpleTranslationLocal.Func.Import {
    abstract class IDictionaryParser {

        #region Declaration
        private FileOperator _operator;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file">対象のファイル</param>
        public IDictionaryParser(string file) {
            this._operator = new FileOperator(file);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// get total row count
        /// </summary>
        /// <returns>row count</returns>
        public abstract long GetCount();

        /// <summary>
        /// parse data
        /// </summary>
        /// <returns>ワードデータ</returns>
        public abstract WordData Read();
        #endregion

    }
}
