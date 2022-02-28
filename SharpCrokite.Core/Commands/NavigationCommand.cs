using System;
using System.Linq;
using System.Windows.Input;
using SharpCrokite.Core.ViewModels;

namespace SharpCrokite.Core.Commands
{
    public class NavigationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        private readonly NavigatorViewModel navigatorViewModel;

        public NavigationCommand(NavigatorViewModel navigatorViewModel)
        {
            this.navigatorViewModel = navigatorViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is string)
            {
                if(parameter.Equals("Test"))
                {
                    navigatorViewModel.CurrentContentViewModel = navigatorViewModel.ContentViewModels.First();
                }
                if (parameter.Equals("Test2"))
                {
                    navigatorViewModel.CurrentContentViewModel = navigatorViewModel.ContentViewModels.Last();
                }
            }
        }
    }
}
