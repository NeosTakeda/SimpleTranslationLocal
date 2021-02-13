using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {

    /// <summary>
    /// import dictionanry data
    /// </summary>
    internal abstract class IImportService {

        #region Declaration
        protected ImportServiceCallback _callback;
        #endregion

        #region Constructor
        public IImportService(ImportServiceCallback callback) {
            this._callback = callback;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// インポート処理を開始する
        /// </summary>
        /// <param name="targetList">インポート対象のリスト</param>
        internal abstract void Start(Dictionary<DicType, string> targetList);
        #endregion

    }
}
