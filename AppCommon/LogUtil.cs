using System.Diagnostics;

namespace SimpleTranslationLocal.AppCommon {
    class LogUtil {

        #region Public Method
        /// <summary>
        /// デバッグログを出力
        /// </summary>
        /// <param name="log">ログ</param>
        public static void DebugLog(string log) {
            Debug.WriteLine(log);
        }

        /// <summary>
        /// デバッグログを出力
        /// </summary>
        /// <param name="format">書式</param>
        /// <param name="args">値</param>
        public static void DebugLog(string format, params object[] args) {
            Debug.WriteLine(format, args);
        }
        #endregion
    }
}
