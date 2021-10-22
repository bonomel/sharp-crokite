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
        private const string MineralTypeString = "Mineral";

        private HarvestableRepository harvestableRepository;
        private MaterialRepository materialRepository;

        private int systemToUseForPrices = 30000142; // Hard-coded Jita systemid - this will become a setting

        private decimal defaultYieldPerSecond = 50.00m;
        private decimal defaultReprocessingEfficiency = 0.782m;

        private static readonly string[] NormalOreTypes = new[]
        {
            "Veldspar", "Scordite", "Pyroxeres", "Plagioclase", "Kernite", "Omber", "Jaspet", "Dark Ochre", "Hemorphite", "Hedbergite", "Spodumain" ,"Crokite", "Bistot", "Arkonor"
        };

        public NormalOreIskPerHourQuery(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        internal IEnumerable<NormalOreIskPerHour> Execute()
        {
            List<NormalOreIskPerHour> normalOreIskPerHourCollection = new();

            List<Material> minerals = materialRepository.Find(m => m.Type == MineralTypeString).ToList();

            IEnumerable<Harvestable> harvestableModels = harvestableRepository.Find(h =>
            NormalOreTypes.Contains(h.Type) &&
            h.IsCompressedVariantOfType == null);

            foreach(Harvestable harvestableModel in harvestableModels)
            {
                normalOreIskPerHourCollection.Add(new()
                {
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Volume = harvestableModel.Volume,
                    Type = harvestableModel.Type,
                    Minerals = harvestableModel.MaterialContents.ToDictionary(mc => mc.Material.Name, mc => mc.Quantity)
                });
            }

            foreach(string oreType in NormalOreTypes)
            {
                List<NormalOreIskPerHour> normalOreIskPerHourPerType = normalOreIskPerHourCollection.Where(o => o.Type == oreType).ToList();

                _ = normalOreIskPerHourCollection.Remove(GetObsoleteTypeFromListOfOreTypes(normalOreIskPerHourPerType));
            }

            foreach(NormalOreIskPerHour normalOreIskPerHour in normalOreIskPerHourCollection)
            {
                CalculateBatchValue(normalOreIskPerHour, minerals);
            }

            normalOreIskPerHourCollection = normalOreIskPerHourCollection.OrderBy(o => o.Type).ToList();

            foreach(NormalOreIskPerHour normalOreIskPerHour in normalOreIskPerHourCollection)
            {
                // TODO add calculation:
                //
                // foreach mineral that is not 0, multiply by reprocessing efficiency, round down
                // multiply by price to get value per mineral type
                // add up all results, this is batch value
                // divide by 100 to get value per unit
                // divide unit by volume to get value per m3
                // multiply by yield to get value per second
                // multiply by 60 twice to get value per hour
            }

            return normalOreIskPerHourCollection;
        }

        // This method will return the type with the highest amount of mineral content
        // This is necessary because CCP left obsolete types in the database, but without a proper flag
        private NormalOreIskPerHour GetObsoleteTypeFromListOfOreTypes(List<NormalOreIskPerHour> normalOreIskPerHourPerType)
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

        private void CalculateBatchValue(NormalOreIskPerHour normalOreIskPerHour, List<Material> minerals)
        {
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Tritanium * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Tritanium)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Pyerite * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Pyerite)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Mexallon * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Mexallon)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Isogen * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Isogen)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Nocxium * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Nocxium)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Zydrine * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Zydrine)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
            normalOreIskPerHour.BatchValue += normalOreIskPerHour.Megacyte * minerals.Single(m => m.Name == nameof(normalOreIskPerHour.Megacyte)).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;
        }

        private static int GetMaterialQuantity(Harvestable harvestableModel, string materialName)
        {
            return harvestableModel.MaterialContents.SingleOrDefault(mc => mc.Material.Name == materialName) != null
                ? harvestableModel.MaterialContents.SingleOrDefault(mc => mc.Material.Name == materialName).Quantity
                : 0;
        }
    }
}
