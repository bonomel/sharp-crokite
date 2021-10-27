using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCrokite.Core.Queries
{
    public class NormalOreIskPerHourQuery
    {
        private readonly HarvestableRepository harvestableRepository;

        private static readonly string[] NormalOreTypes = new[]
        {
            "Veldspar", "Scordite", "Pyroxeres", "Plagioclase", "Kernite", "Omber", "Jaspet",
            "Dark Ochre", "Hemorphite", "Hedbergite", "Spodumain" ,"Crokite", "Bistot", "Arkonor"
        };

        public NormalOreIskPerHourQuery(HarvestableRepository harvestableRepository)
        {
            this.harvestableRepository = harvestableRepository;
        }

        internal IEnumerable<NormalOreIskPerHour> Execute()
        {
            List<NormalOreIskPerHour> normalOreIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels =
                harvestableRepository.Find(h => NormalOreTypes.Contains(h.Type) && h.IsCompressedVariantOfType == null);

            // create the wrapped objects based on model data
            foreach (Harvestable harvestableModel in harvestableModels)
            {
                normalOreIskPerHourCollection.Add(new()
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
                IEnumerable<NormalOreIskPerHour> normalOreIskPerHourPerType = normalOreIskPerHourCollection.Where(o => o.Type == oreType);

                if (normalOreIskPerHourPerType.Any())
                {
                    // first remove the redunant improved variant
                    _ = normalOreIskPerHourCollection.Remove(GetOreTypeWithHighestAmountOfMinerals(normalOreIskPerHourPerType));

                    // and then set the improved flag on the improved variants
                    normalOreIskPerHourPerType.Where(o => o != GetOreTypeWithLowestAmountOfMinerals(normalOreIskPerHourPerType)).ToList().ForEach(o => o.IsImprovedVariant = true);
                }
            }

            // order the ores by type
            normalOreIskPerHourCollection = normalOreIskPerHourCollection.OrderBy(o => o.Type).ToList();

            return normalOreIskPerHourCollection;
        }

        private static NormalOreIskPerHour GetOreTypeWithHighestAmountOfMinerals(IEnumerable<NormalOreIskPerHour> normalOreIskPerHourPerType)
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

        private static NormalOreIskPerHour GetOreTypeWithLowestAmountOfMinerals(IEnumerable<NormalOreIskPerHour> normalOreIskPerHourPerType)
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
