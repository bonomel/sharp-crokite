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

            foreach (Harvestable harvestableModel in harvestableModels)
            {
                moonOreIskPerHourCollection.Add(new MoonOreIskPerHour
                {
                    Id = harvestableModel.HarvestableId,
                    //CompressedVariantTypeId = harvestableRepository.Find(h => h.IsCompressedVariantOfType == harvestableModel.HarvestableId).Single().HarvestableId,
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Volume = new Volume(harvestableModel.Volume),
                    Type = harvestableModel.Type,
                    MaterialContent = harvestableModel.MaterialContents.Select(materialContent => new MaterialModel()
                    {
                        Name = materialContent.Material.Name,
                        Type = materialContent.Material.Type,
                        MaterialId = materialContent.Material.MaterialId,
                        Quantity = materialContent.Quantity
                    }).ToList()
                });
            }

            foreach (string oreType in MoonOreTypes)
            {
                List<MoonOreIskPerHour> moonOreIskPerHourPerType = moonOreIskPerHourCollection.Where(o => o.Type == oreType).ToList();

                List<List<MoonOreIskPerHour>> moonOreIskPerHourGrouped = new();

                foreach (MoonOreIskPerHour moonOreIskPerHourType in moonOreIskPerHourPerType)
                {
                    if (moonOreIskPerHourPerType.Any(type =>
                        type.Name.Contains(moonOreIskPerHourType.Name) && type.Name != moonOreIskPerHourType.Name))
                    {
                        List<MoonOreIskPerHour> groupedByType = moonOreIskPerHourPerType.Where(type => type.Name.Contains(moonOreIskPerHourType.Name)).ToList();
                        moonOreIskPerHourGrouped.Add(groupedByType);
                    }
                }

                if (moonOreIskPerHourGrouped.Any())
                {
                    foreach (List<MoonOreIskPerHour> moonOreGroup in moonOreIskPerHourGrouped)
                    {
                        // set the improved flag on the improved variants
                        moonOreGroup.Where(o => o != GetOreTypeWithLowestAmountOfMinerals(moonOreGroup)).ToList().ForEach(o => o.IsImprovedVariant = true);
                    }
                }
            }

            return moonOreIskPerHourCollection;
        }

        private static MoonOreIskPerHour GetOreTypeWithLowestAmountOfMinerals(IEnumerable<MoonOreIskPerHour> moonOreIskPerHourPerType)
        {
            return moonOreIskPerHourPerType.Aggregate((o1, o2) =>
                o1.MaterialContent.First().Quantity < o2.MaterialContent.First().Quantity
                ? o1 : o2);
        }
    }
}
