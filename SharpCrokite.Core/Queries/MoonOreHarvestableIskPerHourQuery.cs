using SharpCrokite.Core.Models;
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
            HarvestableTypes = new [] {
                MoonOreType.UbiquitousMoonAsteroids,
                MoonOreType.CommonMoonAsteroids,
                MoonOreType.UncommonMoonAsteroids,
                MoonOreType.RareMoonAsteroids,
                MoonOreType.ExceptionalMoonAsteroids
            };
        }
    }
}
