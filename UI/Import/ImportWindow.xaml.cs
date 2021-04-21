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
        private readonly ImportViewModel _model;
        #endregion

        #region Constructor
        public ImportWindow() {
            InitializeComponent();

            this._model = new ImportViewModel(this, this.OnOKClick);
            this.DataContext = this._model;
        }
        #endregion

        #region Event
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (cProgressPanel.Visibility == Visibility.Visible) {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Enter || e.Key == Key.Escape) {
                e.Handled = true;
                this.Close();
            }
            
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
