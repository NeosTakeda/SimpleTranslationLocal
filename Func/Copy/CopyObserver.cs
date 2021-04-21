using SimpleTranslationLocal.AppCommon;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace SimpleTranslationLocal.Func.Copy {
    internal class CopyObserver : IDisposable {

        #region Declaration
        [DllImport("user32.dll")]
        private static extern bool AddClipboardFormatListener(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool RemoveClipboardFormatListener(IntPtr hWnd);

        private const int WM_DRAWCLIPBOARD = 0x031D;

        readonly IntPtr _handle;
        readonly HwndSource _hwndSource;

        private bool _disposed = false;
        private bool _isStart = false;

        internal delegate void ClipboardChangedHandler(string text);
        private ClipboardChangedHandler _callback;
        #endregion

        #region  Constructor/Destructor
        internal CopyObserver(Window owner, ClipboardChangedHandler callback) {
            this._callback = callback;
            this._handle = new WindowInteropHelper(owner).Handle;
            this._hwndSource = HwndSource.FromHwnd(this._handle);
            this._hwndSource.AddHook(MainWindowProc);
        }
        ~CopyObserver() {
            this.Dispose(false);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="isDisposing">set true if need to release managed source</param>
        private void Dispose(bool isDisposing) {
            this.ReleaseUnManagedSource();
            if (!this._disposed) {
                if (isDisposing) {
                    this.ReleaseManagedSource();
                }
                this._disposed = true;
            }
        }
        /// <summary>
        /// Release unmanaged source. Called when `Dispose(bool)` is called.
        /// </summary>
        private void ReleaseUnManagedSource() {
            this.Stop();
        }

        /// <summary>
        /// Release managed source. Called when `Dispose(bool)` is called.
        /// </summary>
        private void ReleaseManagedSource() {
            this._callback = null;
        }

        /// <summary>
        /// start to subscribe
        /// </summary>
        public void Start() {
            if (!this._isStart) {
                AddClipboardFormatListener(this._handle);
                this._isStart = true;
            }
        }

        /// <summary>
        /// stop subscribe
        /// </summary>
        public void Stop() {
            if (this._isStart) {
                RemoveClipboardFormatListener(this._handle);
                this._isStart = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr MainWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            switch (msg) {
                case WM_DRAWCLIPBOARD: 
                    if (Clipboard.ContainsText()) {
                        LogUtil.DebugLog("#### WM_DRAWCLIPBOARD Get" + DateTime.Now.ToString("hh:mm:ss.fff"));
                        this._callback?.Invoke(Clipboard.GetText(TextDataFormat.UnicodeText));
                        LogUtil.DebugLog("#### WM_DRAWCLIPBOARD Clear" + DateTime.Now.ToString("hh:mm:ss.fff"));
                    }
                    break;
            }
            return IntPtr.Zero;
        }
        #endregion
    }
}
