using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsnCsLib.Data;

namespace SimpleTranslationLocal.Data.Repo {
    public class AppSettingsRepo : AppDataBase<AppSettingsRepo> {

        #region Declaration
        private static string _file;
        #endregion

        #region Public Property
        /// <summary>
        /// ウィンドウのX座標
        /// </summary>
        public double X { set; get; }

        /// <summary>
        /// ウィンドウのY座標
        /// </summary>
        public double Y { set; get; }

        /// <summary>
        /// ウィンドウの幅
        /// </summary>
        public double Width { set; get; }

        /// <summary>
        /// ウィンドウの高さ
        /// </summary>
        public double Height { set; get; }

        /// <summary>
        /// ウィンドウの最前面表示
        /// </summary>
        public bool Topmost { set; get; }

        /// <summary>
        /// 取り込んだ英辞郎のファイル
        /// </summary>
        public string EijiroFile { set; get; }

        /// <summary>
        /// 取り込んだDictionaryのファイル
        /// </summary>
        public string DictionaryFile { set; get; }
        #endregion

        #region Public Method
        /// <summary>
        /// 初期化処理を行う
        /// </summary>
        /// <param name="file">ファイル名(フルパス)</param>
        /// <returns>レポジトリのインスタンス</returns>
        public static AppSettingsRepo Init(string file) {
            _file = file;
            GetInstanceBase(file);
            if (!System.IO.File.Exists(file)) {
                _instance.Save();
            }
            return _instance;
        }

        /// <summary>
        /// レポジトリのインスタンスを取得
        /// </summary>
        /// <returns>レポジトリのインスタンス</returns>
        public static AppSettingsRepo GetInstance() {
            return GetInstanceBase();
        }

        /// <summary>
        /// 設定情報を保存する
        /// </summary>
        public void Save() {
            GetInstanceBase().SaveToXml(_file);
        }
        #endregion
    }
}
