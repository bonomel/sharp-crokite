using System.Collections.Generic;
using System.Linq;
using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class MoonOreHarvestableIskPerHourQuery : HarvestableIskPerHourQuery<MoonOreIskPerHour>
    {
        private static class MoonOreType
        {
            public const string UbiquitousMoonAsteroids = "Ubiquitous Moon Asteroids";
            public const string CommonMoonAsteroids = "Common Moon Asteroids";
            public const string UncommonMoonAsteroids = "Uncommon Moon Asteroids";
            public const string RareMoonAsteroids = "Rare Moon Asteroids";
            public const string ExceptionalMoonAsteroids = "Exceptional Moon Asteroids";
        }

        public MoonOreHarvestableIskPerHourQuery(HarvestableRepository harvestableRepository) : base(harvestableRepository)
        {
            HarvestableTypes = new[] {
                MoonOreType.UbiquitousMoonAsteroids,
                MoonOreType.CommonMoonAsteroids,
                MoonOreType.UncommonMoonAsteroids,
                MoonOreType.RareMoonAsteroids,
                MoonOreType.ExceptionalMoonAsteroids
            };
        }

        internal override IEnumerable<MoonOreIskPerHour> Execute()
        {
            IEnumerable<MoonOreIskPerHour> harvestableIskPerHourResult = base.Execute().ToList();

            foreach (MoonOreIskPerHour moonOreIskPerHour in harvestableIskPerHourResult)
            {
                moonOreIskPerHour.CompressedVariantTypeId = FindCompressedVariantTypeId(moonOreIskPerHour);
            }

            return harvestableIskPerHourResult;
        }

        private int FindCompressedVariantTypeId(HarvestableIskPerHour harvestableIskPerHour)
        {
            Harvestable compressedVariant = HarvestableRepository.Find(h => h.IsCompressedVariantOfType == harvestableIskPerHour.HarvestableId).First();

            return compressedVariant.HarvestableId;
        }
    }
}
