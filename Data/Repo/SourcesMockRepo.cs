using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {
    /// <summary>
    /// mock sources repo
    /// </summary>
    internal class SourcesMockRepo: IBasicRepo<SourceData> {

        #region Constructor
        internal SourcesMockRepo(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        internal override bool Create() {
            return true;
        }

        internal override void DeleteBySourceId(long id) {
            // NOP
        }

        internal override long Insert() {
            return 0;
        }

        internal override void SetDataModel(SourceData model) {
            // NOP
        }
        #endregion
    }
}
