using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {
    class ImportMockService: IImportService {

        #region Constructor
        public ImportMockService(ImportServiceCallback callback) : base(callback) {
        }
        #endregion

        #region public Method
        internal override void Start(Dictionary<DicType, string> targetList) {

        }
        #endregion
    }
}
