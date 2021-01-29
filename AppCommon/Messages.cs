using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleTranslationLocal.AppCommon {
    class Messages {
        #region Declaration
        /// <summary>
        /// エラーメッセージID
        /// </summary>
        public enum ErrId {
            Err001,
            Err002,
            Err999

        }
        private static Dictionary<ErrId, string> _errorMessages = new Dictionary<ErrId, string> {
             { ErrId.Err001, "{0}が見つかりません。" }
            ,{ ErrId.Err002, "データベースの作成に失敗しました。\n{0}" }
            ,{ ErrId.Err999, "不明なエラーです" }
        };
        #endregion

        #region Public Method
        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        /// <param name="id">メッセージID</param>
        /// <param name="text">大体文字列</param>
        public static void ShowError(ErrId id, params string[] words) {
            string message = _errorMessages[id];
            for (int i = 0; i < words.Length; i++) {
                message = message.Replace("{" + i + "}", words[i]);
            }
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}
