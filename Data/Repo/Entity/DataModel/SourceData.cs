namespace SimpleTranslationLocal.Data.Repo.Eneity.DataModel {

    /// <summary>
    /// 辞書情報取得元 データモデル
    /// </summary>
    internal class SourceData {

        #region Public Property
        internal static readonly string TableName = "sources";
        /// <summary>
        /// id
        /// </summary>
        internal long Id { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        internal string Name { set; get; }

        /// <summary>
        /// 優先順位
        /// </summary>
        internal int Priority { set; get; }

        /// <summary>
        /// ファイル名
        /// </summary>
        internal string File { set; get; }
        #endregion

    }
}
