using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.Func.Search {

    /// <summary>
    /// search service
    /// </summary>
    internal abstract class ISearchService {

        #region Public Method
        /// <summary>
        /// Search word
        /// </summary>
        /// <returns>search result. if not found, return null</returns>
        internal abstract List<DictionaryData> Search(string keyword);
        #endregion
    }
}
