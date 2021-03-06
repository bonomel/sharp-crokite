using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.PriceRetrievalService
{
    public class PriceUpdateHandler
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        private readonly IPriceRetrievalService priceRetrievalService;

        public PriceUpdateHandler(IPriceRetrievalService priceRetrievalService,
            HarvestableRepository harvestableRepository,
            MaterialRepository materialRepository)
        {
            this.priceRetrievalService = priceRetrievalService;
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        public async Task UpdatePrices()
        {
            IList<int> listOfHarvestableIds = harvestableRepository.All().Select(h => h.HarvestableId).ToList();
            IList<int> listOfMaterialIds = materialRepository.All().Select(m => m.MaterialId).ToList();

            IList<int> allTypeIds = listOfHarvestableIds.Concat(listOfMaterialIds).ToList();

            IEnumerable<PriceDto> prices = await priceRetrievalService.Retrieve(allTypeIds);

            PriceUpdater priceUpdater = new(harvestableRepository, materialRepository);

            await priceUpdater.Update(prices);
        }

        public async Task DeleteAllPrices()
        {
            harvestableRepository.All().ToList().ForEach(h => h.Prices = new List<Price>());
            materialRepository.All().ToList().ForEach(m => m.Prices = new List<Price>());

            await harvestableRepository.SaveChangesAsync();
            await materialRepository.SaveChangesAsync();
        }
    }
}
