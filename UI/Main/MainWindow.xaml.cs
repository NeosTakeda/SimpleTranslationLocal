using OsnCsLib.Common;
using OsnCsLib.WPFComponent;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using System.Windows.Input;

namespace SimpleTranslationLocal.UI.Main {
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : ResidentWindow {

        #region Declaration
        private bool _isActivated = false;
        private SearchResultRenderer _renderer;
        #endregion

        #region Constructor
        public MainWindow() {
            InitializeComponent();

            // restore window(set in SetUp() is not working...)
            var settings = AppSettingsRepo.Init(Constants.SettingsFile);
            Util.SetWindowXPosition(this, settings.X);
            Util.SetWindowYPosition(this, settings.Y);
            this.Width = settings.Width;
            this.Height = settings.Height;
            this.Topmost = settings.Topmost;
        }
        #endregion

        #region Protected Method
        /// <summary>
        /// setup
        /// </summary>
        protected override void SetUp() {
            // setup hotkey and notifiation icon
            base.SetUpHotKey(ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt, Key.L);
            base.SetupNofityIcon("SimpleTranslationLocal", new System.Drawing.Icon("app.ico"));

            // create a context menu
            base.AddContextMenu("Show", (sender, e) => this.OnContextMenuShowClick());
            base.AddContextMenu("Import", (sender, e) => this.OnContextMenuImportClick());
            base.AddContextMenuSeparator();
            base.AddContextMenu("Exit", (sender, e) => this.OnContextMenuExitClick());

            // add event
            this.Loaded += (sender, e) => {
                this._renderer = new SearchResultRenderer(this.cBrowser, this.CompleteSearch);
            };
            this.Closing += (sender, e) => {
                e.Cancel = true;
                base.SetWindowsState(true);
            };
            this.Activated += (sender, e) => {
                this.cKeyword.Focus();
                this._isActivated = true;
            };
            this.Minimized += this.MainWindowMinimized;

            // set view model
            var model = new MainWindowViewModel();
            this.DataContext = model;

            // prepare render
            this._renderer = new SearchResultRenderer(this.cBrowser);
        }

        protected override void OnHotkeyPressed() {
            base.OnHotkeyPressed();
            this.cKeyword.Focus();
            this.cKeyword.SelectAll();
        }
        #endregion

        #region Event
        /// <summary>
        /// key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Keyword_PreviewKeyDown(object sender, KeyEventArgs e) {
            switch(e.Key) {

                case Key.Escape:
                    e.Handled = true;
                    base.SetWindowsState(true);
                    break;

                case Key.Enter:
                    e.Handled = true;
                    this.Search();
                    break;
            }
        }
        #endregion

        #region Protected Method

        #endregion

        #region Private Method
        /// <summary>
        /// context menu show click
        /// </summary>
        private void OnContextMenuShowClick() {
            base.SetWindowsState(false);
        }

        /// <summary>
        /// context menu import click
        /// </summary>
        private void OnContextMenuImportClick() {
            base.NotifyIconVisible = false;
            base.IgnoreHotKey = true;
            new Import.ImportWindow().ShowDialog();
            base.NotifyIconVisible = true;
            base.IgnoreHotKey = false;
        }

        /// <summary>
        /// context menu exit click
        /// </summary>
        private void OnContextMenuExitClick() {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Window Closed event
        /// </summary>
        private void MainWindowMinimized() {
            if (this._isActivated) {
                var setting = AppSettingsRepo.GetInstance();
                setting.X = this.Left;
                setting.Y = this.Top;
                setting.Height = this.Height;
                setting.Width = this.Width;
                setting.Save();
            }
        }

        /// <summary>
        /// search word
        /// </summary>
        /// <remarks>not mvmo...</remarks>
        private void Search() {
            if (0 == this.cKeyword.Text.Length) {
                this.cBrowser.NavigateToString("<html><body></body></html>");
            } else {
                this.IsEnabled = false;
                this._renderer.Search(this.cKeyword.Text);
            }
        }

        /// <summary>
        /// search complete
        /// </summary>
        private void CompleteSearch() {
            this.IsEnabled = true;
        }
        #endregion
    }
}
