using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;

namespace SimpleTranslationLocal.Func.Import {
    internal class WebsterParser: IDictionaryParser {

        #region Constructor
        public WebsterParser(string file) : base(file) {
        }
        #endregion

        #region Public Method
        internal override long GetRowCount(GetRowCountCallback callback) {
            throw new NotImplementedException();
        }

        internal override WordData Read() {
            throw new NotImplementedException();
        }
        #endregion

    }
}
