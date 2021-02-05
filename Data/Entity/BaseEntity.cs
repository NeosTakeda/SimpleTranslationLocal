namespace SimpleTranslationLocal.Data.Entity {
    /// <summary>
    /// base entity
    /// </summary>
    internal abstract class BaseEntity {

        #region Public Property
        /// <summary>
        /// データベース
        /// </summary>
        public DictionaryDatabase Database { set; get; }

        ///// <summary>
        ///// テーブル名
        ///// </summary>
        //public abstract string TableName { get; }
        #endregion

        #region Constructor
        public BaseEntity(DictionaryDatabase database) {
            this.Database = database;
        }
        #endregion

        #region Public Class
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
