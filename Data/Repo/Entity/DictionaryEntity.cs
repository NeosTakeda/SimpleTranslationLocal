using OsnLib.Data.Sqlite;
using System;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo.Entity {

    /// <summary>
    /// dictionary entity
    /// </summary>
    internal class DictionaryEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        internal static class Cols {
            public static readonly String Id = "id";
            public static readonly String SourceId = "source_id";
            public static readonly String Word = "word";
            public static readonly String WordSort = "word_sort";
            public static readonly String Data = "data";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        /// <summary>
        /// table name
        /// </summary>
        internal static readonly string TableName = "dictionary";

        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// source id
        /// </summary>
        public int SourceId { set; get; }

        /// <summary>
        /// word
        /// </summary>
        internal string Word { set; get; } = "";

        /// <summary>
        /// word for sort
        /// </summary>
        internal string WordSort { set; get; } = "";

        /// <summary>
        /// html formated data
        /// </summary>
        internal string Data { set; get; } = "";
        #endregion

        #region Constructor
        internal DictionaryEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        internal override void DeleteBySourceId(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql($"DELETE FROM {TableName}")
                .AppendSql($"WHERE {Cols.SourceId} = {id}");
            base.Database.ExecuteNonQuery(sql);
        }

        internal override bool Create() {
            var sql = new SqlBuilder();
            sql.AppendSql($"CREATE TABLE {TableName} (")
                .AppendSql($" {Cols.Id}             INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql($",{Cols.SourceId}       INTEGER NOT NULL")
                .AppendSql($",{Cols.Word}           TEXT    NOT NULL")
                .AppendSql($",{Cols.WordSort}       TEXT    NOT NULL")
                .AppendSql($",{Cols.Data}           TEXT    NOT NULL")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER")
                .Append(")");
            bool result = 0 <= base.Database.ExecuteNonQuery(sql);

            if (result) {
                sql.Clear();
                sql.AppendSql($"CREATE INDEX dictionary_idx1 ON {TableName} (");
                sql.AppendSql($"    {Cols.WordSort}");
                sql.AppendSql(")");
                result = 0 <= base.Database.ExecuteNonQuery(sql);
            }
            return result;
        }

        internal override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql($"INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.SourceId}")
                .AppendSql($",{Cols.Word}")
                .AppendSql($",{Cols.WordSort}")
                .AppendSql($",{Cols.Data}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.SourceId}")
                .AppendSql($",@{Cols.Word}")
                .AppendSql($",@{Cols.WordSort}")
                .AppendSql($",@{Cols.Data}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.SourceId}", this.SourceId);
            paramList.Add($"@{Cols.Word}", this.Word);
            paramList.Add($"@{Cols.WordSort}", this.Word.ToLower());
            paramList.Add($"@{Cols.Data}", this.Data);
            return base.Database.Insert(sql, paramList);
        }

        /// <summary>
        /// search dictionary
        /// </summary>
        /// <param name="word">word</param>
        /// <param name="matchType">search matching type</param>
        /// <returns>record set</returns>
        internal Recordset Search(string word, MatchType matchType) {
            var sql = new SqlBuilder();
            sql.AppendSql($"SELECT")
                .AppendSql($" {Cols.Word}")
                .AppendSql($",{Cols.SourceId}")
                .AppendSql($",{Cols.Data}")
                .AppendSql("FROM")
                .AppendSql($" {TableName} ")
                .AppendSql($"WHERE ");
            switch (matchType) {
                case MatchType.Prefix:
                case MatchType.Broad:
                    sql.AppendSql($"{Cols.WordSort} LIKE @{Cols.WordSort}");
                    break;
                default:
                    sql.AppendSql($"{Cols.WordSort} = @{Cols.WordSort}");
                    break;
            }
            sql.AppendSql($"ORDER BY ")
                .AppendSql($" {Cols.SourceId}")
                .AppendSql($",{Cols.WordSort} ")
                .AppendSql($",{Cols.Id}");
            var paramList = new ParameterList();

            switch (matchType) {
                case MatchType.Prefix:
                    paramList.Add($"@{Cols.WordSort}", $"{word}%");
                    break;
                case MatchType.Broad:
                    paramList.Add($"@{Cols.WordSort}", $"%{word}%");
                    break;
                default:
                    paramList.Add($"@{Cols.WordSort}", word);
                    break;
            }

            return base.Database.OpenRecordset(sql, paramList);
        }
        #endregion
    }
}
