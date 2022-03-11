using System;
using System.ComponentModel;
using JetBrains.Annotations;
using SharpCrokite.Core.Models;

namespace SharpCrokite.Core.ViewModels
{
    [UsedImplicitly]
    public class IskPerHourViewModel : IContentViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [UsedImplicitly] public IskPerHourGridViewModel<AsteroidIskPerHour> AsteroidIskPerHourGridViewModel { get; }

        [UsedImplicitly] public IskPerHourGridViewModel<MoonOreIskPerHour> MoonOreIskPerHourGridViewModel { get; }

        [UsedImplicitly] public IskPerHourGridViewModel<IceIskPerHour> IceIskPerHourGridViewModel { get; }

        private int selectedIndex;

        [UsedImplicitly]
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex.Equals(value) is false)
                {
                    selectedIndex = value;
                    NotifyPropertyChanged(nameof(selectedIndex));
                }
            }
        }

        public IskPerHourViewModel(AsteroidIskPerHourGridViewModel asteroidIskPerHourGridViewModel,
            MoonOreIskPerHourGridViewModel moonOreIskPerHourGridViewModel, IceIskPerHourGridViewModel iceIskPerHourGridViewModel,
            NavigatorViewModel navigatorViewModel)
        {
            AsteroidIskPerHourGridViewModel = asteroidIskPerHourGridViewModel;
            MoonOreIskPerHourGridViewModel = moonOreIskPerHourGridViewModel;
            IceIskPerHourGridViewModel = iceIskPerHourGridViewModel;

            navigatorViewModel.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        // This is necessary because switching viewmodels resets the selected tab, which we don't want
        private void OnCurrentViewModelChanged(Type type)
        {
            if (type == typeof(IskPerHourViewModel))
            {
                SelectedIndex = selectedIndex;
            }
        }
        
        private void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdatePrices()
        {
            AsteroidIskPerHourGridViewModel.UpdatePrices();
            MoonOreIskPerHourGridViewModel.UpdatePrices();
            IceIskPerHourGridViewModel.UpdatePrices();
        }

        public void ReloadStaticData()
        {
            AsteroidIskPerHourGridViewModel.ReloadStaticData();
            MoonOreIskPerHourGridViewModel.ReloadStaticData();
            IceIskPerHourGridViewModel.ReloadStaticData();
        }
    }
}