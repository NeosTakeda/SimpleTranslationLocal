using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Func.Search {
    internal class SearchMockService: ISearchService {

        #region Public Method
        internal override List<WordData> Search(string keyword) {
            return new List<WordData>();
        }
        #endregion
    }
}
