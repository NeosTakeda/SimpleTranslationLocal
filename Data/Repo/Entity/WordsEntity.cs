using OsnLib.Data.Sqlite;
using System;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo.Entity {

    /// <summary>
    /// words entity
    /// </summary>
    internal class WordsEntity : BaseEntity {

        #region Declaration
        /// <summary>
        /// Column Names
        /// </summary>
        internal static class Cols {
            public static readonly String Id = "id";
            public static readonly String SourceId = "source_id";
            public static readonly String Word = "word";
            public static readonly String Pronunciation = "pronunciation";
            public static readonly String Syllable = "syllable";
            public static readonly String Kana = "kana";
            public static readonly String Level = "level";
            public static readonly String Change = "change";
            public static readonly String CreateAt = "create_at";
            public static readonly String UpdateAt = "update_at";
        }
        #endregion

        #region Public Property
        internal static readonly string TableName = "words";
        /// <summary>
        /// id
        /// </summary>
        internal int Id { set; get; }

        /// <summary>
        /// source id
        /// </summary>
        internal int SourceId { set; get; }

        /// <summary>
        /// word
        /// </summary>
        internal string Word { set; get; }

        /// <summary>
        /// pronunciation
        /// </summary>
        internal string Pronunciation { set; get; }

        /// <summary>
        /// syllable
        /// </summary>
        internal string Syllable { set; get; }

        /// <summary>
        /// kana of syllable
        /// </summary>
        internal string Kana { set; get; }

        /// <summary>
        /// level
        /// </summary>
        internal int Level { set; get; }

        /// <summary>
        /// change
        /// </summary>
        internal string Change { set; get; }
        #endregion

        #region Constructor
        internal WordsEntity(DictionaryDatabase database) : base(database) { }
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
                .AppendSql($",{Cols.Pronunciation}  TEXT")
                .AppendSql($",{Cols.Syllable}       TEXT")
                .AppendSql($",{Cols.Kana}           TEXT")
                .AppendSql($",{Cols.Level}          INTEGER")
                .AppendSql($",{Cols.Change}           TEXT")
                .AppendSql($",{Cols.CreateAt}       INTEGER")
                .AppendSql($",{Cols.UpdateAt}       INTEGER")
                .Append(")");
            return 0 < base.Database.ExecuteNonQuery(sql);
        }

        internal override long Insert() {
            var sql = new SqlBuilder();
            sql.AppendSql($"INSERT INTO {TableName}")
                .AppendSql("(")
                .AppendSql($" {Cols.SourceId}")
                .AppendSql($",{Cols.Word}")
                .AppendSql($",{Cols.Pronunciation}")
                .AppendSql($",{Cols.Syllable}")
                .AppendSql($",{Cols.Kana}")
                .AppendSql($",{Cols.Level}")
                .AppendSql($",{Cols.Change}")
                .AppendSql($",{Cols.CreateAt}")
                .AppendSql($",{Cols.UpdateAt}")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql($" @{Cols.SourceId}")
                .AppendSql($",@{Cols.Word}")
                .AppendSql($",@{Cols.Pronunciation}")
                .AppendSql($",@{Cols.Syllable}")
                .AppendSql($",@{Cols.Kana}")
                .AppendSql($",@{Cols.Level}")
                .AppendSql($",@{Cols.Change}")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(",datetime('now', 'localtime')")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add($"@{Cols.SourceId}", this.SourceId);
            paramList.Add($"@{Cols.Word}", this.Word);
            paramList.Add($"@{Cols.Pronunciation}", this.Pronunciation);
            paramList.Add($"@{Cols.Syllable}", this.Syllable);
            paramList.Add($"@{Cols.Kana}", this.Kana);
            paramList.Add($"@{Cols.Level}", this.Level);
            paramList.Add($"@{Cols.Change}", this.Change);
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
                .AppendSql($",{Cols.Pronunciation}")
                .AppendSql($",{Cols.Syllable}")
                .AppendSql($",{Cols.Kana}")
                .AppendSql($",{Cols.Level}")
                .AppendSql($",{Cols.Change}")
                .AppendSql($",{Cols.SourceId}")
                .AppendSql($",{MeaningsEntity.Cols.Meaning}")
                .AppendSql($",{MeaningsEntity.Cols.PartOfSpeach}")
                .AppendSql($",{AdditionsEntity.Cols.Type}")
                .AppendSql($",{AdditionsEntity.Cols.Data}")
                .AppendSql("FROM")
                .AppendSql($" {TableName} ")
                .AppendSql($" INNER JOIN {MeaningsEntity.TableName} ON")
                .AppendSql($"     {TableName}.{Cols.Id} = {MeaningsEntity.Cols.WordId}")
                .AppendSql($" LEFT OUTER JOIN {AdditionsEntity.TableName} ON")
                .AppendSql($"     {MeaningsEntity.TableName}.{MeaningsEntity.Cols.Id} = {AdditionsEntity.Cols.MeaningId}")
                .AppendSql($"WHERE ");
            switch (matchType) {
                case MatchType.Prefix:
                case MatchType.Broad:
                    sql.AppendSql($"{Cols.Word} LIKE @{Cols.Word}");
                    break;
                default:
                    sql.AppendSql($"{Cols.Word} = @{Cols.Word}");
                    break;
            }
            sql.AppendSql($"ORDER BY ")
                .AppendSql($" {Cols.SourceId}")
                .AppendSql($",{Cols.Word}")
                .AppendSql($",{MeaningsEntity.Cols.PartOfSpeach}")
                .AppendSql($",{TableName}.{Cols.Id}");
            var paramList = new ParameterList();

            switch (matchType) {
                case MatchType.Prefix:
                    paramList.Add($"@{Cols.Word}", $"{word}%");
                    break;
                case MatchType.Broad:
                    paramList.Add($"@{Cols.Word}", $"%{word}%");
                    break;
                default:
                    paramList.Add($"@{Cols.Word}", word);
                    break;
            }
                    
            return base.Database.OpenRecordset(sql, paramList);
        }
        #endregion
    }
}