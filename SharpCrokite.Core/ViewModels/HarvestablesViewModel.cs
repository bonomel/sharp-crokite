using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public class HarvestablesViewModel
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public ObservableCollection<HarvestableModel> Harvestables { get; private set; }

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
            Harvestables = new(allHarvestablesQuery.Execute());
        }
    }
}
