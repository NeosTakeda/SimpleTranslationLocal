using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleTranslationLocal.UI {
    class BaseAction : ICommand {
        protected Action _action;

        public BaseAction(Action action) {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            if (null != CanExecuteChanged) {
            }
            return _action != null;
        }

        public void Execute(object parameter) { //今回は引数を使わずActionを実行
            _action?.Invoke();
        }
    }
}
