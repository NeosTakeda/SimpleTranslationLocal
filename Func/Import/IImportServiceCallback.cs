namespace SimpleTranslationLocal.Func.Import {
    interface IImportServiceCallback {
        /// <summary>
        /// インポート準備完了通知
        /// </summary>
        /// <param name="totalCount">トータル件数</param>
        void OnPrepared(long totalCount);

        /// <summary>
        /// インポートの処理開始(１件単位で通知)
        /// </summary>
        /// <param name="count">現在の件数</param>
        void OnProceed(long count);

        /// <summary>
        /// インポート完了(成功)
        /// </summary>
        void OnSuccess();

        /// <summary>
        /// インポート完了(失敗)
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        void OnFail(string errorMessage);
    }
}
