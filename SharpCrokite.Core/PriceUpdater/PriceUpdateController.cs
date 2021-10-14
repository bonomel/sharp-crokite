using System.Collections.Generic;
using System.Linq;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.PriceUpdater
{
    public class PriceUpdateController
    {
        private readonly IRepository<Harvestable> harvestableRepository;
        private readonly IRepository<Material> materialRepository;

        private readonly EveMarketerPriceRetriever priceRetriever;
        
        public PriceUpdateController(EveMarketerPriceRetriever priceRetriever,
            IRepository<Harvestable> harvestableRepository,
            IRepository<Material> materialRepository)
        {
            this.priceRetriever = priceRetriever;
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        public void UpdatePrices()
        {
            IList<int> listOfHarvestableIds = harvestableRepository.All().Select(h => h.HarvestableId).ToList();
            IList<int> listOfMaterialIds = materialRepository.All().Select(m => m.MaterialId).ToList();

            IList<int> allTypeIds = listOfHarvestableIds.Concat(listOfMaterialIds).ToList();

            IList<PriceDto> prices = priceRetriever.Retrieve(allTypeIds);

            PriceUpdater priceUpdater = new(harvestableRepository, materialRepository);

            priceUpdater.Update(prices);
        }

        public void DeleteAllPrices()
        {
            harvestableRepository.All().ToList().ForEach(h => h.Prices = new List<Price>());
            materialRepository.All().ToList().ForEach(m => m.Prices = new List<Price>());

            harvestableRepository.SaveChanges();
            materialRepository.SaveChanges();
        }
    }
}
