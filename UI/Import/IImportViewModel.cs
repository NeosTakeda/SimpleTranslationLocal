using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTranslationLocal.UI.Import.Command;
using Microsoft.Win32;

namespace SimpleTranslationLocal.UI.Import {
    abstract class IImportViewModel : BindableBase {
        // https://trapemiya.hatenablog.com/entry/20100930/1285826338

        #region 
        private Window _owner;
        #endregion

        #region Property
        /// <summary>
        /// 取込む英辞郎のファイル
        /// </summary>
        public virtual string EijiroFile {
            set;
            get;
        }

        /// <summary>
        /// 取込むDictionaryのファイル
        /// </summary>
        public virtual string DictionaryFile {
            set;
            get;
        }

        /// <summary>
        /// OKボタンクリック コマンド
        /// </summary>
        public OKCommand OKClick { private set; get; }

        /// <summary>
        /// 英辞郎ファイル選択クリック コマンド
        /// </summary>
        public SelectEijiroCommand SelectEijiroClick { private set; get; }

        /// <summary>
        /// 英辞郎ファイルインポートクリック コマンド
        /// </summary>
        public ImportEijiroCommand ImportEijiroClick { private set; get; }

        /// <summary>
        /// Dictionaryファイル選択クリック コマンド
        /// </summary>
        public SelectDictionaryCommand SelectDictionaryClick { private set; get; }

        /// <summary>
        /// Dictionaryファイルインポートクリック コマンド
        /// </summary>
        public ImportDictionaryCommand ImportDictionaryClick { private set; get; }
        #endregion

        #region Constructor
        public IImportViewModel(Window owner, Action OnOkClickCallback) {
            this._owner = owner;
            this.SetupCommand(OnOkClickCallback);
        }
        #endregion

        #region Action
        ///// <summary>
        ///// 英辞郎ファイル選択ボタンクリック時
        ///// </summary>
        //public abstract void SelectEijiroFileClickAction();

        ///// <summary>
        ///// 英辞郎ファイルインポートボタンクリック時
        ///// </summary>
        //public abstract void ImportEijiroFileClickAction();

        ///// <summary>
        ///// Dictionaryファイル選択ボタンクリック時
        ///// </summary>
        //public abstract void SelectDictionaryFileClickAction();

        ///// <summary>
        ///// Dictionaryファイルインポートボタンクリック時
        ///// </summary>
        //public abstract void ImportDictionaryFileClickAction();

        ///// <summary>
        ///// OKボタンクリック時
        ///// </summary>
        //public abstract void OKClickAction();
        #endregion

        #region Private Method
        /// <summary>
        /// コマンドをセットアップする
        /// </summary>
        /// <param name="OnOkClickCallback"></param>
        private void SetupCommand(Action OnOkClickCallback) {
            this.OKClick = new OKCommand(OnOkClickCallback);
            this.SelectEijiroClick = new SelectEijiroCommand(SelectEijiroFile);
            this.ImportEijiroClick = new ImportEijiroCommand(null);
            this.SelectDictionaryClick = new SelectDictionaryCommand(SelectDictionaryFile);
            this.ImportDictionaryClick = new ImportDictionaryCommand(null);
        }

        /// <summary>
        /// 英辞郎ファイル選択処理
        /// </summary>
        private void SelectEijiroFile() {
            var file = this.SelectFile(this.EijiroFile, "英辞郎(*.txt)|*.txt");
            if (0 < file.Length) {
                this.EijiroFile = file;
            }
        }

        /// <summary>
        /// Dictionaryファイル選択処理
        /// </summary>
        private void SelectDictionaryFile() {
            var file = this.SelectFile(this.DictionaryFile, "Dictionary(*.json)|*.json");
            if (0 < file.Length) {
                this.EijiroFile = file;
            }
        }

        /// <summary>
        /// ファイルを開くダイアログを表示
        /// </summary>
        /// <param name="initialFile">既定のファイル</param>
        /// <param name="filter">フィルター</param>
        /// <returns></returns>
        private string SelectFile(string initialFile, string filter) {
            var result = "";
            var dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.FilterIndex = 0;
            dialog.FileName = initialFile;
            dialog.Title = "辞書ファイルを選択";
            dialog.CheckFileExists = true;
            if (true == dialog.ShowDialog(this._owner)) {
                result = dialog.FileName;
            }
            return result;
        }
        #endregion
    }
}
