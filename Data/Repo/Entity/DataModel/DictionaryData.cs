namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {

    /// <summary>
    /// dictionary data
    /// </summary>
    internal class DictionaryData {

        #region Public Property
        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// source id
        /// </summary>
        public int SourceId { set; get; }

        /// <summary>
        /// word
        /// </summary>
        internal string Word { set; get; } = "";

        /// <summary>
        /// word for sort
        /// </summary>
        internal string WordSort { set; get; } = "";

        /// <summary>
        /// html formated data
        /// </summary>
        internal string Data { set; get; } = "";
        #endregion
    }
}
