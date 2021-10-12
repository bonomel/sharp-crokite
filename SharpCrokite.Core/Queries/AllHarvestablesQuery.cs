using SharpCrokite.Core.ViewModels;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharpCrokite.DataAccess.Queries
{
    public class AllHarvestablesQuery
    {
        private readonly IRepository<Harvestable> harvestableRepository;
        private readonly IRepository<Material> materialRepository;

        private readonly NumberFormatInfo ISKNumberFormatInfo;

        public AllHarvestablesQuery(IRepository<Harvestable> harvestableRepository, IRepository<Material> materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            ISKNumberFormatInfo = new NumberFormatInfo()
            {
                CurrencyDecimalSeparator = ",",
                CurrencyDecimalDigits = 2,
                CurrencyGroupSeparator = ".",
                CurrencyGroupSizes = new int[] { 3 },
                CurrencySymbol = "ISK",
                CurrencyPositivePattern = 3,
                CurrencyNegativePattern = 8
            };
        }

        public IList<HarvestableViewModel> Execute()
        {
            List<HarvestableViewModel> harvestableViewModels = new();

            foreach (Harvestable harvestable in harvestableRepository.All())
            {
                harvestableViewModels.Add(new HarvestableViewModel
                {
                    HarvestableId = harvestable.HarvestableId,
                    Icon = harvestable.Icon,
                    Name = harvestable.Name,
                    Price = harvestable.Prices.FirstOrDefault() != null ? DisplayAsISK(harvestable.Prices.First().SellMin) : "N/A",
                    MaterialContents = MaterialContentsAsString(harvestable.MaterialContents),
                    Description = harvestable.Description,
                    IsCompressedVariantOfType = harvestable.IsCompressedVariantOfType
                });
            }

            return harvestableViewModels;
        }

        private string DisplayAsISK(decimal decimalISK)
        {
            return $"{decimalISK.ToString("C", ISKNumberFormatInfo)}";
        }

        private string MaterialContentsAsString(IEnumerable<MaterialContent> materialContents)
        {
            StringBuilder materialContentStringBuilder = new();

            foreach(MaterialContent material in materialContents)
            {
                materialContentStringBuilder.Append($"{materialRepository.Get(material.MaterialId).Name}: {material.Quantity}\n");
            }

            string materialContentString = materialContentStringBuilder.ToString();
            string materialContentStringTrimmed = materialContentString.Substring(0, materialContentString.Length - 2);

            return materialContentStringTrimmed;
        }
    }
}
