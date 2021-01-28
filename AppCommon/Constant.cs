using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.AppCommon {
    class Constant {
        /// <summary>
        /// アプリの設定関連情報
        /// </summary>
        public static readonly string SettingsFile = OsnCsLib.Common.Util.GetAppPath() + @"app.settings";
        
        /// <summary>
        /// アプリデータベース
        /// </summary>
        public static readonly string DatabaseFile = OsnCsLib.Common.Util.GetAppPath() + @"app.dic";

        /// <summary>
        /// 辞書種別
        /// </summary>
        public enum DicType : short {
            /// <summary>
            /// 英辞郎
            /// </summary>
            /// https://booth.pm/ja/items/777563
            Eijiro,
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
    }
}
