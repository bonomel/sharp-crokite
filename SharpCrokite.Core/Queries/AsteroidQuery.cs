using System.Collections.Generic;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class AsteroidQuery
    {
        private readonly HarvestableRepository harvestableRepository;

        private static readonly string[] NormalOreTypes = {
            "Veldspar", "Scordite", "Pyroxeres", "Plagioclase", "Kernite", "Omber", "Jaspet",
            "Dark Ochre", "Hemorphite", "Hedbergite", "Spodumain" ,"Crokite", "Bistot", "Arkonor"
        };

        public AsteroidQuery(HarvestableRepository harvestableRepository)
        {
            this.harvestableRepository = harvestableRepository;
        }

        internal IEnumerable<AsteroidIskPerHour> Execute()
        {
            List<AsteroidIskPerHour> normalOreIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels =
                harvestableRepository.Find(h => NormalOreTypes.Contains(h.Type) && h.IsCompressedVariantOfType == null);

            // create the wrapped objects based on model data
            foreach (Harvestable harvestableModel in harvestableModels)
            {
                normalOreIskPerHourCollection.Add(new AsteroidIskPerHour
                {
                    Id = harvestableModel.HarvestableId,
                    CompressedVariantTypeId = harvestableRepository.Find(h => h.IsCompressedVariantOfType == harvestableModel.HarvestableId).Single().HarvestableId,
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Volume = new Volume(harvestableModel.Volume),
                    Type = harvestableModel.Type,
                    Minerals = harvestableModel.MaterialContents.ToDictionary(mc => mc.Material.Name, mc => mc.Quantity)
                });
            }

            foreach (string oreType in NormalOreTypes)
            {
                IEnumerable<AsteroidIskPerHour> normalOreIskPerHourPerType = normalOreIskPerHourCollection.Where(o => o.Type == oreType);

                List<AsteroidIskPerHour> oreIskPerHourPerType = normalOreIskPerHourPerType.ToList();

                if (oreIskPerHourPerType.Any())
                {
                    // first remove the redunant improved variant
                    _ = normalOreIskPerHourCollection.Remove(GetOreTypeWithHighestAmountOfMinerals(oreIskPerHourPerType));

                    // and then set the improved flag on the improved variants
                    oreIskPerHourPerType.Where(o => o != GetOreTypeWithLowestAmountOfMinerals(oreIskPerHourPerType)).ToList().ForEach(o => o.IsImprovedVariant = true);
                }
            }

            // order the ores by type
            normalOreIskPerHourCollection = normalOreIskPerHourCollection.OrderBy(o => o.Type).ToList();

            return normalOreIskPerHourCollection;
        }

        private static AsteroidIskPerHour GetOreTypeWithHighestAmountOfMinerals(IEnumerable<AsteroidIskPerHour> normalOreIskPerHourPerType)
        {
            return normalOreIskPerHourPerType.Aggregate((o1, o2) =>
                o1.Tritanium > o2.Tritanium ||
                o1.Pyerite > o2.Pyerite ||
                o1.Mexallon > o2.Mexallon ||
                o1.Isogen > o2.Isogen ||
                o1.Nocxium > o2.Nocxium ||
                o1.Zydrine > o2.Zydrine ||
                o1.Megacyte > o2.Megacyte
                ? o1 : o2);
        }

        private static AsteroidIskPerHour GetOreTypeWithLowestAmountOfMinerals(IEnumerable<AsteroidIskPerHour> normalOreIskPerHourPerType)
        {
            return normalOreIskPerHourPerType.Aggregate((o1, o2) =>
                o1.Tritanium < o2.Tritanium ||
                o1.Pyerite < o2.Pyerite ||
                o1.Mexallon < o2.Mexallon ||
                o1.Isogen < o2.Isogen ||
                o1.Nocxium < o2.Nocxium ||
                o1.Zydrine < o2.Zydrine ||
                o1.Megacyte < o2.Megacyte
                ? o1 : o2);
        }
    }
}
