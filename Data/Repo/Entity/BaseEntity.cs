namespace SimpleTranslationLocal.Data.Repo.Entity {

    /// <summary>
    /// ベースエンティティ
    /// </summary>
    internal abstract class BaseEntity {

        #region Public Property
        internal DictionaryDatabase Database { set; get; }
        #endregion

        #region Constructor
        internal BaseEntity(DictionaryDatabase database) {
            this.Database = database;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// delete data by source id
        /// </summary>
        /// <param name="id"></param>
        internal abstract void DeleteBySourceId(long id);

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
        #endregion

    }
}
