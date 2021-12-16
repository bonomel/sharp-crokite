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

        private static readonly string[] MoonOreTypes = new[]
        {
            "Ubiquitous Moon Asteroids", "Common Moon Asteroids", "Uncommon Moon Asteroids",
            "Rare Moon Asteroids", "Exceptional Moon Asteroids"
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

            // create the wrapped objects based on model data
            foreach (Harvestable harvestableModel in harvestableModels)
            {
                moonOreIskPerHourCollection.Add(new MoonOreIskPerHour
                {
                    Id = harvestableModel.HarvestableId,
                    // CompressedVariantTypeId = harvestableRepository.Find(h => h.IsCompressedVariantOfType == harvestableModel.HarvestableId).Single().HarvestableId,
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Volume = new Volume(harvestableModel.Volume),
                    Type = harvestableModel.Type,
                    Materials = harvestableModel.MaterialContents.ToDictionary(mc => mc.Material.Name, mc => mc.Quantity)
                });
            }

            foreach (string oreType in MoonOreTypes)
            {
                IEnumerable<MoonOreIskPerHour> moonOreIskPerHourPerType = moonOreIskPerHourCollection.Where(o => o.Type == oreType);

                IEnumerable<IGrouping<string, MoonOreIskPerHour>> groupedMoonOreIskPerHourPerType =
                    moonOreIskPerHourPerType.GroupBy(moonOre => moonOre.Type);

                foreach(IGrouping<string, MoonOreIskPerHour> moonOreIskPerHourPerTypeGroup in groupedMoonOreIskPerHourPerType)
                {
                    
                }

                //if (moonOreIskPerHourType.Any())
                //{
                //    // set the improved flag on the improved variants
                //    moonOreIskPerHourType.Where(o => o != GetOreTypeWithLowestAmountOfMinerals(moonOreIskPerHourType)).ToList().ForEach(o => o.IsImprovedVariant = true);
                //}
            }

            // order the ores by type
            moonOreIskPerHourCollection = moonOreIskPerHourCollection.OrderBy(o => o.Type).ToList();

            return moonOreIskPerHourCollection;
        }

        //private static MoonOreIskPerHour GetOreTypeWithHighestAmountOfMinerals(IEnumerable<AsteroidIskPerHour> moonOreIskPerHourPerType)
        //{
        //    return moonOreIskPerHourPerType.Aggregate((o1, o2) =>
        //        o1.Tritanium > o2.Tritanium ||
        //        o1.Pyerite > o2.Pyerite ||
        //        o1.Mexallon > o2.Mexallon ||
        //        o1.Isogen > o2.Isogen ||
        //        o1.Nocxium > o2.Nocxium ||
        //        o1.Zydrine > o2.Zydrine ||
        //        o1.Megacyte > o2.Megacyte
        //        ? o1 : o2);
        //}

        private static MoonOreIskPerHour GetOreTypeWithLowestAmountOfMinerals(IEnumerable<MoonOreIskPerHour> moonOreIskPerHourPerType)
        {
            return moonOreIskPerHourPerType.Last(); // or whatever
        }
    }
}
