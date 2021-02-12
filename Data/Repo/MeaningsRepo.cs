using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// meanings repo
    /// </summary>
    internal class MeaningsRepo : IBasicRepo<MeaningData> {

        #region Declaration
        private MeaningsEntity _entity;
        #endregion

        #region Constructor
        internal MeaningsRepo(DictionaryDatabase database) : base(database) {
            this._entity = new MeaningsEntity(database);
        }
        #endregion

        #region Public Method
        internal override bool Create() {
            return this._entity.Create();
        }

        internal override long Insert() {
            return this._entity.Insert();
        }

        internal override void DeleteBySourceId(long id) {
            this._entity.DeleteBySourceId(id);
        }

        internal override void SetDataModel(MeaningData model) {
            this._entity.Id = model.Id;
            this._entity.WordId = model.WordId;
            this._entity.Meaning = model.Meaning;
            this._entity.PartOfSpeach = model.PartOfSpeach;
        }
        #endregion
    }
}
