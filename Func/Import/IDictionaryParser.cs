using OsnCsLib.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.Func.Import {
    abstract class IDictionaryParser {

        #region Declaration
        private FileOperator _operator;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file">対象のファイル</param>
        public IDictionaryParser(string file) {
            this._operator = new FileOperator(file);
        }
        #endregion
    }
}
