﻿using OsnCsLib.Common;
using OsnCsLib.File;
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
        private string _templateHtml = "";
        #endregion

        #region Constructor
        public MainWindow() {
            InitializeComponent();

            // restore window(load event is not working)
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
            this.Closing += (sender, e) => {
                e.Cancel = true;
                base.SetWindowsState(true);
            };
            this.Activated += (sender, e) => {
                this.cKeyword.Focus();
                this._isActivated = true;
            };
            this.Minimized += this.MainWindowMinimized;

            // load html template
            using(var file = new FileOperator(Constants.HtmlTemplateFile)) {
                this._templateHtml = file.ReadAll();
            }

            // set view model
            var model = new MainWindowViewModel();
            this.DataContext = model;
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
                    this.cBrowser.NavigateToString(this._templateHtml);
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
            base.SetNotifyIconVisible(false);
            new SimpleTranslationLocal.UI.Import.ImportWindow().ShowDialog();
            base.SetNotifyIconVisible(true);
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
        #endregion
    }
}
