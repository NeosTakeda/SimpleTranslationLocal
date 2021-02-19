using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// dictionary repo
    /// </summary>
    internal class DictionaryRepo : IBasicRepo<DictionaryData> {

        #region Declaration
        private readonly DictionaryEntity _entity;
        private readonly DictionaryMemoryEntity _memoryEntity;
        private delegate List<DictionaryData> SearchDelegate(string word, MatchType matchType);
        private readonly SearchDelegate searchMethod = null;
        #endregion

        #region Constructor
        internal DictionaryRepo(bool userMemoryDic, Action completeLoad) : base(null) {
            if (userMemoryDic) {
                searchMethod = SearchMemory;
                this._memoryEntity = new DictionaryMemoryEntity(completeLoad); 
            } else {
                searchMethod = SearchDatabase;
            }
        }

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
            return this.searchMethod(word, matchType);
        }

        /// <summary>
        /// search dictionary
        /// </summary>
        /// <param name="word">word</param>
        /// <param name="matchType">search matching type</param>
        /// <returns>result. if data does not find, return null</returns>
        internal List<DictionaryData> SearchDatabase(string word, MatchType matchType) {
            using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                database.Open();
                var entity = new DictionaryEntity(database);
                using (var recset = entity.Search(word, matchType)) {
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
                        dictionaryData = new DictionaryData {
                            SourceId = recset.GetInt(DictionaryEntity.Cols.SourceId),
                            Word = recset.GetString(DictionaryEntity.Cols.Word),
                            Data = recset.GetString(DictionaryEntity.Cols.Data)
                        };
                        result.Add(dictionaryData);
                    }

                    return result;
                }
            }
        }

        internal List<DictionaryData> SearchMemory(string word, MatchType matchType) {
            return this._memoryEntity.Search(word, matchType);
        }
        #endregion
    }
}
