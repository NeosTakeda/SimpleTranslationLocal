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
    }
}
