using Microsoft.Win32;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Func.Import;
using SimpleTranslationLocal.UI.Import.Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleTranslationLocal.UI.Import {
    abstract class IImportViewModel : BindableBase, ImportServiceCallback {
        // https://trapemiya.hatenablog.com/entry/20100930/1285826338

        #region 
        private readonly Window _owner;
        Constants.DicType _dicType;
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
        public virtual string WebsterFile {
            set;
            get;
        }

        /// <summary>
        /// 処理中パネルの可視
        /// </summary>
        private Visibility _progressPanelVisibility = Visibility.Collapsed;
        public Visibility ProgressPanelVisibility {
            set { base.SetProperty(ref this._progressPanelVisibility, value);  }
            get { return this._progressPanelVisibility; }
        }

        /// <summary>
        /// 現在の取込件数
        /// </summary>
        private long _currentCount = 0;
        public long CurrentCount {
            set { base.SetProperty(ref this._currentCount, value); }
            get { return this._currentCount; }
        }

        /// <summary>
        /// トータル件数
        /// </summary>
        private long _totalCount = 0;
        public long TotalCount {
            set { base.SetProperty(ref this._totalCount, value); }
            get { return this._totalCount; }
        }

        /// <summary>
        /// 英辞郎ファイルインポートボタンの使用可否
        /// </summary>
        public bool ImportEijiroEnabled {
            get { return 0 < EijiroFile?.Length;  }
        }

        /// <summary>
        /// Websterファイルインポートボタンの使用可否
        /// </summary>
        public bool ImportWebsterEnabled {
            get { return 0 < WebsterFile?.Length; }
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
        public SelectWebsterCommand SelectWebsterClick { private set; get; }

        /// <summary>
        /// Dictionaryファイルインポートクリック コマンド
        /// </summary>
        public ImportWebsterCommand ImportWebsterClick { private set; get; }
        #endregion

        #region Constructor
        public IImportViewModel(Window owner, Action OnOkClickCallback) {
            this._owner = owner;
            this.SetupCommand(OnOkClickCallback);
        }
        #endregion

        #region ImportServiceCallback
        void ImportServiceCallback.OnPrepared(long totalCount) {
            this.TotalCount = totalCount;
        }

        void ImportServiceCallback.OnProceed(long count) {
            this.CurrentCount = count;
        }

        void ImportServiceCallback.OnSuccess() {
            var settings = AppSettingsRepo.GetInstance();
            switch(this._dicType) {
                case Constants.DicType.Eijiro:
                    settings.EijiroFile = EijiroFile;
                    break;
                case Constants.DicType.Webster:
                    settings.WebsterFile = WebsterFile;
                    break;
            }
            settings.Save();
            this._owner.Dispatcher.Invoke((Action)(() => {
                Messages.ShowInfo(this._owner, Messages.InfoId.Info001);
            }));
        }

        void ImportServiceCallback.OnFail(string errorMessage) {
            this._owner.Dispatcher.Invoke((Action)(() => {
                Messages.ShowError(this._owner, errorMessage);
            }));
        }
        #endregion

        #region Protected Method
        #endregion

        #region Private Method
        /// <summary>
        /// コマンドをセットアップする
        /// </summary>
        /// <param name="OnOkClickCallback"></param>
        private void SetupCommand(Action OnOkClickCallback) {
            this.OKClick = new OKCommand(OnOkClickCallback);
            this.SelectEijiroClick = new SelectEijiroCommand(SelectEijiro);
            this.ImportEijiroClick = new ImportEijiroCommand(ImportEijiro);
            this.SelectWebsterClick = new SelectWebsterCommand(SelectWebster);
            this.ImportWebsterClick = new ImportWebsterCommand(ImportWebster);
        }

        /// <summary>
        /// 英辞郎ファイル選択処理
        /// </summary>
        private void SelectEijiro() {
            var file = this.SelectFile(this.EijiroFile, "英辞郎(*.txt)|*.txt");
            if (0 < file.Length) {
                this.EijiroFile = file;
            }
        }

        /// <summary>
        /// Websterファイル選択処理
        /// </summary>
        private void SelectWebster() {
            var file = this.SelectFile(this.WebsterFile, "Dictionary(*.json)|*.json");
            if (0 < file.Length) {
                this.WebsterFile = file;
            }
        }
        
        /// <summary>
        /// 英辞郎のインポート処理
        /// </summary>
        private void ImportEijiro() {
            this.ImportData(Constants.DicType.Eijiro);
        }

        /// <summary>
        /// Websterのインポート処理
        /// </summary>
        private void ImportWebster() {
            this.ImportData(Constants.DicType.Webster);
        }

        /// <summary>
        /// ファイルを開くダイアログを表示
        /// </summary>
        /// <param name="initialFile">既定のファイル</param>
        /// <param name="filter">フィルター</param>
        /// <returns></returns>
        private string SelectFile(string initialFile, string filter) {
            var result = "";
            var dialog = new OpenFileDialog {
                Filter = filter,
                FilterIndex = 0,
                FileName = initialFile,
                Title = "辞書ファイルを選択",
                CheckFileExists = true
            };
            if (true == dialog.ShowDialog(this._owner)) {
                result = dialog.FileName;
            }
            return result;
        }

        /// <summary>
        /// 辞書をインポート
        /// </summary>
        /// <param name="dicType">辞書種別</param>
        private async void ImportData(Constants.DicType dicType) {
            this._dicType = dicType;
            this.CurrentCount = 0;
            this.TotalCount = 0;
            this.ProgressPanelVisibility = Visibility.Visible;
            this._owner.IsEnabled = false;

            await Task.Run(() => {
                IImportService service;
                if (Env.Current == Env.EnvType.Stub) {
                    service = new ImportMockService(this);
                } else {
                    service = new ImportService(this);
                }
                service.Start(new Dictionary<Constants.DicType, string>(){ 
                    { dicType, dicType == Constants.DicType.Eijiro ? this.EijiroFile : this.WebsterFile } });

                this.ProgressPanelVisibility = Visibility.Collapsed;
                this._owner.Dispatcher.Invoke((Action)(() => {
                    this._owner.IsEnabled = true;
                }));
            });
        }
        #endregion
    }
}
