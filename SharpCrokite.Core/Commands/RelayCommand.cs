using System;
using System.Windows.Input;

namespace SharpCrokite.Core.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        private readonly Action targetExecuteMethod;
        private readonly Func<bool> targetCanExecuteMethod;

        public RelayCommand(Action executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
            {
                return targetCanExecuteMethod();
            }
            else if (targetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (targetExecuteMethod != null)
            {
                targetExecuteMethod.Invoke();
            }
        }
    }
}
