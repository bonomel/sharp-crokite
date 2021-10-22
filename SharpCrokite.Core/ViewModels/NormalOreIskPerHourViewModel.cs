using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.ObjectModel;

namespace SharpCrokite.Core.ViewModels
{
    public class NormalOreIskPerHourViewModel
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        private ObservableCollection<NormalOreIskPerHour> normalOreIskPerHourCollection = new();

        public ObservableCollection<NormalOreIskPerHour> NormalOreIskPerHourCollection
        { 
            get => normalOreIskPerHourCollection;
            private set
            {
                normalOreIskPerHourCollection = value;
            }
        }

        public NormalOreIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            normalOreIskPerHourCollection = LoadNormalIskPerHour();
        }

        private ObservableCollection<NormalOreIskPerHour> LoadNormalIskPerHour()
        {
            NormalOreIskPerHourQuery normalOreIskPerHourQuery = new(harvestableRepository, materialRepository);
            return new(normalOreIskPerHourQuery.Execute());
        }
    }
}
