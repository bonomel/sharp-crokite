using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System.Collections.ObjectModel;

namespace SharpCrokite.Core.ViewModels
{
    public class HarvestableViewModel
    {
        private readonly IRepository<Harvestable> harvestableRepository;
        private readonly IRepository<Material> materialRepository;

        public ObservableCollection<HarvestableModel> Harvestables
        {
            get;
            set;
        }

        public HarvestableViewModel()
        {
            SharpCrokiteDbContext dbContext = new();
            harvestableRepository = new HarvestableRepository(dbContext);
            materialRepository = new MaterialRepository(dbContext);

            LoadHarvestables();
        }

        private void LoadHarvestables()
        {
            AllHarvestablesQuery allHarvestablesQuery = new(harvestableRepository, materialRepository);
            Harvestables = new(allHarvestablesQuery.Execute());
        }
    }
}
