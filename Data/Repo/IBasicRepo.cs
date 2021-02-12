using SimpleTranslationLocal.Data.Repo.Entity;

namespace SimpleTranslationLocal.Data.Repo {
    internal abstract class IBasicRepo<T> {

        #region Public Property
        /// <summary>
        /// データベース
        /// </summary>
        internal DictionaryDatabase Database { set; get; }
        #endregion

        #region Constructor
        internal IBasicRepo(DictionaryDatabase database) {
            this.Database = database;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// create table
        /// </summary>
        /// <returns>true: success, false: otherwise</returns>
        internal abstract bool Create();

        /// <summary>
        /// insert data
        /// </summary>
        /// <returns>if success return id, else return -1</returns>
        internal abstract long Insert();

        /// <summary>
        /// delete data by source id
        /// </summary>
        /// <param name="id"></param>
        internal abstract void DeleteBySourceId(long id);

        /// <summary>
        /// copy model data to member.
        /// </summary>
        /// <param name="model"></param>
        internal abstract void SetDataModel(T model);
        #endregion
    }
}
