using Microsoft.EntityFrameworkCore;
using MyEveToolset.Data.Models;
using MyEveToolset.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEveToolset.Data.Queries
{
    public class AllHarvestablesQuery
    {
        private readonly MyEveToolDbContext dbContext;

        public AllHarvestablesQuery(MyEveToolDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IList<HarvestableViewModel> Execute()
        {
            List<HarvestableViewModel> harvestableViewModels = new();

            foreach (Harvestable harvestable in dbContext.Harvestables.Include(h => h.Prices).Include(h => h.MaterialContents))
            {
                harvestableViewModels.Add(new HarvestableViewModel
                {
                    HarvestableId = harvestable.HarvestableId,
                    Icon = harvestable.Icon,
                    Name = harvestable.Name,
                    Price = harvestable.Prices.FirstOrDefault() != null ? harvestable.Prices.First().SellMax : 0,
                    MaterialContents = MaterialContentsAsString(harvestable.MaterialContents),
                    Description = harvestable.Description,
                    IsCompressedVariantOfType = harvestable.IsCompressedVariantOfType
                });
            }

            return harvestableViewModels;
        }

        private string MaterialContentsAsString(List<MaterialContent> materialContents)
        {
            StringBuilder materialContentStringBuilder = new();

            foreach(MaterialContent material in materialContents)
            {
                materialContentStringBuilder.Append($"{dbContext.Materials.Find(material.MaterialId).Name}: {material.Quantity}\n");
            }

            string materialContentString = materialContentStringBuilder.ToString();
            string materialContentStringTrimmed = materialContentString.Substring(0, materialContentString.Length - 2);

            return materialContentStringTrimmed;
        }

        //private IList<MaterialContentViewModel> FindMaterialContentsForHarvestable(int harvestableId)
        //{
        //    List<MaterialContentViewModel> materialContentViewModels = new();

        //    foreach (MaterialContent materialContent in dbContext.MaterialContents
        //        .Where(mc => mc.HarvestableId == harvestableId).ToList())
        //    {
        //        materialContentViewModels.Add(new MaterialContentViewModel
        //        {
        //            HarvestableId = materialContent.HarvestableId,
        //            MaterialId = materialContent.MaterialId,
        //            Quantity = materialContent.Quantity
        //        });
        //    }

        //    return materialContentViewModels;
        //}
    }
}
