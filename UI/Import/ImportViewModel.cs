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
        private AppSettingsRepo _settings = AppSettingsRepo.GetInstance();      // このRepoはライブラリを継承しているので抽象化できず。。
        #endregion

        #region Property

        public override string EijiroFile {
            get { return _settings.EijiroFile; }
            set { 
                base.SetProperty();
                base.SetProperty(nameof(ImportEijiroEnabled));
            }
        }

        public override string DictionaryFile {
            get { return _settings.DictionaryFile; }
            set { 
                base.SetProperty();
                base.SetProperty(nameof(ImportDictionaryEnabled));
            }
        }
        #endregion

        #region Constructor
        public ImportViewModel(Window owner, Action OnOkClick) : base(owner, OnOkClick)  {
            
        }
        #endregion
    }
}
