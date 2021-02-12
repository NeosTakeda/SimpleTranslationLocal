namespace SimpleTranslationLocal.Data.Repo.Entity {

    /// <summary>
    /// ベースエンティティ
    /// </summary>
    internal abstract class BaseEntity {

        #region Public Property
        public DictionaryDatabase Database { set; get; }
        #endregion

        #region Constructor
        public BaseEntity(DictionaryDatabase database) {
            this.Database = database;
        }
        #endregion

        #region Public Method
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
        #endregion

    }
}
