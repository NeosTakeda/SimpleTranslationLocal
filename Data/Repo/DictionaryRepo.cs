using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Collections.Generic;
using System.IO;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// dictionary repo
    /// </summary>
    internal class DictionaryRepo {

        #region Declaration
        private string _searchWord = "";
        // １つ目のキーは辞書ファイル単位で作成しているフォルダ
        // ２つ目のキーはディレクトリを含むファイル名
        private readonly Dictionary<string,Dictionary<string, List<DictionaryData>>> _wordList 
            = new Dictionary<string, Dictionary<string, List<DictionaryData>>>();
        #endregion

        #region Constructor
        internal DictionaryRepo() {
            foreach (var dir in Constants.DataDirs) {
                this._wordList[dir] = new Dictionary<string, List<DictionaryData>>();
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// search dictionary
        /// </summary>
        /// <param name="word">word</param>
        /// <param name="matchType">search matching type</param>
        /// <returns>result. if data does not find, return null</returns>
        internal List<DictionaryData> Search(string word, MatchType matchType) {
            return SearchData(word, matchType);
        }

        /// <summary>
        /// search dictionary
        /// </summary>
        /// <param name="word">word</param>
        /// <param name="matchType">search matching type</param>
        /// <returns>result. if data does not find, return null</returns>
        internal List<DictionaryData> SearchData(string word, MatchType matchType) {
            // create search list
            var nm = AppUtil.ConvertToFileName(word);
            if (nm != this._searchWord) {
                this._searchWord = nm;
                foreach(var dir in Constants.DataDirs) {
                    if (!this._wordList[dir].ContainsKey(nm)) {
                        var file = $@"{dir}\{nm}";
                        if (File.Exists(file)) {
                            this._wordList[dir][nm] = new List<DictionaryData>();
                            using (var op = new FileOperator($@"{dir}\{nm}", FileOperator.OpenMode.Read)) {
                                while (!op.Eof) {
                                    var line = op.ReadLine().Split('\t');
                                    var data = new DictionaryData {
                                        Word = line[0],
                                        WordSort = line[0].ToLower(),
                                        Data = line[1]
                                    };
                                    this._wordList[dir][nm].Add(data);
                                }
                            }
                        }
                    }
                }
            }

            List<DictionaryData> result = null;
            switch (matchType) {
                case MatchType.Exact:
                    result = SearchExact(word);
                    break;

                case MatchType.Prefix:
                    result = SearchPrefix(word);
                    break;
            }

            return result;
        }
        #endregion


        #region Private Method
        /// <summary>
        /// 完全一致検索
        /// </summary>
        /// <param name="word">検索文字列</param>
        /// <returns>検索結果</returns>
        private List<DictionaryData> SearchExact(string word) {
            var result = new List<DictionaryData>();
            var fileName = AppUtil.ConvertToFileName(word);
            foreach (var dir in Constants.DataDirs) {
                if (this._wordList[dir].ContainsKey(fileName)) {
                    foreach (var data in this._wordList[dir][fileName]) {
                        //if (data.WordSort == word) {
                        if (data.WordSort.ToLower() == word.ToLower()) {
                            result.Add(data);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 前方一致検索
        /// </summary>
        /// <param name="word">検索文字列</param>
        /// <returns>検索結果</returns>
        private List<DictionaryData> SearchPrefix(string word) {
            int count;
            bool found;
            var result = new List<DictionaryData>();
            var fileName = AppUtil.ConvertToFileName(word);
            foreach (var dir in Constants.DataDirs) {
                found = false;
                count = 0;
                if (this._wordList[dir].ContainsKey(fileName)) {
                    foreach (var data in this._wordList[dir][_searchWord]) {
                        //                        if (data.WordSort.StartsWith(word)) {
                        if (data.WordSort.ToLower().StartsWith(word.ToLower())) {
                            result.Add(data);
                            found = true;
                            count++;
                            if (Constants.MaxNumberOfListWord / 2 < count) {
                                break;
                            }
                        } else {
                            if (found) {
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
