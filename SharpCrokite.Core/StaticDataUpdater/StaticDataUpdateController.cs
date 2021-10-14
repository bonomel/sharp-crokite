using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System.Collections.Generic;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class StaticDataUpdateController
    {
        private readonly IRepository<Harvestable> harvestableRepository;
        private readonly IRepository<Material> materialRepository;
        private readonly EsiStaticDataRetriever dataRetriever;

        public StaticDataUpdateController(EsiStaticDataRetriever dataRetriever,
            IRepository<Harvestable> harvestableRepository,
            IRepository<Material> materialRepository)
        {
            this.dataRetriever = dataRetriever;
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        public void UpdateData()
        {
            IEnumerable<HarvestableDto> harvestableDtos = dataRetriever.RetrieveHarvestables();
            IEnumerable<MaterialDto> materialDtos = dataRetriever.RetrieveMaterials();

            StaticDataUpdater updater = new(harvestableRepository, materialRepository);

            updater.UpdateHarvestables(harvestableDtos);
            updater.UpdateMaterials(materialDtos);
        }

        public void DeleteAllStaticData()
        {
            harvestableRepository.DeleteAll();
            materialRepository.DeleteAll();

            harvestableRepository.SaveChanges();
            materialRepository.SaveChanges();
        }
    }
}
