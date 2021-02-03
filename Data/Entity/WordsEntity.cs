using OsnLib.Data.Sqlite;
using System;

namespace SimpleTranslationLocal.Data.Entity {
    class WordsEntity : BaseEntity{
        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        private class Cols {
            public static readonly String Id = "id";
            public static readonly String SourceId = "source_id";
            public static readonly String Word = "word";
            public static readonly String Pronunciation = "pronunciation";
            public static readonly String Syllable = "syllable";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        public override string TableName => "words";
        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// ソースID
        /// </summary>
        public int SourceId { set; get; }

        /// <summary>
        /// 用語
        /// </summary>
        public string Word { set; get; }

        /// <summary>
        /// 発音
        /// </summary>
        public string Pronunciation { set; get; }

        /// <summary>
        /// 音節
        /// </summary>
        public string Syllable { set; get; }
        #endregion

        #region Constructor
        public WordsEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        public override bool Create() {
            var sql = new SqlBuilder();
            sql.AppendSql($"CREATE TABLE {TableName} (")
                .AppendSql($" {Cols.Id}             INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql($",{Cols.SourceId}       INTEGER NOT NULL")
                .AppendSql($",{Cols.Word}           TEXT    NOT NULL")
                .AppendSql($",{Cols.Pronunciation}  TEXT")
                .AppendSql($",{Cols.Syllable}       TEXT")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER")
                .Append(")");
            return 0 < base.Database.ExecuteNonQuery(sql);
        }

        public override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql("INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.SourceId}")
                .AppendSql($",{Cols.Word}")
                .AppendSql($",{Cols.Pronunciation}")
                .AppendSql($",{Cols.Syllable}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.SourceId}")
                .AppendSql($",@{Cols.Word}")
                .AppendSql($",@{Cols.Pronunciation}")
                .AppendSql($",@{Cols.Syllable}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.SourceId}", this.SourceId);
            paramList.Add($"@{Cols.Word}", this.Word);
            paramList.Add($"@{Cols.Pronunciation}", this.Pronunciation);
            paramList.Add($"@{Cols.Syllable}", this.Syllable);
            return base.Database.Insert(sql, paramList);
        }
        #endregion
    }
}
