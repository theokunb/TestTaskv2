using System;
using System.Windows.Input;

namespace TestTaskv2.Core
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
