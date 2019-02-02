using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UI {



    public class ViewModelBase :INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected ICommand CreateCommand(Func<bool> canExecuteAction, Action executeAction) {
            Command command = new Command(canExecuteAction, executeAction);
            return command;
        }

        protected ICommand CreateCommand<T>(Func<T, bool> canExecuteAction, Action<T> executeAction) where T : class {
            Command<T> command = new Command<T>(canExecuteAction, executeAction);
            return command;
        }

    }


    public class Command :ICommand {
        private Func<bool> _canExecuteAction;
        private Action _executeAction;

        internal Command(Func<bool> canExecuteAction, Action executeAction) {
            _canExecuteAction = canExecuteAction;
            _executeAction = executeAction;
        }


        public bool CanExecute(object parameter) {
            var canExecute= _canExecuteAction();
            OnCanExecuteChanged();
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            _executeAction();
        }

        internal void OnCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }





    public class Command<T> :ICommand where T : class {
        private Func<T, bool> _canExecuteAction;
        private Action<T> _executeAction;

        internal Command(Func<T, bool> canExecuteAction, Action<T> executeAction) {
            _canExecuteAction = canExecuteAction;
            _executeAction = executeAction;
        }


        public bool CanExecute(object parameter) {
            var canExecute = _canExecuteAction(parameter as T);            
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            _executeAction(parameter as T);
        }

        internal void OnCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
