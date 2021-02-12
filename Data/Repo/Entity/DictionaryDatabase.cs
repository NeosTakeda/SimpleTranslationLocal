using OsnLib.Data.Sqlite;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.Repo.Entity {
    class DictionaryDatabase : Database {

        #region Declaration
        private enum Ver : int {
            Ver00 = 0,
            Current = Ver00
        }
        private delegate List<SqlBuilder> CreateSqls();
        #endregion

        #region Publi Property
        public static string Password { private set; get; } = "";
        #endregion

        #region Constructor
        public DictionaryDatabase(string database) : base(database, (int)Ver.Current) {
        }
        #endregion

        #region Public Method
        public override void Open() {
            base.Open(Password);
        }
        #endregion
    }
}
