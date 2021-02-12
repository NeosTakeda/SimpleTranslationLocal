using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// mock word repo
    /// </summary>
    internal class WordsMockRepo : IBasicRepo<WordData> {

        #region Constructor
        internal WordsMockRepo(DictionaryDatabase database) : base(database) { }
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

        internal override void SetDataModel(WordData model) {
            // NOP
        }
        #endregion
    }
}
