using SimpleTranslationLocal.Data.DataModel;

namespace SimpleTranslationLocal.Func.Import {
    class MockParser : IDictionaryParser {

        #region Constructor
        public MockParser(string file) : base(file) {
        }
        #endregion

        #region Public Method

        public override long GetRowCount(GetRowCountCallback callback) {
            return 0;
        }

        public override WordData Read() {
            return null;
        }
        #endregion

    }
}
