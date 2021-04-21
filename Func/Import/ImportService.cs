using OsnCsLib.File;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {

    /// <summary>
    /// import dictionary data
    /// </summary>
    internal class ImportService {

        #region Declaration
        private readonly IImportServiceCallback _callback;
        #endregion

        #region Constructor
        public ImportService(IImportServiceCallback callback) {
            this._callback = callback;
        }
        #endregion

        #region Public Method
        internal void Start(DicType dicType, string file) {
            var processName = "";
            try {
                string rootDir;
                IDictionaryParser parser;
                WordData data = null;
                
                switch (dicType) {
                    case DicType.Eijiro:
                        rootDir = Constants.EijiroData;
                        parser = new EijiroParser(file);
                        break;
                    case DicType.Webster:
                        rootDir = Constants.WebsterData;
                        parser = new WebsterParser(file);
                        break;
                    default:
                        throw new Exception($"unknown key type : {dicType}");
                }



                // delete all index files
                var files = Directory.GetFiles(rootDir);
                foreach(var f in files) {
                    File.Delete(f);
                }

                // get filename
                var dataFiles = new List<string>();
                for (var i = (int)'a'; i <= (int)'z'; i++) {
                    dataFiles.Add(((char)i).ToString());
                    for (var j = (int)'a'; j <= (int)'z'; j++) {
                        dataFiles.Add(((char)i).ToString() + ((char)j).ToString());
                    }
                }

                this._callback.OnPrepared(parser.GetRowCount((long rowCount) => {
                    this._callback.OnPrepared(rowCount);
                }));

                FileOperator op = null;
                string currentNm = "";
                while ((data = parser.Read()) != null) {
                    this._callback.OnProceed(parser.CurrentLine);

                    string nm = "";
                    if (1 == data.Word.Length) {
                        nm = data.Word.ToLower();
                    } else {
                        nm = data.Word.Substring(0, 2).ToLower();
                    }
                    if (!dataFiles.Contains(nm)) {
                        nm = "!!";
                    }
                    
                    if (currentNm != nm) {
                        currentNm = nm;
                        op?.Close();
                        op = new FileOperator($@"{rootDir}\{nm}",FileOperator.OpenMode.Write);
                    }
                    op.WriteLine($"{data.Word}\t{this.GetDisplayData(data)}");
                }
                op?.Close();
                this._callback.OnSuccess();
            } catch (Exception ex) {
                this._callback.OnFail(processName + "\n" + ex.Message);
            }
        }
        #endregion

        #region Private Method
        private string GetDisplayData(WordData data) {

            var body = new StringBuilder();

            body.Append("<main>");
            body.Append($"<h1>{data.Word}</h1>");
            var info = this.GetInfo(data);
            if (0 < info.Length) {
                body.Append($"<div class='info'>{info}</div>");
            }
            var startUl = false;
            var partOfSpeech = "";
            foreach (var meaning in data.Meanings) {
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
                        body.Append("</ul>");
                        body.Append("</div>");
                    }
                    startUl = true;
                    body.Append($"<div{className}>");
                    if (0 < meaning.PartOfSpeach.Length) {
                        body.Append($"<h4>{meaning.PartOfSpeach}</h4>");
                    }
                    body.Append($"<ul{className}>");
                }
                body.Append($"<li>{meaning.Meaning}");

                if (0 < meaning.Additions.Count) {
                    body.Append("<div class='note'>");
                    for (var j = 0; j < meaning.Additions.Count; j++) {
                        var addition = meaning.Additions[j];
                        switch (addition.Type) {
                            case Constants.AdditionType.Supplement:
                                body.Append($"<span class='supplement'>{addition.Data}</span>");
                                break;
                            case Constants.AdditionType.Example:
                                body.Append($"<span class='example'>{addition.Data}</span>");
                                break;
                        }
                        if (j < meaning.Additions.Count - 1) {
                            body.Append("<br/>");
                        }
                    }
                    body.Append("</div>");
                }
                body.Append("</li>");
                partOfSpeech = meaning.PartOfSpeach;
            }
            if (startUl) {
                body.Append("</ul>");
                body.Append("</div>");
            }
            body.Append("</div>");
            body.Append("</main>");
            return body.ToString();
        }


        private string GetInfo(WordData data) {
            var info = new StringBuilder();
            if (0 < data.Syllable.Length) {
                info.Append($"<span class='syllable'>音節</span> {data.Syllable}&nbsp;&nbsp;");
            }
            if (0 < data.Pronunciation.Length) {
                info.Append($"<span class='pronumciation'>発音</span> {data.Pronunciation}");
                if (0 < data.Kana.Length) {
                    info.Append($"({data.Kana})");
                }
                info.Append("&nbsp;&nbsp;");
            }
            if (0 < data.Change.Length) {
                info.Append($"<span class='change'>変化</span> {data.Change}&nbsp;&nbsp;");
            }
            return info.ToString();
        }
        #endregion
    }
}
