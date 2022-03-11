using System;
using JetBrains.Annotations;
using SharpCrokite.Core.Commands;

namespace SharpCrokite.Core.ViewModels
{
    public class NavigatorViewModel
    {
        [UsedImplicitly] public NavigationCommand NavigationCommand { get; private set; }

        public event Action<Type> CurrentViewModelChanged;

        public NavigatorViewModel()
        {
            NavigationCommand = new NavigationCommand(OnNavigation);
        }

        private void OnNavigation(object navigationTarget)
        {
            OnCurrentViewModelChanged(navigationTarget);
        }

        private void OnCurrentViewModelChanged(object navigationTarget)
        {
            CurrentViewModelChanged?.Invoke((Type)navigationTarget);
        }
    }
}
