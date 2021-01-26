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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OsnCsLib.WPFComponent;
using OsnCsLib.Common;

using SimpleTranslationLocal.Data.Repo;
using SimpleTranslationLocal.AppCommon;

namespace SimpleTranslationLocal.UI.Main {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : ResidentWindow {

        #region Constructor
        public MainWindow() {
            InitializeComponent();
        }
        #endregion

        #region Protected Method
        /// <summary>
        /// setup
        /// </summary>
        protected override void SetUp() {
            // setup hotkey and notifiation icon
            base.SetUpHotKey(ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt, Key.L);
            base.SetupNofityIcon("SimpleTranslationLocal", new System.Drawing.Icon("app.ico"));

            // restore window
            var settings = AppSettingsRepo.Init(Constant.SettingsFile);
            Util.SetWindowXPosition(this, settings.X);
            Util.SetWindowYPosition(this, settings.Y);
            this.Width = settings.Width;
            this.Height = settings.Height;
            this.Topmost = settings.Topmost;


            // create a context menu
            base.AddContextMenu("Show", (sender, e) => this.OnContextMenuShowClick());
            base.AddContextMenu("Import", (sender, e) => this.OnContextMenuImportClick());
            base.AddContextMenuSeparator();
            base.AddContextMenu("Exit", (sender, e) => this.OnContextMenuExitClick());

            // add event
            this.Activated += (sender, e) => this.cKeyword.Focus();
            this.Minimized += this.MainWindowMinimized;
            this.Normalized += this.MainWindowNormalized;

            //this.SetContextMenuEnabled();

            // set view model
            var model = new MainWindowViewModel();
            //model.SaveAction = new Action(this.SaveAction);
            //model.CancelAction = new Action(this.CancelAction);
            //model.ShowDataAction = new Action(this.ShowDataAction);
            this.DataContext = model;
        }
        #endregion

        #region Event
        /// <summary>
        /// key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Keyword_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                e.Handled = true;


                this.cBrowser.NavigateToString(@"
<!DOCTYPE html>
<html lang='ja'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
<style>
html, body, div, span, applet, object, iframe,
h1, h2, h3, h4, h5, h6, p, blockquote, pre,
a, abbr, acronym, address, big, cite, code,
del, dfn, em, img, ins, kbd, q, s, samp,
small, strike, strong, sub, sup, tt, var,
b, u, i, center,
dl, dt, dd, ol, ul, li,
fieldset, form, label, legend,
table, caption, tbody, tfoot, thead, tr, th, td,
article, aside, canvas, details, embed, 
figure, figcaption, footer, header, hgroup, 
menu, nav, output, ruby, section, summary,
time, mark, audio, video {
	margin: 0;
	padding: 0;
	border: 0;
	font-size: 100%;
	font: inherit;
	vertical-align: baseline;
}
/* HTML5 display-role reset for older browsers */
article, aside, details, figcaption, figure, 
footer, header, hgroup, menu, nav, section {
	display: block;
}
body {
	line-height: 1;
}
ol, ul {
	list-style: none;
}
blockquote, q {
	quotes: none;
}
blockquote:before, blockquote:after,
q:before, q:after {
	content: '';
	content: none;
}
table {
	border-collapse: collapse;
	border-spacing: 0;
}



body {
    background-color: #FEFEFE;
    color: #333333;
    font-family: 'Meiryo UI';
    font-size: 11pt;
    margin:1em;
    line-height: 1.6em;
}
h1 {
    font-size: 15pt;
    font-weight: bold;
}
h4 {
    color: #666666;
    font-weight: bold;
    margin-top: 1em;
}
ul {
    margin-top: 0.5em;
    margin-left: 1em;
}
li {
    margin-top: 0.3em;
}
hr {
    margin: 1em 0;
}
.info {
    color: #5C5E60;
    margin-top: 0.7em;
}
.syllable, .pronumciation, multiples{
    background-color: #f1f1f2;
    color: #003b46;
    font-size: 10pt;
    padding: 0.2em;
}
.note {
    color: #666666;
    margin-left: 1em;
    font-size: 10.5pt;
}
</style>
</head>
<body>

<main>
<h1>human</h1>
<div class='info'>
<span class='syllable'>音節</span> hu・man&nbsp;&nbsp;<span class='pronumciation'>発音</span> hju':mэn
</div>

<section>
<h4>形容詞</h4>
<ul>
<li>人［人間］の［に関する］</li>
<li>〔動物や機械と異なり〕人間らしい［くさい］、思いやりのある</li>
<li>〔神と異なり〕間違いを犯す、不完全な
<div class='note'>
I'm (only) human. 私も（ただの）人間だ。<br/>
「だから間違いもする」という含みがある。</div>
</li>
<li>〔神や霊などが〕人の形を取った、人となった</li>
<li>人から構成される、人間で作られた</li>
<li>人間のような</li>
</ul>
</section>
<h4>名詞</h4>
<div class='info'>
<span class='syllable'>複数形</span> humans
</div>
<ul>
<li>〈話〉〔動物と異なる〕人、人間</li>
</ul>
</section>
</main>

<hr/>


<main>
<h1>human</h1>
<div class='info1'>
<span class='syllable'>音節</span> hu・man&nbsp;&nbsp;<span class='pronumciation'>発音</span> hju':mэn
</div>

<section>
<h4>形容詞</h4>
<ul>
<li>人［人間］の［に関する］</li>
<li>〔動物や機械と異なり〕人間らしい［くさい］、思いやりのある</li>
<li>〔神と異なり〕間違いを犯す、不完全な
<div class='note'>
I'm (only) human. 私も（ただの）人間だ。
「だから間違いもする」という含みがある。</div>
</li>
<li>〔神や霊などが〕人の形を取った、人となった</li>
<li>人から構成される、人間で作られた</li>
<li>人間のような</li>
</ul>
</section>
<h2>名詞</h2>
<span class='syllable'>複数形</span> humans
<ul>
<li>〈話〉〔動物と異なる〕人、人間</li>
</ul>
</section>
</main>

</body>
</html>

");
            }
        }
        #endregion

        #region Protected Method

        #endregion

        #region Private Method
        private void OnContextMenuShowClick() {
            base.SetWindowsState(false);
        }

        private void OnContextMenuImportClick() {
            base.SetNotifyIconVisible(false);
            new SimpleTranslationLocal.UI.Import.ImportWindow().ShowDialog();
            base.SetNotifyIconVisible(true);
        }

        private void OnContextMenuExitClick() {
        }

        private void MainWindowMinimized() {

        }

        private void MainWindowNormalized() {

        }
        #endregion
    }
}
