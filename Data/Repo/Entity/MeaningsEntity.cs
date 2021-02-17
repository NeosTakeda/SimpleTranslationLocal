using OsnLib.Data.Sqlite;
using System;

namespace SimpleTranslationLocal.Data.Repo.Entity {

    /// <summary>
    /// meanings entity
    /// </summary>
    internal class MeaningsEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        internal static class Cols {
            public static readonly String Id = "id";
            public static readonly String WordId = "word_id";
            public static readonly String Meaning = "meaning";
            public static readonly String PartOfSpeach = "part_of_speach";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        internal static readonly string TableName = "meanings";
        /// <summary>
        /// id
        /// </summary>
        internal long Id { set; get; }

        /// <summary>
        /// wor id
        /// </summary>
        internal long WordId { set; get; }

        /// <summary>
        /// word meaning
        /// </summary>
        internal string Meaning { set; get; }

        /// <summary>
        /// part of speach
        /// </summary>
        internal string PartOfSpeach { set; get; }
        #endregion

        #region Constructor
        internal MeaningsEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        internal override void DeleteBySourceId(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql($"DELETE FROM {TableName}")
                .AppendSql($" WHERE {Cols.Id} IN (")
                .AppendSql($"    SELECT t1.{Cols.Id}")
                .AppendSql($"      FROM {TableName} t1")
                .AppendSql($"     INNER JOIN {WordsEntity.TableName} t2 ON ")
                .AppendSql($"           t1.{Cols.WordId} = t2.{WordsEntity.Cols.Id}")
                .AppendSql($"     WHERE t2.{WordsEntity.Cols.SourceId} = {id}")
                .AppendSql($")");
            base.Database.ExecuteNonQuery(sql);
        }

        internal override bool Create() {
            var result = false;
            var sql = new SqlBuilder();
            sql.AppendSql($"CREATE TABLE {TableName} (")
                .AppendSql($" {Cols.Id}             INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql($",{Cols.WordId}         INTEGER NOT NULL")
                .AppendSql($",{Cols.Meaning}        TEXT    NOT NULL")
                .AppendSql($",{Cols.PartOfSpeach}   TEXT")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER")
                .Append(")");
            result =  0 <= base.Database.ExecuteNonQuery(sql);

            if (result) {
                sql.Clear();
                sql.AppendSql("CREATE INDEX meanings_idx1 ON meanings (");
                sql.AppendSql("    word_id");
                sql.AppendSql(")");
                result = 0 <= base.Database.ExecuteNonQuery(sql);
            }
            return result;
        }

        internal override long Insert() {
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
