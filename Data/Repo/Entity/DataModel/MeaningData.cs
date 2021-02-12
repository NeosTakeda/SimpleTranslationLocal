using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.Repo.Eneity.DataModel {

    /// <summary>
    /// 意味 データモデル
    /// </summary>
    internal class MeaningData {

        #region Public Property
        /// <summary>
        /// 意味
        /// </summary>
        internal string Meaning { set; get; } = "";

        /// <summary>
        /// 品詞
        /// </summary>
        internal string PartOfSpeach { set; get; } = "";

        /// <summary>
        /// 付加情報
        /// </summary>
        internal List<AdditionData> Additions { set; get; } = new List<AdditionData>();
        #endregion

    }
}
