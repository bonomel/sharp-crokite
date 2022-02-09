using System.Collections.Generic;
using System.Linq;
using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
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
            HarvestableTypes = new[] {
                IceType.Ice
            };
        }

        internal override IEnumerable<IceIskPerHour> Execute()
        {
            IEnumerable<IceIskPerHour> harvestableIskPerHourResult = base.Execute().ToList();

            foreach (IceIskPerHour iceIskPerHour in harvestableIskPerHourResult)
            {
                iceIskPerHour.CompressedVariantTypeId = FindCompressedVariantTypeId(iceIskPerHour);
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
