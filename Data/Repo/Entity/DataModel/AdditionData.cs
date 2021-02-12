using SimpleTranslationLocal.AppCommon;

namespace SimpleTranslationLocal.Data.Repo.Eneity.DataModel {
    /// <summary>
    /// 付加情報
    /// </summary>
    class AdditionData {
        #region Public Property
        /// <summary>
        /// 種別
        /// </summary>
        public int Type { set; get; } = Constants.AdditionType.Unknown;

        /// <summary>
        /// データ
        /// </summary>
        public string Data { set; get; } = "";
        #endregion

        #region  Constructor
        public AdditionData(int Type, string Data) {
            this.Type = Type;
            this.Data = Data;
        }
        #endregion
    }
}
