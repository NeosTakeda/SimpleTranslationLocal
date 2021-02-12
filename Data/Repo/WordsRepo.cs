using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;

namespace SimpleTranslationLocal.Data.Repo {
    internal class WordsRepo : IBasicRepo<WordData> {

        #region Declaration
        private WordsEntity _entity;
        #endregion

        #region Constructor
        internal WordsRepo(DictionaryDatabase database) : base(database) {
            this._entity = new WordsEntity(database);
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

        internal override void SetDataModel(WordData model) {
            this._entity.Id = model.Id;
            this._entity.SourceId = model.SourceId;
            this._entity.Word = model.Word;
            this._entity.Pronunciation = model.Pronunciation;
            this._entity.Syllable = model.Syllable;
            this._entity.Kana = model.Kana;
            this._entity.Level = model.Level;
            this._entity.Word = model.Word;
            this._entity.Change = model.Change;
        }
        #endregion
    }
}
