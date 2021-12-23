using System.Collections.Generic;
using System.Linq;
using SharpCrokite.Core.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class AsteroidIskPerHourQuery : IskPerHourQuery<AsteroidIskPerHour>
    {
        private static class AsteroidType
        {
            public const string Veldspar = "Veldspar";
            public const string Scordite = "Scordite";
            public const string Pyroxeres = "Pyroxeres";
            public const string Plagioclase = "Plagioclase";
            public const string Kernite = "Kernite";
            public const string Omber = "Omber";
            public const string Jaspet = "Jaspet";
            public const string DarkOchre = "Dark Ochre";
            public const string Hemorphite = "Hemorphite";
            public const string Hedbergite = "Hedbergite";
            public const string Spodumain = "Spodumain";
            public const string Crokite = "Crokite";
            public const string Bistot = "Bistot";
            public const string Arkonor = "Arkonor";
        }

        public AsteroidIskPerHourQuery(HarvestableRepository harvestableRepository) : base(harvestableRepository)
        {
            HarvestableTypes = new[] {
                AsteroidType.Veldspar,
                AsteroidType.Scordite,
                AsteroidType.Pyroxeres,
                AsteroidType.Plagioclase,
                AsteroidType.Kernite,
                AsteroidType.Omber,
                AsteroidType.Jaspet,
                AsteroidType.DarkOchre,
                AsteroidType.Hemorphite,
                AsteroidType.Hedbergite,
                AsteroidType.Spodumain,
                AsteroidType.Crokite,
                AsteroidType.Bistot,
                AsteroidType.Arkonor
            };
        }

        protected override void SanitizeHarvestableCollection(List<AsteroidIskPerHour> harvestableIskPerHourCollection, string oreType)
        {
            IEnumerable<AsteroidIskPerHour> normalOreIskPerHourPerType = harvestableIskPerHourCollection.Where(o => o.Type == oreType);

            List<AsteroidIskPerHour> oreIskPerHourPerType = normalOreIskPerHourPerType.ToList();

            if (oreIskPerHourPerType.Any())
            {
                // first remove the redunant improved variant
                _ = harvestableIskPerHourCollection.Remove(GetOreTypeWithHighestAmountOfMinerals(oreIskPerHourPerType));

                // and then set the improved flag on the improved variants
                oreIskPerHourPerType.Where(o => o != GetBasicOreType(oreIskPerHourPerType)).ToList().ForEach(o => o.IsImprovedVariant = true);
            }
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
    }
}
