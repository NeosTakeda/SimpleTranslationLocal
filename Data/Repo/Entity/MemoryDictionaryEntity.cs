using SimpleTranslationLocal.AppCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SimpleTranslationLocal.AppCommon.Constants;
using System.IO;
using OsnCsLib.File;

namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {
    internal class DictionaryMemoryEntity : BaseEntity {

        #region Declaration
        private bool _isBusy = false;
        private readonly Dictionary<string, List<DictionaryData>> _memoryData = new Dictionary<string, List<DictionaryData>>();
        private Action _completeLoad;
        private readonly List<string> _dataFiles = new List<string>();
        #endregion

        #region Constructor
        internal DictionaryMemoryEntity(Action completeLoad) : base(null) {
            for (var i = (int)'a'; i <= (int)'z'; i++) {
                _dataFiles.Add(((char)i).ToString());
                for (var j = (int)'a'; j <= (int)'z'; j++) {
                    _dataFiles.Add(((char)i).ToString() + ((char)j).ToString());
                }
            }

            this._completeLoad = completeLoad;
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
                var dirs = new string[] { Constants.EijiroData, Constants.WebsterData };
                foreach(var dir in dirs) {
                    var files = Directory.GetFiles(dir);
                    foreach (var f in files) {
                        using (var op = new FileOperator(f,FileOperator.OpenMode.Read)) {
                            if (!this._memoryData.ContainsKey(op.NameWithoutExtension)) {
                                this._memoryData[op.NameWithoutExtension] = new List<DictionaryData>();
                            }
                            while (!op.Eof) {
                                var line = op.ReadLine().Split('\t');
                                var data = new DictionaryData {
                                    Word = line[0],
                                    WordSort = line[0].ToLower(),
                                    Data = line[1]
                                };
                                this._memoryData[op.NameWithoutExtension].Add(data);
                            }
                        }

                    }
                }

                System.Diagnostics.Debug.WriteLine("◆◆◆ READ END" + DateTime.Now.ToString("hh:mm:ss fff"));
                this._isBusy = false;
                this._completeLoad?.Invoke();
            });
        }

        private List<DictionaryData> SearchExact(string word) {
            var found = false;
            List<DictionaryData> result = new List<DictionaryData>();
            var nm = AppUtil.convertToFileName(word);
            if (this._memoryData.ContainsKey(nm)) {
                var list = this._memoryData[nm];
                for (var i = 0; i < list.Count; i++) {
                    if (list[i].WordSort == word) {
                        found = true;
                        result.Add(list[i]);
                    } else {
                        if (found) {
                            break;
                        }
                    }
                }
            }
            return (result.Count == 0) ? null : result;
        }
        private List<DictionaryData> SearchPrefix(string word) {
            var count = 0;
            List<DictionaryData> result = new List<DictionaryData>();
            var nm = AppUtil.convertToFileName(word);
            if (this._memoryData.ContainsKey(nm)) {
                var list = this._memoryData[nm];
                for (var i = 0; i < list.Count; i++) {
                    if (list[i].WordSort.StartsWith(word)) {
                        count++;
                        result.Add(list[i]);
                        if (Constants.MaxNumberOfListWord <= count) {
                            break;
                        }
                    }
                }
            }

            return (result.Count == 0) ? null : result;
        }
        #endregion
    }
}
