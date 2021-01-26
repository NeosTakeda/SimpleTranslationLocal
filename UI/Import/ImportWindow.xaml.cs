using SimpleTranslationLocal.AppCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleTranslationLocal.UI.Import {
    /// <summary>
    /// ImportWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ImportWindow : Window {
        #region Declaration
        private IImportViewModel _model;
        #endregion

        #region Constructor
        public ImportWindow() {
            InitializeComponent();

            if (Env.Current == Env.EnvType.Stub) {
                this._model = new ImportMockViewModel(this, this.OnOKClick);
            } else {
                this._model = new ImportViewModel(this, this.OnOKClick);
            }
            this.DataContext = this._model;
        }
        #endregion

        #region Event 
        /// <summary>
        /// OKボタンクリック時
        /// </summary>
        public void OnOKClick() {
            this.Close();
        }
        #endregion
    }
}
