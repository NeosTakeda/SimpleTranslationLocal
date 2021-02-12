﻿using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// Source Repo
    /// </summary>
    internal class SourcesRepo : IBasicRepo<SourceData> {

        #region Declaration
        private SourcesEntity _entity;
        #endregion

        #region Constructor
        internal SourcesRepo(DictionaryDatabase database) : base(database) {
            this._entity = new SourcesEntity(database);
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

        internal override void SetDataModel(SourceData model) {
            this._entity.Id = model.Id;
            this._entity.Name = model.Name;
            this._entity.Priority = model.Priority;
            this._entity.File = model.File;
        }
        #endregion
    }
}
