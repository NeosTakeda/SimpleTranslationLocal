using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleTranslationLocal.AppCommon {
    class Message {
        #region Declaration
        /// <summary>
        /// エラーメッセージID
        /// </summary>
        public enum ErrId {
            Err001,
            Err999

        }
        private Dictionary<ErrId, string> _errorMessages = new Dictionary<ErrId, string> {
             { ErrId.Err001, "{0}が見つかりません。" }
            ,{ ErrId.Err999, "不明なエラーです" }
        };
        #endregion

        #region Public Method
        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        /// <param name="id">メッセージID</param>
        /// <param name="text">大体文字列</param>
        public void ShowError(ErrId id, params string[] words) {
            string message = this._errorMessages[id];
            for (int i = 0; i < words.Length; i++) {
                message = message.Replace("{" + i + "}", words[i]);
            }
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}
