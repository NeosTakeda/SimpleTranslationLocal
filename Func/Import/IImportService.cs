using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {
    class IImportService {

        #region Declaration
        private ImportServiceCallback _callback;
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
        public void Start(Dictionary<DicType, string>targetList) {

        }
        #endregion
    }
}
