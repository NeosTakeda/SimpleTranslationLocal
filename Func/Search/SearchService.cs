using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Func.Search {
    class SearchService {

        #region Declaration
        private readonly DictionaryRepo _repo;
        #endregion

        #region Constructor 
        public SearchService() {
            this._repo = new DictionaryRepo();
        }
        #endregion

        #region Public Method
        internal List<DictionaryData> Search(string keyword) {

            List<DictionaryData> result = null;

            var matchTypes = new List<Constants.MatchType>
                    { Constants.MatchType.Exact, Constants.MatchType.Prefix};
            foreach (var matchType in matchTypes) {
                result = this._repo.Search(keyword, matchType);
                if (null != result && 0 < result.Count) {
                    break;
                }
            }
            return result;
        }
        #endregion
    }
}
