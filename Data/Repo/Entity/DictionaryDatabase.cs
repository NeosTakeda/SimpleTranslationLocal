﻿using OsnLib.Data.Sqlite;
using System.Collections.Generic;

namespace SimpleTranslationLocal.Data.Repo.Entity {
    internal class DictionaryDatabase : Database {

        #region Declaration
        private enum Ver : int {
            Ver00 = 0,
            Current = Ver00
        }
        private delegate List<SqlBuilder> CreateSqls();
        #endregion

        #region Publi Property
        internal static string Password { private set; get; } = "";
        #endregion

        #region Constructor
        internal DictionaryDatabase(string database) : base(database, (int)Ver.Current) {
        }
        #endregion

        #region Public Method
        public override void Open() {
            base.Open(Password);
        }
        #endregion
    }
}