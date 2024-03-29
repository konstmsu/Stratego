using System;
using System.Windows.Input;

namespace Stratego.UI.Utility
{
    public class DelegateCommand : ICommand
    {
        readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;
    }
}