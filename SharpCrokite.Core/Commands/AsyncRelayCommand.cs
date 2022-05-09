using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpCrokite.Core.Commands
{
    public class AsyncRelayCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        private bool isExecuting;
        public bool IsExecuting
        {
            get => isExecuting;
            private set
            {
                isExecuting = value;
                targetNotifyIsExecutingChanged?.Invoke();
            }
        }

        private readonly Func<Task> targetExecuteMethod;
        private readonly Func<bool> targetCanExecuteMethod;
        private readonly Action targetNotifyIsExecutingChanged;

        public AsyncRelayCommand(Func<Task> targetExecuteMethod)
        {
            this.targetExecuteMethod = targetExecuteMethod;
        }

        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Func<bool> targetCanExecuteMethod)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetCanExecuteMethod = targetCanExecuteMethod;
        }
        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Action targetNotifyIsExecutingChanged)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetNotifyIsExecutingChanged = targetNotifyIsExecutingChanged;
        }

        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Func<bool> targetCanExecuteMethod, Action targetNotifyIsExecutingChanged)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetCanExecuteMethod = targetCanExecuteMethod;
            this.targetNotifyIsExecutingChanged = targetNotifyIsExecutingChanged;
        }

        public bool CanExecute()
        {
            return !IsExecuting && (targetCanExecuteMethod?.Invoke() ?? true);
        }
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    IsExecuting = true;
                    await targetExecuteMethod();
                }
                finally
                {
                    IsExecuting = false;
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