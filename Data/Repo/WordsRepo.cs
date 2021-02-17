using SimpleTranslationLocal.AppCommon;
using SimpleTranslationLocal.Data.Repo.Entity;
using SimpleTranslationLocal.Data.Repo.Entity.DataModel;
using System.Collections.Generic;
using static SimpleTranslationLocal.AppCommon.Constants;

namespace SimpleTranslationLocal.Data.Repo {

    /// <summary>
    /// words repo
    /// </summary>
    internal class WordsRepo : IBasicRepo<WordData> {

        #region Declaration
        private WordsEntity _entity;
        #endregion

        #region Constructor
        internal WordsRepo(DictionaryDatabase database) : base(database) {
            this._entity = new WordsEntity(database);
        }
        #endregion

        #region Public Method
        internal override bool Create() {
            return this._entity.Create();
        }

        internal override long Insert() {
            return this._entity.Insert();
        }

        internal override void DeleteBySourceId(long id) {
            this._entity.DeleteBySourceId(id);
        }

        internal override void SetDataModel(WordData model) {
            this._entity.Id = model.Id;
            this._entity.SourceId = model.SourceId;
            this._entity.Word = model.Word;
            this._entity.Pronunciation = model.Pronunciation;
            this._entity.Syllable = model.Syllable;
            this._entity.Kana = model.Kana;
            this._entity.Level = model.Level;
            this._entity.Word = model.Word;
            this._entity.Change = model.Change;
        }

        /// <summary>
        /// search dictionary
        /// </summary>
        /// <param name="word">word</param>
        /// <param name="matchType">search matching type</param>
        /// <returns>result. if data does not find, return null</returns>
        internal List<WordData> Search(string word, MatchType matchType) {
            using (var recset = this._entity.Search(word, matchType)) {
                if (!recset.HasRows) {
                    return null;
                }

                var result = new List<WordData>();
                WordData wordData = null;
                MeaningData meaningData = null;
                int wordCount = 1;

                while (recset.Read()) {
                    if (Constants.MaxNumberOfListWord < wordCount) {
                        return result;
                    }

                    if (null == wordData || wordData.Word != recset.GetString(WordsEntity.Cols.Word)) {
                        wordData = new WordData();
                        result.Add(wordData);
                        meaningData = null;
                        wordCount++;
                    }

                    if (0 == wordData.Word.Length) {
                        wordData.Word = recset.GetString(WordsEntity.Cols.Word);
                        wordData.Pronunciation = recset.GetString(WordsEntity.Cols.Pronunciation);
                        wordData.Syllable = recset.GetString(WordsEntity.Cols.Syllable);
                        wordData.Kana = recset.GetString(WordsEntity.Cols.Kana);
                        wordData.Level = recset.GetInt(WordsEntity.Cols.Level);
                        wordData.Change = recset.GetString(WordsEntity.Cols.Change);
                    }

                    if (null == meaningData || meaningData.Meaning != recset.GetString(MeaningsEntity.Cols.Meaning)) {
                        meaningData = new MeaningData();
                        wordData.Meanings.Add(meaningData);
                    }
                    if (0 == meaningData.Meaning.Length) {
                        meaningData.SourceId = recset.GetInt(WordsEntity.Cols.SourceId);
                        meaningData.Meaning = recset.GetString(MeaningsEntity.Cols.Meaning);
                        meaningData.PartOfSpeach = recset.GetString(MeaningsEntity.Cols.PartOfSpeach);
                    }

                    if (0 < recset.GetInt(AdditionsEntity.Cols.Type)) {
                        var additionData = new AdditionData();
                        additionData.Type = recset.GetInt(AdditionsEntity.Cols.Type);
                        additionData.Data = recset.GetString(AdditionsEntity.Cols.Data);
                        meaningData.Additions.Add(additionData);
                    }
                }

                return result;
            }
        }
        #endregion
    }
}
