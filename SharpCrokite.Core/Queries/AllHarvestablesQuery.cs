using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class AllHarvestablesQuery
    {
        private readonly HarvestableRepository harvestableRepository;

        public AllHarvestablesQuery(HarvestableRepository harvestableRepository)
        {
            this.harvestableRepository = harvestableRepository;
        }

        public IEnumerable<HarvestableModel> Execute()
        {
            List<HarvestableModel> harvestableViewModels = new();

            foreach (Harvestable harvestable in harvestableRepository.All())
            {
                harvestableViewModels.Add(CreateHarvestableModelFrom(harvestable));
            }
            return harvestableViewModels;
        }

        private HarvestableModel CreateHarvestableModelFrom(Harvestable harvestable)
        {
            return new HarvestableModel
            {
                HarvestableId = harvestable.HarvestableId,
                Icon = harvestable.Icon,
                Name = harvestable.Name,
                Type = harvestable.Type,
                Price = harvestable.Prices.FirstOrDefault() != null ? new Isk(harvestable.Prices.First().SellMin) : new Isk(0m),
                MaterialContents = MaterialContentsAsString(harvestable.MaterialContents),
                Description = harvestable.Description,
                Volume = harvestable.Volume,
                IsCompressedVariantOfType = GetCompressedVariantOrDefault(harvestable)
            };
        }

        private HarvestableModel GetCompressedVariantOrDefault(Harvestable harvestable)
        {
            return harvestable.IsCompressedVariantOfType.HasValue
                ? CreateHarvestableModelFrom(harvestableRepository.Get(harvestable.IsCompressedVariantOfType.Value))
                : null;
        }

        private static string MaterialContentsAsString(IEnumerable<MaterialContent> materialContents)
        {
            StringBuilder materialContentStringBuilder = new();

            foreach (MaterialContent materialContent in materialContents)
            {
                materialContentStringBuilder.Append($"{materialContent.Material.Name}: {materialContent.Quantity}\n");
            }

            string materialContentString = materialContentStringBuilder.ToString();

            // this means position 0 until '..' 1 before last '^' 
            // thus it'll trim the last newline character :)
            string materialContentStringTrimmed = !string.IsNullOrWhiteSpace(materialContentString) ? materialContentString[..^1] : string.Empty;

            return materialContentStringTrimmed;
        }
    }
}
