using OsnLib.Data.Sqlite;
using System;

namespace SimpleTranslationLocal.Data.Entity {
    class MeaningsEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        private class Cols {
            public static readonly String Id = "id";
            public static readonly String WordId = "word_id";
            public static readonly String Meaning = "meaning";
            public static readonly String PartOfSpeach = "part_of_speach";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        public override string TableName => "meanings";
        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 用語ID
        /// </summary>
        public int WordId { set; get; }

        /// <summary>
        /// 意味
        /// </summary>
        public string Meaning { set; get; }

        /// <summary>
        /// 品詞
        /// </summary>
        public string PartOfSpeach { set; get; }
        #endregion

        #region Constructor
        public MeaningsEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        public override bool Create() {
            var sql = new SqlBuilder();
            sql.AppendSql($"CREATE TABLE {TableName} (")
                .AppendSql($" {Cols.Id}             INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql($",{Cols.WordId}         INTEGER NOT NULL")
                .AppendSql($",{Cols.Meaning}        TEXT    NOT NULL")
                .AppendSql($",{Cols.PartOfSpeach}   TEXT")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER")
                .Append(")");
            return 0 < base.Database.ExecuteNonQuery(sql);
        }

        public override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql($"INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.WordId}")
                .AppendSql($",{Cols.Meaning}")
                .AppendSql($",{Cols.PartOfSpeach}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.WordId}")
                .AppendSql($",@{Cols.Meaning}")
                .AppendSql($",@{Cols.PartOfSpeach}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.WordId}", this.WordId);
            paramList.Add($"@{Cols.Meaning}", this.Meaning);
            paramList.Add($"@{Cols.PartOfSpeach}", this.PartOfSpeach);
            return base.Database.Insert(sql, paramList);
        }
        #endregion
    }
}
