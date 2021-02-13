using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// additions repo
    /// </summary>
    internal class AdditionsRepo : IBasicRepo<AdditionData> {

        #region Declaration
        private AdditionsEntity _entity;
        #endregion

        #region Constructor
        internal AdditionsRepo(DictionaryDatabase database) : base(database) {
            this._entity = new AdditionsEntity(database);
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

        internal override void SetDataModel(AdditionData model) {
            this._entity.Id = model.Id;
            this._entity.MeaningId = model.MeaningId;
            this._entity.Type = model.Type;
            this._entity.Data = model.Data;
        }
        #endregion
    }
}
