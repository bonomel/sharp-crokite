using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class AsteroidHarvestableIskPerHourQuery : HarvestableIskPerHourQuery<AsteroidIskPerHour>
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
            public const string Gneiss = "Gneiss";
            public const string DarkOchre = "Dark Ochre";
            public const string Hemorphite = "Hemorphite";
            public const string Hedbergite = "Hedbergite";
            public const string Spodumain = "Spodumain";
            public const string Crokite = "Crokite";
            public const string Bistot = "Bistot";
            public const string Arkonor = "Arkonor";
        }

        public AsteroidHarvestableIskPerHourQuery(HarvestableRepository harvestableRepository) : base(harvestableRepository)
        {
            HarvestableTypes = new[] {
                AsteroidType.Veldspar,
                AsteroidType.Scordite,
                AsteroidType.Pyroxeres,
                AsteroidType.Plagioclase,
                AsteroidType.Kernite,
                AsteroidType.Omber,
                AsteroidType.Jaspet,
                AsteroidType.Gneiss,
                AsteroidType.DarkOchre,
                AsteroidType.Hemorphite,
                AsteroidType.Hedbergite,
                AsteroidType.Spodumain,
                AsteroidType.Crokite,
                AsteroidType.Bistot,
                AsteroidType.Arkonor
            };
        }

        internal override IEnumerable<AsteroidIskPerHour> Execute()
        {
            IEnumerable<AsteroidIskPerHour> harvestableIskPerHourResult = base.Execute().ToList();

            foreach (AsteroidIskPerHour asteroidIskPerHour in harvestableIskPerHourResult)
            {
                asteroidIskPerHour.CompressedVariantTypeId = FindCompressedVariantTypeId(asteroidIskPerHour);
            }

            return harvestableIskPerHourResult;
        }

        private int FindCompressedVariantTypeId(HarvestableIskPerHour asteroidIskPerHour)
        {
            Harvestable compressedVariant = HarvestableRepository.Find(h => h.IsCompressedVariantOfType == asteroidIskPerHour.HarvestableId && !h.Name.StartsWith("Batch")).First();

            return compressedVariant.HarvestableId;
        }

        protected override void SanitizeHarvestableCollection(List<AsteroidIskPerHour> harvestableIskPerHourCollection, string oreType)
        {
            IEnumerable<AsteroidIskPerHour> normalOreIskPerHourPerType = harvestableIskPerHourCollection.Where(o => o.Type == oreType);

            List<AsteroidIskPerHour> oreIskPerHourPerType = normalOreIskPerHourPerType.ToList();

            if (oreIskPerHourPerType.Any())
            {
                // first remove the redunant improved variant
                var bestVariant = GetBestAsteroidType(oreIskPerHourPerType);
                _ = harvestableIskPerHourCollection.Remove(bestVariant);

                // and then set the improved flag on the improved variants
                var basicOreType = GetBasicHarvestableType(oreIskPerHourPerType);
                oreIskPerHourPerType.Where(o => o != basicOreType).ToList().ForEach(o => o.IsImprovedVariant = true);
            }
        }

        private static AsteroidIskPerHour GetBestAsteroidType(IEnumerable<AsteroidIskPerHour> harvestableTypeGroup)
        {
            return harvestableTypeGroup.Aggregate(FindAsteroidWithHighestMaterialContent);
        }

        private static AsteroidIskPerHour FindAsteroidWithHighestMaterialContent(AsteroidIskPerHour harvestableToCompare, AsteroidIskPerHour harvestableToCompareTo)
        {
            foreach (MaterialModel materialToCompare in harvestableToCompare.MaterialContent)
            {
                MaterialModel materialToCompareTo = harvestableToCompareTo.MaterialContent.Single(material => material.Name == materialToCompare.Name);

                if (materialToCompare.Quantity > materialToCompareTo.Quantity)
                {
                    return harvestableToCompare;
                }
            }

            return harvestableToCompareTo;
        }
    }
}
