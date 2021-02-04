using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try {
                IDictionaryParser parser;
                foreach (var item in targetList) {
                    // コミットは辞書単位で行う
                    using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                        try {
                            database.Open();
                            database.BeginTrans();

                            // 既存データを削除
                            DeleteBySourceId((int)item.Key, database);

                            switch(item.Key) {
                                case DicType.Eijiro:
                                    parser = new EijiroParser(item.Value);
                                    break;
                                case DicType.Webster:
                                    parser = new WebsterParser(item.Value);
                                    break;
                                default:
                                    throw new Exception("unknown key type : " + item.Key);
                            }

                            // 対象件数の取得・通知

                            // データの読込

                            // データ更新

                            // 件数更新


                            database.CommitTrans();
                        } catch (Exception ex) {
                            Messages.ShowError(Messages.ErrId.Err002, ex.Message);
                        }
                    }
                }
                this._callback.OnSuccess();
            } catch (Exception ex) {
                this._callback.OnFail(ex.Message);
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// ソースIDをキーとして全テーブルを削除する
        /// </summary>
        /// <param name="id">ソースID</param>
        /// <param name="database">データベースのインスタンス</param>
        private void DeleteBySourceId(int id, DictionaryDatabase database) {

        }
        #endregion
    }
}
