using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharpCrokite.Core.Commands
{
    public class AsyncRelayCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool IsExecuting { get; private set; }

        private readonly Func<Task> targetExecuteMethod;
        private readonly Func<bool> targetCanExecuteMethod;
        private readonly Action targetNotifyPropertyChanged;

        public AsyncRelayCommand(Func<Task> targetExecuteMethod)
        {
            this.targetExecuteMethod = targetExecuteMethod;
        }

        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Func<bool> targetCanExecuteMethod)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetCanExecuteMethod = targetCanExecuteMethod;
        }
        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Action targetNotifyPropertyChanged)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetNotifyPropertyChanged = targetNotifyPropertyChanged;
        }

        public AsyncRelayCommand(Func<Task> targetExecuteMethod, Func<bool> targetCanExecuteMethod, Action targetNotifyPropertyChanged)
        {
            this.targetExecuteMethod = targetExecuteMethod;
            this.targetCanExecuteMethod = targetCanExecuteMethod;
            this.targetNotifyPropertyChanged = targetNotifyPropertyChanged;
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
                    targetNotifyPropertyChanged?.Invoke();
                    await targetExecuteMethod();
                }
                finally
                {
                    IsExecuting = false;
                    targetNotifyPropertyChanged?.Invoke();
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