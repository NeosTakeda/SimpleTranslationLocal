using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using SimpleTranslationLocal.Func.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleTranslationLocal.UI.Main {

    public static class StringBuiderEx {
        public static void AppendLine(this StringBuilder sb, string val) {
            if (Env.Current == Env.EnvType.Release) {
                sb.Append(val);
            } else {
                sb.Append(val).Append("\r\n");
            }
        }
    }

    /// <summary>
    /// render seach result
    /// </summary>
    internal class SearchResultRenderer {

        #region Declaration
        private string _templateHtml = "";
        private string _nodataHtml = "";
        private WebBrowser _browser;
        private SearchService _service;
        #endregion

        #region Constructor
        internal SearchResultRenderer(WebBrowser browser) {
            this._browser = browser;
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
                return;
            }

            var body = new StringBuilder();
            for (var i = 0; i < result.Count; i++) {
                var data = result[i];
                if (0 < i) {
                    body.AppendLine("<hr/>");
                }
                body.AppendLine("<main>");
                body.AppendLine($"<h1>{data.Word}</h1>");
                var info = this.GetInfo(data);
                if (0 < info.Length) {
                    body.AppendLine($"<div class='info'>{info}</div>");
                }
                var startUl = false;
                var partOfSpeech = "";
                foreach(var meaning in data.Meanings) {
                    if (meaning.PartOfSpeach == "" || partOfSpeech != meaning.PartOfSpeach) {
                        if (startUl) {
                            body.AppendLine("</ul>");
                            body.AppendLine("</section>");
                        }
                        startUl = true;
                        body.AppendLine("<section>");
                        body.AppendLine($"<h4>{meaning.PartOfSpeach}</h4>");
                        body.AppendLine("<ul>");
                    }
                    body.AppendLine($"<li>{meaning.Meaning}");

                    if (0 < meaning.Additions.Count) {
                        body.AppendLine("<div class='note'>");
                        for (var j = 0; j < meaning.Additions.Count; j++) {
                            var addition = meaning.Additions[j];
                            switch(addition.Type) {
                                case Constants.AdditionType.Supplement:
                                    body.AppendLine($"<span class='supplement'>{addition.Data}</span>");
                                    break;
                                case Constants.AdditionType.Example:
                                    body.AppendLine($"<span class='example'>{addition.Data}</span>");
                                    break;
                            }
                            if (j < meaning.Additions.Count - 1) {
                                body.AppendLine("<br/>");
                            }
                        }
                        body.AppendLine("</div>");
                    }
                    body.AppendLine("</li>");
                    partOfSpeech = meaning.PartOfSpeach;
                }
                if (startUl) {
                    body.AppendLine("</ul>");
                    body.AppendLine("</section>");
                }
                body.AppendLine("</section>");
                body.AppendLine("</main>");
            }
            this._browser.NavigateToString(this._templateHtml.Replace("@body@", body.ToString()));
        }
        #endregion

        #region Private Method
        private string GetInfo(WordData data) {
            var info = new StringBuilder();
            if (0 < data.Syllable.Length) {
                info.AppendLine($"<span class='syllable'>音節</span> {data.Syllable}&nbsp;&nbsp;");
            }
            if (0 < data.Pronunciation.Length) {
                info.AppendLine($"<span class='pronumciation'>発音</span> {data.Pronunciation}");
                if (0 < data.Kana.Length) {
                    info.AppendLine($"({data.Kana})");
                }
                info.AppendLine("&nbsp;&nbsp;");
            }
            if (0 < data.Change.Length) {
                info.AppendLine($"<span class='syllable'>変化</span> {data.Change}&nbsp;&nbsp;");
            }
            return "";
        }
        #endregion

    }
}
