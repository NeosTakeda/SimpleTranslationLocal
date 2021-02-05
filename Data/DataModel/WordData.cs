using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.DataModel {
    /// <summary>
    /// 用語
    /// </summary>
    class WordData {

        #region Public Property
        /// <summary>
        /// 用語
        /// </summary>
        public string Word { set; get; } = "";

        /// <summary>
        /// 発音
        /// </summary>
        public string Pronumciation { set; get; } = "";

        /// <summary>
        /// 音節
        /// </summary>
        public string Syllable { set; get; } = "";

        /// <summary>
        /// 意味
        /// </summary>
        public List<MeaningData> Meanings { set; get; } = new List<MeaningData>();
        #endregion

    }
}
