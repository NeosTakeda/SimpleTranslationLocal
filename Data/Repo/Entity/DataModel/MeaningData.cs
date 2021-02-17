using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {

    /// <summary>
    /// meaning data model
    /// </summary>
    internal class MeaningData {

        #region Public Property
        /// <summary>
        /// id
        /// </summary>
        internal long Id { set; get; }

        /// <summary>
        /// word id
        /// </summary>
        internal long WordId { set; get; }

        /// <summary>
        /// word meaning
        /// </summary>
        internal string Meaning { set; get; } = "";

        /// <summary>
        /// part of speach
        /// </summary>
        internal string PartOfSpeach { set; get; } = "";

        /// <summary>
        /// source id
        /// </summary>
        public int SourceId { set; get; }

        /// <summary>
        /// addition data list
        /// </summary>
        internal List<AdditionData> Additions { set; get; } = new List<AdditionData>();
        #endregion

    }
}
