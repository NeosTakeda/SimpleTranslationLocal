using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.UI.Import {
    class ImportMockViewModel : IImportViewModel {
        #region Property
        private string _eijiroFile;
        public override string EijiroFile {
            get { return this._eijiroFile; }
            set { this.SetProperty(ref this._eijiroFile, value); }
        }


        private string _dictionaryFile;
        public override string DictionaryFile {
            get { return this._dictionaryFile; }
            set { this.SetProperty(ref this._dictionaryFile, value); }
        }
        #endregion
    }
}
