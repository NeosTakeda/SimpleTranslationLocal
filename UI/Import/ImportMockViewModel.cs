using System;
using System.Windows;

namespace SimpleTranslationLocal.UI.Import {
    class ImportMockViewModel : IImportViewModel {

        #region Property
        private string _eijiroFile = @"D:\OneDriveForBusiness\OneDrive - TK\storage\tools\Mouse Dictionary\EIJIRO-1448_ForTest.TXT";
        public override string EijiroFile {
            get { return this._eijiroFile; }
            set { this.SetProperty(ref this._eijiroFile, value); }
        }


        private string _websterFile = @"D:\OneDriveForBusiness\OneDrive - TK\storage\tools\Mouse Dictionary\dictionary.json";
        public override string WebsterFile {
            get { return this._websterFile; }
            set { this.SetProperty(ref this._websterFile, value); }
        }
        #endregion

        #region Constructor
        public ImportMockViewModel(Window owner, Action OnOkClick) : base(owner, OnOkClick) {

        }
        #endregion

        #region Protected Method

        #endregion
    }
}
