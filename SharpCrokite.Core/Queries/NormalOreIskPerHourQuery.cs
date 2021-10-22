using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCrokite.Core.Queries
{
    public class NormalOreIskPerHourQuery
    {
        private HarvestableRepository harvestableRepository;
        private MaterialRepository materialRepository;

        public NormalOreIskPerHourQuery(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        internal IEnumerable<NormalOreIskPerHour> Execute()
        {
            List<NormalOreIskPerHour> normalOreIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels = harvestableRepository.Find(h =>
            (h.Type == "Veldspar" ||
            h.Type == "Scordite" ||
            h.Type == "Pyroxeres" ||
            h.Type == "Plagioclase") &&
            h.IsCompressedVariantOfType == null);

            foreach(Harvestable harvestableModel in harvestableModels)
            {
                normalOreIskPerHourCollection.Add(new()
                {
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Tritanium = GetMaterialQuantity(harvestableModel, "Tritanium"),
                    Pyerite = GetMaterialQuantity(harvestableModel, "Pyerite"),
                    Isogen = GetMaterialQuantity(harvestableModel, "Isogen"),
                    Megacyte = GetMaterialQuantity(harvestableModel, "Megacyte"),
                    Mexallon = GetMaterialQuantity(harvestableModel, "Mexallon"),
                    Nocxium = GetMaterialQuantity(harvestableModel, "Nocxium"),
                    Zydrine = GetMaterialQuantity(harvestableModel, "Zydrine"),
                    MaterialIskPerHour = "todo",
                    CompressedIskPerHour = "todo"
                });
            }

            return normalOreIskPerHourCollection;
        }

        private static int GetMaterialQuantity(Harvestable harvestableModel, string materialName)
        {
            return harvestableModel.MaterialContents.SingleOrDefault(mc => mc.Material.Name == materialName) != null
                ? harvestableModel.MaterialContents.SingleOrDefault(mc => mc.Material.Name == materialName).Quantity
                : 0;
        }
    }
}
