using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.UI.Import {
    abstract class IImportViewModel : BindableBase {

        #region Property
        /// <summary>
        /// 取込む英辞郎のファイル
        /// </summary>
        public virtual string EijiroFile {
            set;
            get;
        }
        /// <summary>
        /// 取込むDictionaryのファイル
        /// </summary>
        public virtual string DictionaryFile {
            set;
            get;
        }
        #endregion

        #region Action
        ///// <summary>
        ///// 英辞郎ファイル選択ボタンクリック時
        ///// </summary>
        //public abstract void SelectEijiroFileClickAction();

        ///// <summary>
        ///// 英辞郎ファイルインポートボタンクリック時
        ///// </summary>
        //public abstract void ImportEijiroFileClickAction();

        ///// <summary>
        ///// Dictionaryファイル選択ボタンクリック時
        ///// </summary>
        //public abstract void SelectDictionaryFileClickAction();

        ///// <summary>
        ///// Dictionaryファイルインポートボタンクリック時
        ///// </summary>
        //public abstract void ImportDictionaryFileClickAction();

        ///// <summary>
        ///// OKボタンクリック時
        ///// </summary>
        //public abstract void OKClickAction();
        #endregion
    }
}
