using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// dictionary repo
    /// </summary>
    internal class DictionaryRepo : IBasicRepo<DictionaryData> {
        #region Declaration
        private DictionaryEntity _entity;
        #endregion

        #region Constructor
        internal DictionaryRepo(DictionaryDatabase database) : base(database) {
            this._entity = new DictionaryEntity(database);
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

        internal override void SetDataModel(DictionaryData model) {
            this._entity.Id = model.Id;
            this._entity.SourceId = model.SourceId;
            this._entity.Word = model.Word;
            this._entity.Data = model.Data;
        }

        /// <summary>
        /// search dictionary
        /// </summary>
        /// <param name="word">word</param>
        /// <param name="matchType">search matching type</param>
        /// <returns>result. if data does not find, return null</returns>
        internal List<DictionaryData> Search(string word, MatchType matchType) {
            using (var recset = this._entity.Search(word, matchType)) {
                if (!recset.HasRows) {
                    return null;
                }

                var result = new List<DictionaryData>();
                DictionaryData dictionaryData = null;
                int wordCount = 1;

                while (recset.Read()) {
                    if (Constants.MaxNumberOfListWord < wordCount) {
                        return result;
                    }
                    wordCount++;
                    dictionaryData = new DictionaryData();
                    dictionaryData.SourceId = recset.GetInt(DictionaryEntity.Cols.SourceId);
                    dictionaryData.Word = recset.GetString(DictionaryEntity.Cols.Word);
                    dictionaryData.Data = recset.GetString(DictionaryEntity.Cols.Data);
                    result.Add(dictionaryData);
                }

                return result;
            }
        }
        #endregion
    }
}
