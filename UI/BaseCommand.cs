using System;
using System.Windows.Input;

namespace SimpleTranslationLocal.UI {
    class BaseCommand : ICommand {
        protected Action _action;

        public BaseCommand(Action action) {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            if (null != CanExecuteChanged) {
            }
            return _action != null;
        }

        public void Execute(object parameter) {
            _action?.Invoke();
        }
    }
}
