using System.Collections.Generic;

using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class StaticDataUpdateController
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;
        private readonly EsiStaticDataRetriever dataRetriever;

        public StaticDataUpdateController(EsiStaticDataRetriever dataRetriever,
            HarvestableRepository harvestableRepository,
            MaterialRepository materialRepository)
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

            updater.UpdateMaterials(materialDtos);
            updater.UpdateHarvestables(harvestableDtos);
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
