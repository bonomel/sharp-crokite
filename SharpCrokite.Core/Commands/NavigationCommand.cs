using System;
using System.Windows.Input;

namespace SharpCrokite.Core.Commands
{
    public class NavigationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        private readonly Action<object> targetExecuteMethod;

        public NavigationCommand(Action<object> executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            targetExecuteMethod?.Invoke(parameter);
        }
    }
}
