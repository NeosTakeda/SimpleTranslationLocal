using OsnCsLib.File;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Func.Import {
    internal abstract class IDictionaryParser {

        #region Declaration
        protected string _file;

        /// <summary>
        /// 件数カウントのコールバック
        /// </summary>
        /// <param name="count"></param>
        public delegate void GetRowCountCallback(long count);
        #endregion

        #region Public Property
        /// <summary>
        /// 現在の行(パーサーは複数行読み取ってから返却することがある)
        /// </summary>
        internal virtual long CurrentLine { get; } = 0;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file">対象のファイル</param>
        internal IDictionaryParser(string file) {
            if (!System.IO.File.Exists(file)) {
                throw new System.Exception($"File({file}) not found!");
            }
            this._file = file;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// get total row count
        /// </summary>
        /// <returns>row count</returns>
        internal abstract long GetRowCount(GetRowCountCallback callback);

        /// <summary>
        /// parse data
        /// </summary>
        /// <returns>ワードデータ</returns>
        internal abstract WordData Read();
        #endregion

    }
}
