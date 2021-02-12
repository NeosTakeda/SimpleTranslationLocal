using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// meanings mock repo
    /// </summary>
    internal class MeaningMockRepo : IBasicRepo<MeaningData> {

        #region Constructor
        internal MeaningMockRepo(DictionaryDatabase database) : base(database) { }
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

        internal override void SetDataModel(MeaningData model) {
            // NOP
        }
        #endregion
    }
}
