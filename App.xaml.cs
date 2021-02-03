using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Entity;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SimpleTranslationLocal {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {

        #region Declaration
//        private Window _main = new SimpleTranslationLocal.UI.Main.MainWindow();
        #endregion

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            /*
            var notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("./app.ico");
            notifyIcon.Visible = true;

            Assembly asm = Assembly.GetExecutingAssembly();
            object[] titleArray = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            notifyIcon.Text = ((AssemblyTitleAttribute)titleArray[0]).Title;


            NotifyIcon icon = new NotifyIcon();

            icon.Icon = new System.Drawing.Icon("./test.ico");
            icon.Visible = true;
            icon.Text = "My Icon";

            //Menuのインスタンス化
            var menu = new ContextMenuStrip();

            //MenuItemの作成
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "&Exit";
            menuItem.Click += (s, e) =>
            {
                System.Windows.Application.Current.Shutdown();
            };

            //MenuにMenuItemを追加
            menu.Items.Add(menuItem);

            //Menuをタスクトレイのアイコンに追加
            icon.ContextMenuStrip = menu;
             */


            // create a database file if need
            if (!System.IO.File.Exists(Constants.DatabaseFile)) {
                using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                    try {
                        database.Open();
                        database.BeginTrans();
                        var tables = new List<BaseEntity>  { new AdditionsEntity(database)
                                                             ,new SourcesEntity(database)
                                                             ,new MeaningsEntity(database)
                                                             ,new WordsEntity(database)};
                        foreach(var table in tables) {
                            if (!table.Create()) {
                                // Messages.ShowError(Messages.ErrId)
                            }
                        }

                        database.CommitTrans();
                    } catch (Exception ex) {
                        Messages.ShowError(Messages.ErrId.Err002, ex.Message);
                    }
                }


                //using (var database = new ProfileDatabase(profile.FilePath)) {
                //    try {
                //        database.SetPassWord(ProfileDatabase.Password);
                //        database.Open();
                //        database.BeginTrans();
                //        if (database.ExecuteNonQuery(CategoriesTable.CreateTable()) < 0) {
                //            AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToCreate, "categories table"));
                //            return;
                //        }
                //        if (database.ExecuteNonQuery(ItemsTable.CreateTable()) < 0) {
                //            AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToCreate, "items table"));
                //            return;
                //        }
                //        database.CommitTrans();
                //    } catch (Exception ex) {
                //        AppCommon.ShowErrorMsg(ex.Message);
                //    } finally {
                //        database.RollbackTrans();
                //    }
                //}
            }
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

//            this._main.Close();
        }
    }
}
