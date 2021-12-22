using System.Collections.Generic;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class MoonOreQuery
    {
        private readonly HarvestableRepository harvestableRepository;

        private static class MoonOreType
        {
            public const string UbiquitousMoonAsteroids = "Ubiquitous Moon Asteroids";
            public const string CommonMoonAsteroids = "Common Moon Asteroids";
            public const string UncommonMoonAsteroids = "Uncommon Moon Asteroids";
            public const string RareMoonAsteroids = "Rare Moon Asteroids";
            public const string ExceptionalMoonAsteroids = "Exceptional Moon Asteroids";
        }

        private static readonly string[] MoonOreTypes = {
            MoonOreType.UbiquitousMoonAsteroids,
            MoonOreType.CommonMoonAsteroids,
            MoonOreType.UncommonMoonAsteroids,
            MoonOreType.RareMoonAsteroids,
            MoonOreType.ExceptionalMoonAsteroids
        };

        public MoonOreQuery(HarvestableRepository harvestableRepository)
        {
            this.harvestableRepository = harvestableRepository;
        }

        internal IEnumerable<MoonOreIskPerHour> Execute()
        {
            List<MoonOreIskPerHour> moonOreIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels =
                harvestableRepository.Find(h => MoonOreTypes.Contains(h.Type) && h.IsCompressedVariantOfType == null);

            foreach (Harvestable harvestableModel in harvestableModels)
            {
                moonOreIskPerHourCollection.Add(new MoonOreIskPerHour
                {
                    Id = harvestableModel.HarvestableId,
                    //CompressedVariantTypeId = harvestableRepository.Find(h => h.IsCompressedVariantOfType == harvestableModel.HarvestableId).Single().HarvestableId,
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Volume = new Volume(harvestableModel.Volume),
                    Type = harvestableModel.Type,
                    MaterialContent = harvestableModel.MaterialContents.Select(materialContent => new MaterialModel
                    {
                        Name = materialContent.Material.Name,
                        Quantity = materialContent.Quantity,
                        Quality = materialContent.Material.Quality
                    }).ToList()
                });
            }

            foreach (string oreType in MoonOreTypes)
            {
                List<MoonOreIskPerHour> moonOreIskPerHourPerType = moonOreIskPerHourCollection.Where(o => o.Type == oreType).ToList();

                List<List<MoonOreIskPerHour>> moonOreIskPerHourGrouped = new();

                foreach (MoonOreIskPerHour moonOreIskPerHourType in moonOreIskPerHourPerType)
                {
                    if (moonOreIskPerHourPerType.Any(type =>
                        type.Name.Contains(moonOreIskPerHourType.Name) && type.Name != moonOreIskPerHourType.Name))
                    {
                        List<MoonOreIskPerHour> groupedByType = moonOreIskPerHourPerType.Where(type => type.Name.Contains(moonOreIskPerHourType.Name)).ToList();
                        moonOreIskPerHourGrouped.Add(groupedByType);
                    }
                }

                if (moonOreIskPerHourGrouped.Any())
                {
                    foreach (List<MoonOreIskPerHour> moonOreGroup in moonOreIskPerHourGrouped)
                    {
                        // set the improved flag on the improved variants
                        moonOreGroup.Where(o => o != GetBasicMoonOreType(moonOreGroup)).ToList().ForEach(o => o.IsImprovedVariant = true);
                    }
                }
            }

            return moonOreIskPerHourCollection;
        }

        /// <summary>
        /// This methods takes in <see cref="IEnumerable{T}"/> and return the non-improved one, based on their material types.
        /// It assumes that all <see cref="MoonOreIskPerHour"/>s are the same type.
        /// </summary>
        /// <param name="moonOreIskPerHourPerType"></param>
        /// <returns>The non-improved type of <see cref="MoonOreIskPerHour"/>.</returns>
        private static MoonOreIskPerHour GetBasicMoonOreType(IEnumerable<MoonOreIskPerHour> moonOreIskPerHourPerType)
        {
            return moonOreIskPerHourPerType.Aggregate(FindMoonOreWithLowestMaterialContent);
        }

        private static MoonOreIskPerHour FindMoonOreWithLowestMaterialContent(MoonOreIskPerHour moonOreToCompare, MoonOreIskPerHour moonOreToCompareTo)
        {
            foreach (MaterialModel materialToCompare in moonOreToCompare.MaterialContent)
            {
                MaterialModel materialToCompareTo = moonOreToCompareTo.MaterialContent.Single(material => material.Name == materialToCompare.Name);

                return materialToCompare.Quantity < materialToCompareTo.Quantity ? moonOreToCompare : moonOreToCompareTo;
            }

            return null;
        }
    }
}
