using SimpleTranslationLocal.AppCommon;

namespace SimpleTranslationLocal.Data.Repo.Entity.DataModel {
    /// <summary>
    /// addition data model
    /// </summary>
    class AdditionData {

        #region Public Property
        /// <summary>
        /// id
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// meaning id
        /// </summary>
        public long MeaningId { set; get; }

        /// <summary>
        /// addition type. see also Constants.AdditionType
        /// </summary>
        public int Type { set; get; } = Constants.AdditionType.Unknown;

        /// <summary>
        /// addition data
        /// </summary>
        public string Data { set; get; } = "";
        #endregion

        #region  Constructor
        public AdditionData() { }
        public AdditionData(int Type, string Data) {
            this.Type = Type;
            this.Data = Data;
        }
        #endregion

    }
}
