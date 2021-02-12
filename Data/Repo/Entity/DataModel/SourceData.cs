namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {

    /// <summary>
    /// source data model
    /// </summary>
    internal class SourceData {

        #region Public Property
        internal static readonly string TableName = "sources";
        /// <summary>
        /// id
        /// </summary>
        internal long Id { set; get; }

        /// <summary>
        /// source name
        /// </summary>
        internal string Name { set; get; }

        /// <summary>
        /// source priority
        /// </summary>
        internal int Priority { set; get; }

        /// <summary>
        /// source file
        /// </summary>
        internal string File { set; get; }
        #endregion

    }
}
