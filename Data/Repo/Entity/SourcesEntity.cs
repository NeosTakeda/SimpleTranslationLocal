using OsnLib.Data.Sqlite;
using System;

namespace SimpleTranslationLocal.Data.Repo.Entity {
        
    /// <summary>
    /// sources entity
    /// </summary>
    internal class SourcesEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        internal static class Cols {
            public static readonly String Id = "id";
            public static readonly String Name = "name";
            public static readonly String Priority = "priority";
            public static readonly String File = "file";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        internal static readonly string TableName = "sources";
        /// <summary>
        /// id
        /// </summary>
        internal long Id { set; get; }

        /// <summary>
        /// source name
        /// </summary>
        internal string Name { set; get; }

        /// <summary>
        /// source priority
        /// </summary>
        internal int Priority { set; get; }

        /// <summary>
        /// source file
        /// </summary>
        internal string File { set; get; }
        #endregion

        #region Constructor
        internal SourcesEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        internal override void DeleteBySourceId(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql($"DELETE FROM {TableName} ")
                .AppendSql($"WHERE {Cols.Id} = {id}");
            base.Database.ExecuteNonQuery(sql);
        }

        internal override bool Create() {
            var sql = new SqlBuilder();
            sql.AppendSql($"CREATE TABLE {TableName} (")
                .AppendSql($" {Cols.Id}             INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql($",{Cols.Name}           TEXT     NOT NULL")
                .AppendSql($",{Cols.Priority}       INTEGER  NOT NULL")
                .AppendSql($",{Cols.File}           TEXT     NOT NULL")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER")
                .Append(")");
            return 0 < base.Database.ExecuteNonQuery(sql);
        }

        internal override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql($"INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.Id}")
                .AppendSql($",{Cols.Name}")
                .AppendSql($",{Cols.Priority}")
                .AppendSql($",{Cols.File}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.Id}")
                .AppendSql($",@{Cols.Name}")
                .AppendSql($",@{Cols.Priority}")
                .AppendSql($",@{Cols.File}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.Id}", this.Id);
            paramList.Add($"@{Cols.Name}", this.Name);
            paramList.Add($"@{Cols.Priority}", this.Priority);
            paramList.Add($"@{Cols.File}", this.File);
            return base.Database.Insert(sql, paramList);
        }
        #endregion

    }
}
