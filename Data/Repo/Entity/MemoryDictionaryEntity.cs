using SimpleTranslationLocal.AppCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {
    internal class DictionaryMemoryEntity : BaseEntity {

        #region Declaration
        private bool _isBusy = false;
        private readonly List<DictionaryData> _memoryData = new List<DictionaryData>();
        #endregion

        #region Constructor
        internal DictionaryMemoryEntity() : base(null) {
            this.Load();
        }
        #endregion


        #region Public Method

        internal override bool Create() {
            throw new NotImplementedException();
        }

        internal override void DeleteBySourceId(long id) {
            throw new NotImplementedException();
        }

        internal override long Insert() {
            throw new NotImplementedException();
        }

        public List<DictionaryData> Search(string word, MatchType matchType) {
            if (this._isBusy) {
                var data = new DictionaryData {
                    Data = "<h4>expand data on memory now...</h4>"
                };
                return new List<DictionaryData>() {
                    data
                };
            }

            List<DictionaryData> result = null;
            switch (matchType) {
                case MatchType.Exact:
                    result = this.SearchExact(word.ToLower());
                    break;

                case MatchType.Prefix:
                    result = this.SearchPrefix(word.ToLower());
                    break;

                case MatchType.Broad:
                    result = this.SearchBroad(word.ToLower());
                    break;
            }

            return result;
        }
        #endregion


        #region Private Method
        /// <summary>
        /// load all dictionary data on memory
        /// </summary>
        private async void Load() {
            this._isBusy = true;
            System.Diagnostics.Debug.WriteLine("◆◆◆ READ STR" + DateTime.Now.ToString("hh:mm:ss fff"));
            await Task.Run(() => {
                using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                    database.Open();
                    var entity = new DictionaryEntity(database);

                    using (var record = entity.SelectAll()) {
                        while (record.Read()) {
                            var data = new DictionaryData {
                                SourceId = record.GetInt(DictionaryEntity.Cols.SourceId),
                                //                            data.Word = record.GetString(DictionaryEntity.Cols.Word);
                                WordSort = record.GetString(DictionaryEntity.Cols.WordSort),
                                Data = record.GetString(DictionaryEntity.Cols.Data)
                            };
                            this._memoryData.Add(data);
                        }
                    }
                    this._isBusy = false;
                    System.Diagnostics.Debug.WriteLine("◆◆◆ READ END" + DateTime.Now.ToString("hh:mm:ss fff"));
                }
            });
        }

        private List<DictionaryData> SearchExact(string word) {
            var found = false;
            List<DictionaryData> result = new List<DictionaryData>();
            for (var i = 0; i < this._memoryData.Count; i++) {
                if (this._memoryData[i].WordSort == word) {
                    found = true;
                    result.Add(this._memoryData[i]);
                } else {
                    if (found) {
                        break;
                    }
                }
            }
            return (result.Count == 0) ? null : result;
        }
        private List<DictionaryData> SearchPrefix(string word) {
            var count = 0;
            List<DictionaryData> result = new List<DictionaryData>();
            for (var i = 0; i < this._memoryData.Count; i++) {
                if (this._memoryData[i].WordSort.StartsWith(word)) {
                    count++;
                    result.Add(this._memoryData[i]);
                    if (Constants.MaxNumberOfListWord <= count) {
                        break;
                    }
                }
            }
            return (result.Count == 0) ? null : result;
        }
        private List<DictionaryData> SearchBroad(string word) {
            var count = 0;
            List<DictionaryData> result = new List<DictionaryData>();
            for (var i = 0; i < this._memoryData.Count; i++) {
                if (this._memoryData[i].WordSort.Contains(word)) {
                    count++;
                    result.Add(this._memoryData[i]);
                    if (Constants.MaxNumberOfListWord <= count) {
                        break;
                    }
                }
            }
            return (result.Count == 0) ? null : result;
        }
        #endregion
    }
}
