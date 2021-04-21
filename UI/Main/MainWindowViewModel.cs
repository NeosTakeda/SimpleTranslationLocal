using System;

namespace SimpleTranslationLocal.UI.Main {
    class MainWindowViewModel : BindableBase {

        #region Declaration
        #endregion

        #region Constructor
        internal MainWindowViewModel() {
        }
        #endregion

        #region Public Property
        /// <summary>
        /// search keyword
        /// </summary>
        private string _searchWord = "";
        public String SearchWord {
            get { return this._searchWord; }
            set { this.SetProperty(ref this._searchWord, value); }

        }

        /// <summary>
        /// spread
        /// </summary>
        private string _translatedText = "";
        public String TranslatedText {
            get { return this._translatedText; }
            set {
                if (this.SetProperty(ref this._translatedText, value)) {
                    this.SetProperty(nameof(CanUseSave));
                }
            }
        }

        /// <summary>
        /// cancel
        /// </summary>
        public BaseCommand CancelCommand { set; get; }

        /// <summary>
        /// show data
        /// </summary>
        public BaseCommand ShowDataCommand { set; get; }

        /// <summary>
        /// Ok Button enabled
        /// </summary>
        public bool CanUseSave {
            get {
                return (0 < this._translatedText.Length);
            }
        }
        #endregion
    }
}
