using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using System;
using System.Windows;
using System.IO;

namespace SimpleTranslationLocal {

    /// <summary>
    /// app entry point
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            // create data directory
            var dirs = new string[]{ Constants.DataFolder, Constants.EijiroData, Constants.WebsterData};
            foreach(var dir in dirs) {
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
            }
        }
    }
}
