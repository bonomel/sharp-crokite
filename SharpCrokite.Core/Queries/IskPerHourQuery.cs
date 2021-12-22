using System;
using System.Collections.Generic;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public abstract class IskPerHourQuery<T> where T : HarvestableIskPerHour
    {
        private protected string[] HarvestableTypes;
        private readonly HarvestableRepository harvestableRepository;

        protected IskPerHourQuery(HarvestableRepository harvestableRepository)
        {
            this.harvestableRepository = harvestableRepository;
        }

        internal IEnumerable<T> Execute()
        {
            List<T> harvestableIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels =
                harvestableRepository.Find(h => HarvestableTypes.Contains(h.Type) && h.IsCompressedVariantOfType == null);

            foreach (Harvestable harvestableModel in harvestableModels)
            {
                T harvestableIskPerHour = Activator.CreateInstance<T>();

                harvestableIskPerHour.Id = harvestableModel.HarvestableId;
                harvestableIskPerHour.Icon = harvestableModel.Icon;
                harvestableIskPerHour.Name = harvestableModel.Name;
                harvestableIskPerHour.Description = harvestableModel.Description;
                harvestableIskPerHour.Volume = new Volume(harvestableModel.Volume);
                harvestableIskPerHour.Type = harvestableModel.Type;
                harvestableIskPerHour.MaterialContent = harvestableModel.MaterialContents.Select(materialContent =>
                    new MaterialModel
                    {
                        Name = materialContent.Material.Name,
                        Quantity = materialContent.Quantity,
                        Quality = materialContent.Material.Quality
                    }).ToList();

                harvestableIskPerHourCollection.Add(harvestableIskPerHour);
            }

            foreach (string oreType in HarvestableTypes)
            {
                List<T> harvestableIskPerHourPerType = harvestableIskPerHourCollection.Where(o => o.Type == oreType).ToList();

                List<List<T>> harvestableIskPerHourGrouped = new();

                foreach (T harvestableIskPerHour in harvestableIskPerHourPerType)
                {
                    if (harvestableIskPerHourPerType.Any(type =>
                        type.Name.Contains(harvestableIskPerHour.Name) && type.Name != harvestableIskPerHour.Name))
                    {
                        List<T> groupedByType = harvestableIskPerHourPerType.Where(type => type.Name.Contains(harvestableIskPerHour.Name)).ToList();
                        harvestableIskPerHourGrouped.Add(groupedByType);
                    }
                }

                if (harvestableIskPerHourGrouped.Any())
                {
                    foreach (List<T> harvestableGroup in harvestableIskPerHourGrouped)
                    {
                        // set the improved flag on the improved variants
                        harvestableGroup.Where(o => o != GetBasicOreType(harvestableGroup)).ToList().ForEach(o => o.IsImprovedVariant = true);
                    }
                }
            }

            return harvestableIskPerHourCollection;
        }

        private static T GetBasicOreType(IEnumerable<T> moonOreIskPerHourPerType)
        {
            return moonOreIskPerHourPerType.Aggregate(FindHarvestableWithLowestMaterialContent);
        }

        private static T FindHarvestableWithLowestMaterialContent(T harvestableToCompare, T harvestableToCompareTo)
        {
            foreach (MaterialModel materialToCompare in harvestableToCompare.MaterialContent)
            {
                MaterialModel materialToCompareTo = harvestableToCompareTo.MaterialContent.Single(material => material.Name == materialToCompare.Name);

                return materialToCompare.Quantity < materialToCompareTo.Quantity ? harvestableToCompare : harvestableToCompareTo;
            }

            return null;
        }
    }
}