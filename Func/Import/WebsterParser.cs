using SimpleTranslationLocal.Data.DataModel;
using System;

namespace SimpleTranslationLocal.Func.Import {
    class WebsterParser: IDictionaryParser {

        #region Constructor
        public WebsterParser(string file) : base(file) {
        }
        #endregion

        #region Public Method
        public override long GetCount() {
            throw new NotImplementedException();
        }

        public override WordData Read() {
            throw new NotImplementedException();
        }
        #endregion

    }
}
