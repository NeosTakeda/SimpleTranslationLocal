using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;


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
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

//            this._main.Close();
        }
    }
}
