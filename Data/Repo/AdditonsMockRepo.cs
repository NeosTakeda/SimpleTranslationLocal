using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// additions mock repo
    /// </summary>
    class AdditonsMockRepo : IBasicRepo<AdditionData> {

        #region Constructor
        internal AdditonsMockRepo(DictionaryDatabase database) : base(database) { }
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

        internal override void SetDataModel(AdditionData model) {
            // NOP
        }
        #endregion
    }
}
