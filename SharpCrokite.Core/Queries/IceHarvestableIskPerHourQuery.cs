using SharpCrokite.Core.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class IceHarvestableIskPerHourQuery : HarvestableIskPerHourQuery<IceIskPerHour>
    {
        private static class IceType
        {
            public const string Ice = "Ice";
        }

        public IceHarvestableIskPerHourQuery(HarvestableRepository harvestableRepository) : base(harvestableRepository)
        {
            HarvestableTypes = new [] {
                IceType.Ice
            };
        }
    }
}
