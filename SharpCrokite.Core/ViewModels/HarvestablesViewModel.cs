using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public class HarvestablesViewModel : INotifyPropertyChanged
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ObservableCollection<HarvestableModel> harvestables = new();
        public ObservableCollection<HarvestableModel> Harvestables
        { 
            get => harvestables;
            private set
            {
                if (Equals(value, harvestables)) return;
                harvestables = value;
                NotifyPropertyChanged(nameof(Harvestables));
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public HarvestablesViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            LoadHarvestables();
        }
        internal void UpdateHarvestables()
        {
            LoadHarvestables();
        }

        private void LoadHarvestables()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            AllHarvestablesQuery allHarvestablesQuery = new(harvestableRepository, materialRepository);
            Harvestables = new(allHarvestablesQuery.Execute());
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
