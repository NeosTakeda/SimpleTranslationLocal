using OsnCsLib.File;
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
                body.AppendLine("<main>");
                body.AppendLine($"<h1>{data.Word}</h1>");
                var info = this.GetInfo(data);
                if (0 < info.Length) {
                    body.AppendLine($"<div class='info'>{info}</div>");
                }
                var startUl = false;
                var partOfSpeech = "";
                foreach(var meaning in data.Meanings) {
                    var className = "";
                    switch ((Constants.DicType)meaning.SourceId) {
                        case Constants.DicType.Eijiro:
                            className = " class='eijiro'";
                            break;
                        case Constants.DicType.Webster:
                            className = " class='webster'";
                            break;
                    }

                    if (meaning.PartOfSpeach == "" || partOfSpeech != meaning.PartOfSpeach) {
                        if (startUl) {
                            body.AppendLine("</ul>");
                            body.AppendLine("</div>");
                        }
                        startUl = true;
                        body.AppendLine($"<div{className}>");
                        if (0 < meaning.PartOfSpeach.Length) {
                            body.AppendLine($"<h4>{meaning.PartOfSpeach}</h4>");
                        }
                        body.AppendLine($"<ul{className}>");
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
                    body.AppendLine("</div>");
                }
                body.AppendLine("</div>");
                body.AppendLine("</main>");
            }
            this._browser.NavigateToString(this._templateHtml.Replace("@body@", body.ToString()));
            this._completeSearch?.Invoke();
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
                info.AppendLine($"<span class='change'>変化</span> {data.Change}&nbsp;&nbsp;");
            }
            return info.ToString();
        }
        #endregion

    }
}
