using OsnLib.Data.Sqlite;
using System;


namespace SimpleTranslationLocal.Data.Entity {
    class SourcesEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        private class Cols {
            public static readonly String Id = "id";
            public static readonly String Name = "name";
            public static readonly String Priority = "priority";
            public static readonly String File = "file";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        public override string TableName => "sources";
        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 優先順位
        /// </summary>
        public string Priority { set; get; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string File { set; get; }
        #endregion

        #region Constructor
        public SourcesEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        public override bool Create() {
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

        public override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql($"INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.Name}")
                .AppendSql($",{Cols.Priority}")
                .AppendSql($",{Cols.File}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.Name}")
                .AppendSql($",@{Cols.Priority}")
                .AppendSql($",@{Cols.File}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.Name}", this.Name);
            paramList.Add($"@{Cols.Priority}", this.Priority);
            paramList.Add($"@{Cols.File}", this.File);
            return base.Database.Insert(sql, paramList);
        }
        #endregion

    }
}
