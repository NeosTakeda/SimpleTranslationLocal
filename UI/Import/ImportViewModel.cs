using SimpleTranslationLocal.Data.Repo;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.UI.Import {
    class ImportViewModel : IImportViewModel {

        #region Declaration
        #endregion

        #region Property
        private string _eijiroFile = AppSettingsRepo.GetInstance().EijiroFile;
        public override string EijiroFile {
            get { return this._eijiroFile; }
            set { 
                base.SetProperty(ref this._eijiroFile, value);
                base.SetProperty(nameof(ImportEijiroEnabled));
            }
        }

        private string _dictionaryFile = AppSettingsRepo.GetInstance().DictionaryFile;
        public override string WebsterFile {
            get { return this._dictionaryFile; }
            set {
                base.SetProperty(ref this._dictionaryFile, value);
                base.SetProperty(nameof(ImportWebsterEnabled));
            }
        }
        #endregion

        #region Constructor
        public ImportViewModel(Window owner, Action OnOkClick) : base(owner, OnOkClick)  {
            
        }
        #endregion

        #region Protected Method

        #endregion
    }
}
