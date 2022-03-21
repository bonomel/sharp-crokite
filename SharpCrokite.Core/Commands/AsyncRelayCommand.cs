using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpCrokite.Core.Commands
{
    public class AsyncRelayCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        private bool isExecuting;

        private readonly Func<Task> targetExecuteMethod;
        private readonly Func<bool> targetCanExecuteMethod;

        public AsyncRelayCommand(Func<Task> targetExecuteMethod)
        {
            this.targetExecuteMethod = targetExecuteMethod;
        }

        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Func<bool> targetCanExecuteMethod = null)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetCanExecuteMethod = targetCanExecuteMethod;
        }

        public bool CanExecute()
        {
            return !isExecuting && (targetCanExecuteMethod?.Invoke() ?? true);
        }
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    isExecuting = true;
                    await targetExecuteMethod();
                }
                finally
                {
                    isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetAsync();
        }


        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }
    }
}