using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using JetBrains.Annotations;
using SharpCrokite.Core.Commands;

namespace SharpCrokite.Core.ViewModels
{
    public class NavigatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IContentViewModel currentContentViewModel;

        public ICommand NavigationCommand => new NavigationCommand(this);

        [UsedImplicitly]
        public IContentViewModel CurrentContentViewModel
        {
            get => currentContentViewModel;
            set
            {
                currentContentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentContentViewModel));
            }
        }

        public readonly List<IContentViewModel> ContentViewModels = new();
        
        public NavigatorViewModel(IskPerHourViewModel iskPerHourViewModel, SurveyCalculatorViewModel surveyCalculatorViewModel)
        {
            currentContentViewModel = iskPerHourViewModel;

            ContentViewModels.Add(iskPerHourViewModel);
            ContentViewModels.Add(surveyCalculatorViewModel);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
