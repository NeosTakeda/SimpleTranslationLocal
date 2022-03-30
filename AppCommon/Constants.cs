using System.Collections.Generic;

namespace SimpleTranslationLocal.AppCommon {
    class Constants {
        /// <summary>
        /// アプリの設定関連情報
        /// </summary>
        public static readonly string SettingsFile = OsnCsLib.Common.Util.GetAppPath() + @"app.settings";
        
        /// <summary>
        /// アプリデータベース
        /// </summary>
        public static readonly string DatabaseFile = OsnCsLib.Common.Util.GetAppPath() + @"app.dic";

        /// <summary>
        /// データ格納フォルダルート
        /// </summary>
        public static readonly string DataFolder = OsnCsLib.Common.Util.GetAppPath() + @"data\";

        /// <summary>
        /// 英辞郎データ格納フォルダ
        /// </summary>
        public static readonly string EijiroData = DataFolder + "1";

        /// <summary>
        /// Websterデータ格納フォルダ
        /// </summary>
        public static readonly string WebsterData = DataFolder + "2";

        /// <summary>
        /// データ格納フォルダ
        /// </summary>
        public static readonly string[]  DataDirs = new string[] { WebsterData, EijiroData, };

        /// <summary>
        /// テンプレートHTML
        /// </summary>
        public static readonly string TemplateHtmlFile = OsnCsLib.Common.Util.GetAppPath() + @"Res\ResultHtml.txt";

        /// <summary>
        /// 該当データなしHTML
        /// </summary>
        public static readonly string NoDataHtmlFile = OsnCsLib.Common.Util.GetAppPath() + @"Res\NoDataHtml.txt";

        /// <summary>
        /// リストに表示する最大単語数
        /// </summary>
        public static readonly int MaxNumberOfListWord = 5;

        /// <summary>
        /// 辞書種別
        /// </summary>
        public enum DicType : short {
            /// <summary>
            /// 英辞郎
            /// </summary>
            /// https://booth.pm/ja/items/777563
            Eijiro = 1,
            /// <summary>
            /// GitHubに上がっている英英データ
            /// </summary>
            /// https://github.com/matthewreagan/WebstersEnglishDictionary
            Webster,
            /// <summary>
            /// まだ未確認
            /// </summary>
            /// https://www.vector.co.jp/soft/data/writing/se323658.html
            WordNet,
        }
        public static Dictionary<DicType, string> DicTypeName = new Dictionary<DicType, string> {
            { DicType.Eijiro, "英辞郎" },
            { DicType.Webster, "Webster" },
            { DicType.WordNet, "WordNet" }
        };

        /// <summary>
        /// 追加情報の種別
        /// </summary>
        public static class AdditionType {
            /// <summary>
            /// 不明
            /// </summary>
            public const int Unknown = 0;

            /// <summary>
            /// 用例
            /// </summary>
            public const int Example = 1;

            /// <summary>
            /// 補足情報
            /// </summary>
            public const int Supplement = 2;
        }

        /// <summary>
        /// Search match type
        /// </summary>
        public enum MatchType {
            /// <summary>
            /// 完全一致
            /// </summary>
            Exact = 1,

            /// <summary>
            /// 前方一致
            /// </summary>
            Prefix,
        }
    }
}
