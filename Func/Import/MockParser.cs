
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Func.Import {
    internal class MockParser : IDictionaryParser {

        #region Constructor
        internal MockParser(string file) : base(file) {
        }
        #endregion

        #region Public Method

        internal override long GetRowCount(GetRowCountCallback callback) {
            return 0;
        }

        internal override WordData Read() {
            return null;
        }
        #endregion

    }
}
