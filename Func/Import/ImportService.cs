using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {

    /// <summary>
    /// import dictionary data
    /// </summary>
    internal class ImportService {

        #region Declaration
        private readonly IImportServiceCallback _callback;
        private DictionaryRepo _dictionaryRepo;
        #endregion

        #region Constructor
        public ImportService(IImportServiceCallback callback) {
            this._callback = callback;
        }
        #endregion

        #region Public Method
        internal void Start(Dictionary<DicType, string> targetList) {
            var processName = "";
            try {
                IDictionaryParser parser;
                WordData data = null;

                // commit by dictionary
                using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                    database.SyncMode = OsnLib.Data.Sqlite.Database.SynchModeEnum.Off;
                    database.JournalMode = OsnLib.Data.Sqlite.Database.JournalModeEnum.Truncate;
                    database.Open();

                    this._dictionaryRepo = new DictionaryRepo(database);

                    foreach (var item in targetList) {
                        var id = (int)item.Key;
                        var file = item.Value;


                        processName = "Delete Data";
                        DeleteBySourceId(id, database);

                        processName = "Select import file";
                        switch (item.Key) {
                            case DicType.Eijiro:
                                parser = new EijiroParser(file);
                                break;
                            case DicType.Webster:
                                parser = new WebsterParser(file);
                                break;
                            default:
                                throw new Exception("unknown key type : " + item.Key);
                        }

                        processName = "Count Rows";
                        this._callback.OnPrepared(parser.GetRowCount((long rowCount) => {
                            this._callback.OnPrepared(rowCount);
                        }));

                        processName = "Create Source Data";
                        this.CreateSourceData(id, file, database);

                        processName = "Create Dic Data";
                        database.BeginTrans();
                        while ((data = parser.Read()) != null) {
                            this.CreateDicData(id, data);
                            this._callback.OnProceed(parser.CurrentLine);
                            if (0 < parser.CurrentLine && parser.CurrentLine % 100 == 0) {
                                // this._callback.OnProceed(parser.CurrentLine); // reduce refresh screen
                                database.CommitTrans();
                                database.BeginTrans();
                            }
                        }

                        this._callback.OnProceed(parser.CurrentLine);   // if last line is invalid data, curren line is not update. so update here.
                        if (database.IsIntrans()) {
                            database.CommitTrans();
                        }
                        this._callback.OnSuccess();
                    }
                }
            } catch (Exception ex) {
                // Messages.ShowError(Messages.ErrId.Err003, processName, ex.Message);
                this._callback.OnFail(processName + "\n" + ex.Message);
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// delete data by source id
        /// </summary>
        /// <param name="id">source id</param>
        /// <param name="database">database </param>
        private void DeleteBySourceId(int id, DictionaryDatabase database) {
            // the order of delete tables is important
            new SourcesRepo(database).DeleteBySourceId(id);
            new DictionaryRepo(database).DeleteBySourceId(id);
        }

        /// <summary>
        /// create source table data
        /// </summary>
        /// <param name="id">source id</param>
        /// <param name="file">source file</param>
        /// <param name="database">database</param>
        private void CreateSourceData(long id, string file, DictionaryDatabase database) {
            var sourceData = new SourceData() {
                Id = id,
                Name = Constants.DicTypeName[(DicType)id],
                Priority = (int)id,
                File = file
            };

            var sourceRepo = new SourcesRepo(database);
            sourceRepo.SetDataModel(sourceData);
            sourceRepo.Insert();
        }

        /// <summary>
        /// 辞書データを作成する
        /// </summary>
        /// <param name="id">ソースID</param>
        /// <param name="data">作成するデータ</param>
        private void CreateDicData(int id, WordData data) {
            var dictionaryData = new DictionaryData() {
                SourceId = id,
                Word = data.Word,
                Data = this.GetDisplayData(data)
            };
            this._dictionaryRepo.SetDataModel(dictionaryData);
            this._dictionaryRepo.Insert();
        }


        private string GetDisplayData(WordData data) {

            var body = new StringBuilder();

            body.AppendLine("<main>");
            body.AppendLine($"<h1>{data.Word}</h1>");
            var info = this.GetInfo(data);
            if (0 < info.Length) {
                body.AppendLine($"<div class='info'>{info}</div>");
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
                        switch (addition.Type) {
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
            return body.ToString();
        }


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
