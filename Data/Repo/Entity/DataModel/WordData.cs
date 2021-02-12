using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.Repo.Eneity.DataModel {

    /// <summary>
    /// 用語 データモデル
    /// </summary>
    internal class WordData {

        #region Public Property
        /// <summary>
        /// 用語
        /// </summary>
        internal string Word { set; get; } = "";

        /// <summary>
        /// 発音
        /// </summary>
        internal string Pronumciation { set; get; } = "";

        /// <summary>
        /// 音節
        /// </summary>
        internal string Syllable { set; get; } = "";

        /// <summary>
        /// よみがな
        /// </summary>
        internal string Kana { set; get; } = "";

        /// <summary>
        /// レベル
        /// </summary>
        internal int Level { set; get; } = 0;

        /// <summary>
        /// 変化
        /// </summary>
        internal string Change { set; get; } = "";

        /// <summary>
        /// 意味
        /// </summary>
        internal List<MeaningData> Meanings { set; get; } = new List<MeaningData>();
        #endregion

    }
}
