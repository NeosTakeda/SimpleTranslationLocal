using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.DataModel {
    /// <summary>
    /// 意味
    /// </summary>
    class MeaningData {

        #region Public Property
        /// <summary>
        /// 意味
        /// </summary>
        public string Meaning { set; get; } = "";

        /// <summary>
        /// 品詞
        /// </summary>
        public string PartOfSpeach { set; get; } = "";

        /// <summary>
        /// 付加情報
        /// </summary>
        public List<AdditionData> Additions { set; get; } = new List<AdditionData>();
        #endregion

    }
}
