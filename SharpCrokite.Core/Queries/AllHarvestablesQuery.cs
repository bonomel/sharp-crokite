using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class AllHarvestablesQuery
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        private static readonly NumberFormatInfo ISKNumberFormatInfo = new()
        {
            CurrencyDecimalSeparator = ",",
            CurrencyDecimalDigits = 2,
            CurrencyGroupSeparator = ".",
            CurrencyGroupSizes = new int[] { 3 },
            CurrencySymbol = "ISK",
            CurrencyPositivePattern = 3,
            CurrencyNegativePattern = 8
        };

        public AllHarvestablesQuery(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        public IList<HarvestableModel> Execute()
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
            return new HarvestableModel()
            {
                HarvestableId = harvestable.HarvestableId,
                Icon = harvestable.Icon,
                Name = harvestable.Name,
                Type = harvestable.Type,
                Price = harvestable.Prices.FirstOrDefault() != null ? DisplayAsISK(harvestable.Prices.First().SellMin) : "N/A",
                MaterialContents = MaterialContentsAsString(harvestable.MaterialContents),
                Description = harvestable.Description,
                Volume = harvestable.Volume,
                IsCompressedVariantOfType = harvestable.IsCompressedVariantOfType.HasValue
                    ? CreateHarvestableModelFrom(harvestableRepository.Get(harvestable.IsCompressedVariantOfType.Value))
                    : null
            };
        }

        private string DisplayAsISK(decimal decimalISK)
        {
            return decimalISK != 0 ? $"{decimalISK.ToString("C", ISKNumberFormatInfo)}" : "N/A";
        }

        private string MaterialContentsAsString(IEnumerable<MaterialContent> materialContents)
        {
            StringBuilder materialContentStringBuilder = new();

            foreach (MaterialContent materialContent in materialContents)
            {
                materialContentStringBuilder.Append($"{materialContent.Material.Name}: {materialContent.Quantity}\n");
            }

            string materialContentString = materialContentStringBuilder.ToString();

            // this means position 0 until '..' 1 before last '^' 
            // thus it'll trim the last newline character :)
            string materialContentStringTrimmed = !string.IsNullOrWhiteSpace(materialContentString) ? materialContentString[0..^1] : string.Empty;

            return materialContentStringTrimmed;
        }
    }
}
