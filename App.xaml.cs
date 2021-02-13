using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.Data.Repo.Entity;
using System;
using System.Windows;

namespace SimpleTranslationLocal {

    /// <summary>
    /// app entry point
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            // create a database file if need
            if (!System.IO.File.Exists(Constants.DatabaseFile)) {
                using (var database = new DictionaryDatabase(Constants.DatabaseFile)) {
                    try {
                        database.Open();
                        database.BeginTrans();
  
                        new SourcesRepo(database).Create();
                        new WordsRepo(database).Create();
                        new MeaningsRepo(database).Create();
                        new AdditionsRepo(database).Create();

                        database.CommitTrans();
                    } catch (Exception ex) {
                        Messages.ShowError(Messages.ErrId.Err002, ex.Message);
                    }
                }
            }
        }
    }
}
