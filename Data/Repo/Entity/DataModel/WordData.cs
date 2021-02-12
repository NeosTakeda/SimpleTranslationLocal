using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {

    /// <summary>
    /// word data model
    /// </summary>
    internal class WordData {

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
        /// pronunciation
        /// </summary>
        internal string Pronunciation { set; get; } = "";

        /// <summary>
        /// syllable
        /// </summary>
        internal string Syllable { set; get; } = "";

        /// <summary>
        /// kana of syllable
        /// </summary>
        internal string Kana { set; get; } = "";

        /// <summary>
        /// level
        /// </summary>
        internal int Level { set; get; } = 0;

        /// <summary>
        /// change
        /// </summary>
        internal string Change { set; get; } = "";

        /// <summary>
        /// meaning list
        /// </summary>
        internal List<MeaningData> Meanings { set; get; } = new List<MeaningData>();
        #endregion

    }
}
