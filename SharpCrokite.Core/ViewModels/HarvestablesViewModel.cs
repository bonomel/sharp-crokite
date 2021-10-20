using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public class HarvestablesViewModel : BindableBase
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;
        private ObservableCollection<HarvestableModel> harvestables;

        public ObservableCollection<HarvestableModel> Harvestables
        {
            get { return harvestables; }
            private set { harvestables = value; }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public HarvestablesViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            LoadHarvestables();
        }

        private void LoadHarvestables()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            AllHarvestablesQuery allHarvestablesQuery = new(harvestableRepository, materialRepository);
            harvestables = new(allHarvestablesQuery.Execute());
        }
    }
}
