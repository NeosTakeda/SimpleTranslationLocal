using OsnLib.Data.Sqlite;
using System;

namespace SimpleTranslationLocal.Data.Entity {
    class AdditionsEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        private class Cols {
            public static readonly String Id = "id";
            public static readonly String MeaningId = "meaning_id";
            public static readonly String Type = "type";
            public static readonly String Data = "data";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        public override string TableName => "additions";

        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// 意味ID
        /// </summary>
        public int MeaningId { set; get; }

        /// <summary>
        /// 種別
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        /// データ
        /// </summary>
        public string Data { set; get; }
        #endregion

        #region Constructor
        public AdditionsEntity(DictionaryDatabase database) : base(database) { }
        #endregion

        #region Public Method
        public override bool Create() {
            var sql = new SqlBuilder();
            sql.AppendSql($"CREATE TABLE {TableName} (")
                .AppendSql($" {Cols.Id}             INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql($",{Cols.MeaningId}      INTEGER NOT NULL")
                .AppendSql($",{Cols.Type}           TEXT    NOT NULL")
                .AppendSql($",{Cols.Data}           TEXT")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER");
            return 0 < base.Database.ExecuteNonQuery(sql);
        }

        public override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql($"INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.MeaningId}")
                .AppendSql($",{Cols.Type}")
                .AppendSql($",{Cols.Data}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.MeaningId}")
                .AppendSql($",@{Cols.Type}")
                .AppendSql($",@{Cols.Data}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.MeaningId}", this.MeaningId);
            paramList.Add($"@{Cols.Type}", this.Type);
            paramList.Add($"@{Cols.Data}", this.Data);
            return base.Database.Insert(sql, paramList);
        }
        #endregion

    }
}
