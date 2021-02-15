using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Func.Search {
    class SearchService : ISearchService {

        #region Public Method
        internal override List<WordData> Search(string keyword) {

            List<WordData> result = null;

            using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                database.Open();
                var repo = new WordsRepo(database);

                var matchTypes = new List<Constants.MatchType>
                        { Constants.MatchType.Exact, Constants.MatchType.Prefix, Constants.MatchType.Broad};
                foreach (var matchType in matchTypes) {
                    result = repo.Search(keyword, matchType);
                    if (null != result && 0 < result.Count) {
                        break;
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
