using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {

    /// <summary>
    /// import dictionary data
    /// </summary>
    internal class ImportService : IImportService {

        #region Declaration
        private WordsRepo _wordRepo;
        private MeaningsRepo _meaningRepo;
        private AdditionsRepo _additionsRepo;
        #endregion

        #region Constructor
        public ImportService(ImportServiceCallback callback) : base(callback) {
        }
        #endregion

        #region Public Method
        internal override void Start(Dictionary<DicType, string> targetList) {
            var processName = "";
            try {
                IDictionaryParser parser;
                WordData data = null;

                // commit by dictionary
                using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                    database.Open();

                    this._wordRepo = new WordsRepo(database);
                    this._meaningRepo = new MeaningsRepo(database);
                    this._additionsRepo = new AdditionsRepo(database);

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
                            this.CreateDicData(id, data, database);
                            this._callback.OnProceed(parser.CurrentLine);
                            if (0 < parser.CurrentLine && parser.CurrentLine % 100 == 0) {
                                database.CommitTrans();
                                database.BeginTrans();
                            }
                        }
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
            new AdditionsRepo(database).DeleteBySourceId(id);
            new MeaningsRepo(database).DeleteBySourceId(id);
            new WordsRepo(database).DeleteBySourceId(id);
            new SourcesRepo(database).DeleteBySourceId(id);
        }

        /// <summary>
        /// create source table data
        /// </summary>
        /// <param name="id">source id</param>
        /// <param name="file">source file</param>
        /// <param name="database">database</param>
        private void CreateSourceData(long id, string file, DictionaryDatabase database) {
            var sourceData = new SourceData();
            sourceData.Id = id;
            sourceData.Name = Constants.DicTypeName[(DicType)id];
            sourceData.Priority = (int)id;
            sourceData.File = file;

            var sourceRepo = new SourcesRepo(database);
            sourceRepo.SetDataModel(sourceData);
            sourceRepo.Insert();
        }

        /// <summary>
        /// 辞書データを作成する
        /// </summary>
        /// <param name="id">ソースID</param>
        /// <param name="data">作成するデータ</param>
        /// <param name="database">データベースのインスタンス</param>
        private void CreateDicData(int id, WordData data, DictionaryDatabase database) {
            // words
            data.SourceId = id;
            this._wordRepo.SetDataModel(data);
            var wordId = this._wordRepo.Insert();

            // meanings/additions
            foreach (var meaning in data.Meanings) {
                meaning.WordId = wordId;
                this._meaningRepo.SetDataModel(meaning);
                var meaningId = this._meaningRepo.Insert();
                foreach (var addition in meaning.Additions) {
                    addition.MeaningId = meaningId;
                    this._additionsRepo.SetDataModel(addition);
                    this._additionsRepo.Insert();
                }
            }
        }
        #endregion
    }
}
