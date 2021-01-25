using SimpleTranslationLocal.Data.Repo;
using System;
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

        private string _eijiroFile;
        public override string EijiroFile {
            get { return _settings.EijiroFile; }
            set { 
                _settings.EijiroFile = value; _settings.Save();
                base.SetProperty();
            }
        }

        public override string DictionaryFile {
            get { return _settings.DictionaryFile; }
            set { 
                _settings.DictionaryFile = value; _settings.Save();
                base.SetProperty();
            }
        }
        #endregion

        #region Constructor
        #endregion
    }
}
