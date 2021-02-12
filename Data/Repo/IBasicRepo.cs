using SimpleTranslationLocal.Data.Entity;

namespace SimpleTranslationLocal.Data.Repo {
    abstract class IBasicRepo<T> {

        #region Public Property
        /// <summary>
        /// データベース
        /// </summary>
        public DictionaryDatabase Database { set; get; }
        #endregion

        #region Constructor
        public IBasicRepo(DictionaryDatabase database) {
            this.Database = database;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// create table
        /// </summary>
        public abstract void CreateTable();

        /// <summary>
        /// delete data by source id
        /// </summary>
        /// <param name="id"></param>
        public abstract void DeleteBySourceId(long id);

        /// <summary>
        /// create table
        /// </summary>
        /// <returns>true: success, false: otherwise</returns>
        public abstract bool Create();

        /// <summary>
        /// insert data
        /// </summary>
        /// <returns>if success return id, else return -1</returns>
        public abstract long Insert();

        /// <summary>
        /// copy model data to member.
        /// </summary>
        /// <param name="model"></param>
        public abstract void SetDataModel(T model);
        #endregion
    }
}
