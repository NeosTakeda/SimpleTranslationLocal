using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System;
using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Func.Import {
    abstract class IImportService {

        #region Declaration
        protected ImportServiceCallback _callback;
        #endregion

        #region Constructor
        public IImportService(ImportServiceCallback callback) {
            this._callback = callback;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// インポート処理を開始する
        /// </summary>
        /// <param name="targetList">インポート対象のリスト</param>
        public void Start(Dictionary<DicType, string> targetList) {
            var processName = "";
            try {
                IDictionaryParser parser;
                WordData data = null;

                foreach (var item in targetList) {
                    var id = (int)item.Key;
                    var file = item.Value;

                    // コミットは辞書単位で行う
                    using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                        database.Open();
                        database.BeginTrans();

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

                        // 対象件数の取得・通知
                        processName = "Count Rows";
                        this._callback.OnPrepared(parser.GetRowCount((long rowCount) => {
                            this._callback.OnPrepared(rowCount);
                        }));

                        // ソースデータを作成
                        processName = "Create Source Data";
                        this.CreateSourceData(id, file, database);

                        // 辞書データを作成
                        processName = "Create Dic Data";
                        while ((data = parser.Read()) != null) {
                            // データ更新
                            this.CreateDicData(id, data, database);

                            // 件数更新
                            this._callback.OnProceed(parser.CurrentLine);
                        }

                        database.CommitTrans();
                        this._callback.OnSuccess();
                    }
                }
            } catch (Exception ex) {
//                Messages.ShowError(Messages.ErrId.Err003, processName, ex.Message);
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
        private void CreateSourceData(long id, string file ,DictionaryDatabase database) {
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
            //var wordsEntity = new WordsEntity(database);
            //wordsEntity.SetDataModel(data);
            //wordsEntity.SourceId = id;
            //var wordId = wordsEntity.Insert();

            //// meadnigs・additions
            //foreach(var meadning in data.Meanings) {
            //    var meaningsEntity = new MeaningsEntity(database);

            //}


        }
        #endregion
    }
}
