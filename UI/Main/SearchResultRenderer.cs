﻿using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using SimpleTranslationLocal.Func.Search;
using System;
using System.Text;
using System.Windows.Controls;

namespace SimpleTranslationLocal.UI.Main {

    #region Extension
    public static class StringBuiderEx {
        public static void AppendLine(this StringBuilder sb, string val) {
            if (Env.Current == Env.EnvType.Release) {
                sb.Append(val);
            } else {
                sb.Append(val).Append("\r\n");
            }
        }
    }
    #endregion

    /// <summary>
    /// render seach result
    /// </summary>
    internal class SearchResultRenderer {

        #region Declaration
        private string _templateHtml = "";
        private string _nodataHtml = "";
        private WebBrowser _browser;
        private SearchService _service;

        private Action _completeSearch = null;
        #endregion

        #region Constructor
        internal SearchResultRenderer(WebBrowser browser, Action completeSearch = null) {
            this._browser = browser;
            this._completeSearch = completeSearch;
            this._service = new SearchService();
            using (var file = new FileOperator(Constants.TemplateHtmlFile)) {
                this._templateHtml = file.ReadAll();
            }
            using (var file = new FileOperator(Constants.NoDataHtmlFile)) {
                this._nodataHtml = file.ReadAll();
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// show search word
        /// </summary>
        /// <param name="word"></param>
        internal void Search(string word) {
            var result = this._service.Search(word);

            if (null == result) {
                this._browser.NavigateToString(this._nodataHtml);
                this._completeSearch?.Invoke();
                return;
            }

            var body = new StringBuilder();
            for (var i = 0; i < result.Count; i++) {
                var data = result[i];
                if (0 < i) {
                    body.AppendLine("<hr/>");
                }
                body.AppendLine(data.Data);
            }
            this._browser.NavigateToString(this._templateHtml.Replace("@body@", body.ToString()));
            this._completeSearch?.Invoke();
        }
        #endregion
    }
}
