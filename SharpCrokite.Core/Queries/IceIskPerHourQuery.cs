using SharpCrokite.Core.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.Queries
{
    public class IceIskPerHourQuery : IskPerHourQuery<IceIskPerHour>
    {
        private static class IceType
        {
            public const string Ice = "Ice";
        }

        public IceIskPerHourQuery(HarvestableRepository harvestableRepository) : base(harvestableRepository)
        {
            HarvestableTypes = new [] {
                IceType.Ice
            };
        }
    }
}
