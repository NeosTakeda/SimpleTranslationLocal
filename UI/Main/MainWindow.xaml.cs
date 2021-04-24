using OsnCsLib.Common;
using OsnCsLib.WPFComponent;
using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Func.Copy;
using System;
using System.Diagnostics;
using System.Text;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleTranslationLocal.UI.Main {
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : ResidentWindow {

        #region Declaration
        private bool _isActivated = false;
        private SearchResultRenderer _renderer;
        private string _windowTitle;
        private Timer _timer = null;
        private string _searchWord = "";

        private enum CopyMode : short {
            None = 0,
            Once = 1,
            Always = 2,
        }
        private CopyMode _copyMode = CopyMode.None;
        private CopyObserver _copyObserver;
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
                // create SearchResultRenderer after BrowserControl is loaded
                var userMemoryDictionary = AppSettingsRepo.GetInstance().UseMemoryDicitonary;
                if (userMemoryDictionary) {
                    this.IsEnabled = false;
                    this.cBrowser.NavigateToString("<html><body><h4 style='text-align:center;'>now loading...</h4></body></html>");
                }
                this._renderer = new SearchResultRenderer(this.cBrowser, this.CompleteSearch);

                // set title
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                this._windowTitle = $"{versionInfo.ProductName}({versionInfo.FileVersion})";
                this.ShowWindowTitle();

                // set up obserber
                this._copyObserver = new CopyObserver(this, this.ClipboardChanged);

                // set up timer
                this._timer = new Timer(3000);
                this._timer.Elapsed += OnTimedEvent;
            };
            this.Closing += (sender, e) => {
                e.Cancel = true;
                if (this.ShowInTaskbar) {
                    base.SetWindowsState(true);
                }
            };
            this.Activated += (sender, e) => {
                this.cKeyword.Focus();
                this._isActivated = true;
            };
            this.Minimized += this.MainWindowMinimized;

            // set view model
            var model = new MainWindowViewModel();
            this.DataContext = model;
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
        private void ResidentWindow_PreviewKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {

                case Key.Escape:
                    e.Handled = true;
                    base.SetWindowsState(true);
                    break;

                case Key.T:
                    if (Util.IsModifierPressed(ModifierKeys.Shift) && Util.IsModifierPressed(ModifierKeys.Control)) {
                        e.Handled = true;
                        this.Topmost = !this.Topmost;
                        this.ShowWindowTitle();
                    }
                    break;

                case Key.O:
                    if (Util.IsModifierPressed(ModifierKeys.Shift) && Util.IsModifierPressed(ModifierKeys.Control)) {
                        e.Handled = true;
                        if (Util.IsModifierPressed(ModifierKeys.Alt)) {
                            this.ToggleCopyModeAlways();
                        } else {
                            this.ToggleCopyModeOnce();
                        }
                        this.ShowWindowTitle();
                        if (this._copyMode == CopyMode.None) {
                            this.StopClipboardObserve();
                        } else {
                            this.StartClipboardObserve();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Keyword_PreviewKeyDown(object sender, KeyEventArgs e) {
            switch(e.Key) {
                case Key.Enter:
                    e.Handled = true;
                    if (this.IsEnabled) {
                        this.Search();
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Keyword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            if (this.IsEnabled) {
                this._timer.Stop();
                this._timer.Enabled = true;
                this._timer.Start();
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {
            this._timer.Stop();
            this._timer.Enabled = false;
            this.Dispatcher.Invoke((Action)(() => {
                if (0 < this.cKeyword.Text.Length) {
                    this.Search();
                }
            }));
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
            this._copyObserver.Dispose();
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

                this._copyMode = CopyMode.None;
                this._copyObserver.Stop();
                this.ShowWindowTitle();
            }
        }

        /// <summary>
        /// search word
        /// </summary>
        /// <remarks>not mvmo...</remarks>
        private void Search() {
            if (this._searchWord == this.cKeyword.Text) {
                return;
            }
            this._searchWord = this.cKeyword.Text;
            System.Diagnostics.Debug.WriteLine("◆◆◆ STR" + DateTime.Now.ToString("hh:mm:ss fff"));
            this.Cursor = Cursors.Wait;
            this.DoEvents();
            if (0 == this.cKeyword.Text.Length) {
                this.cBrowser.NavigateToString("<html><body></body></html>");
            } else {
                this.IsEnabled = false;
                this.cBrowser.NavigateToString("<html><body><h4 style='text-align:center;'>now searching...</h4></body></html>");
                this.DoEvents();
                this._renderer.Search(this.cKeyword.Text);
            }
        }

        /// <summary>
        /// search complete
        /// </summary>
        private void CompleteSearch() {
            System.Diagnostics.Debug.WriteLine("◆◆◆ END" + DateTime.Now.ToString("hh:mm:ss fff"));
            this.IsEnabled = true;
            this.Cursor = Cursors.None;
            this.cKeyword.Focus();
            this.cKeyword.SelectAll();
        }

        /// <summary>
        /// toggle copy mode
        /// </summary>
        /// <returns>mode is on</returns>
        private void ToggleCopyModeOnce() {
            if (this._copyMode == CopyMode.Once) {
                this._copyMode = CopyMode.None;
            } else {
                this._copyMode = CopyMode.Once;
            }
        }

        /// <summary>
        /// toggle copy mode
        /// </summary>
        /// <returns>mode is on</returns>
        private void ToggleCopyModeAlways() {
            if (this._copyMode == CopyMode.Always) {
                this._copyMode = CopyMode.None;
            } else {
                this._copyMode = CopyMode.Always;
            }
        }

        /// <summary>
        /// clipboard change event
        /// </summary>
        /// <param name="text">copy text</param>
        private void ClipboardChanged(string text) {
            LogUtil.DebugLog("#### ClipboardChanged " + text);
            if (this._copyMode == CopyMode.Once) {
                this._copyMode = CopyMode.None;
                this.StopClipboardObserve();
                this.ShowWindowTitle();
            }
            if (0 < text.Length && this.cKeyword.Text != text) {
                this.Activate();
                this.cKeyword.Text = text.Trim();
                this.cKeyword.Focus();
                this.cKeyword.SelectAll();
                DoEvents();
                this.Search();
            }
        }


        /// <summary>
        /// show window title
        /// </summary>
        private void ShowWindowTitle() {
            var title = new StringBuilder();
            title.Append(this._windowTitle);
            if (this.Topmost) {
                title.Append("[T]");
            }
            switch(this._copyMode) {
                case CopyMode.Once:
                    title.Append("[O]");
                    break;
                case CopyMode.Always:
                    title.Append("[OO]");
                    break;
            }
            this.Title = title.ToString();
        }

        /// <summary>
        /// start observer clipboard
        /// </summary>
        private void StartClipboardObserve() {
            this._copyObserver.Start();
        }

        /// <summary>
        /// stop observe clipboard
        /// </summary>
        private void StopClipboardObserve() {
            this._copyObserver.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoEvents() {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(ExitFrames);
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }
        private object ExitFrames(object obj) {
            ((DispatcherFrame)obj).Continue = false;
            return null;
        }
        #endregion

    }
}
