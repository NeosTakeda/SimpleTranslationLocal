using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Func.Search {
    class SearchService : ISearchService {

        #region Declaration
        private readonly DictionaryRepo _repo;
        #endregion

        #region Constructor 
        public SearchService(bool useMemoryDic) {
            this._repo = new DictionaryRepo(useMemoryDic);
        }
        #endregion

        #region Public Method
        internal override List<DictionaryData> Search(string keyword) {

            List<DictionaryData> result = null;

            var matchTypes = new List<Constants.MatchType>
                    { Constants.MatchType.Exact, Constants.MatchType.Prefix, Constants.MatchType.Broad};
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
