using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            normalOreIskPerHourCollection = normalOreIskPerHourCollection.OrderBy(o => o.Type).ToList();

            foreach(NormalOreIskPerHour normalOreIskPerHour in normalOreIskPerHourCollection)
            {
                var notEmptyMinerals = normalOreIskPerHour.Minerals.Where(m => m.Value != 0);

                decimal batchValueAfterReprocessing = new();

                foreach(KeyValuePair<string, int> mineral in notEmptyMinerals)
                {
                    int mineralsAfterReprocessing = Convert.ToInt32(Math.Floor(mineral.Value * defaultReprocessingEfficiency));
                    decimal currentMarketPrice = minerals.Single(m => m.Name == mineral.Key).Prices.Single(p => p.SystemId == systemToUseForPrices).SellPercentile;

                    batchValueAfterReprocessing += mineralsAfterReprocessing * currentMarketPrice;
                }

                decimal valuePerUnit = batchValueAfterReprocessing / 100; // batchsize
                decimal valuePerSquareMeters = valuePerUnit / normalOreIskPerHour.Volume;

                decimal valuePerSecond = valuePerSquareMeters * defaultYieldPerSecond;

                decimal valuePerHour = valuePerSecond * 60 * 60; // 3600 seconds = 1 hour

                normalOreIskPerHour.MaterialIskPerHour = DisplayAsISK(valuePerHour);
            }

            return normalOreIskPerHourCollection;
        }

        private string DisplayAsISK(decimal decimalISK)
        {
            return decimalISK != 0 ? $"{decimalISK.ToString("C", ISKNumberFormatInfo)}" : "N/A";
        }

        private static readonly NumberFormatInfo ISKNumberFormatInfo = new()
        {
            CurrencyDecimalSeparator = ",",
            CurrencyDecimalDigits = 2,
            CurrencyGroupSeparator = ".",
            CurrencyGroupSizes = new int[] { 3 },
            CurrencySymbol = "ISK",
            CurrencyPositivePattern = 3,
            CurrencyNegativePattern = 8
        };

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

        private static int GetMaterialQuantity(Harvestable harvestableModel, string materialName)
        {
            return harvestableModel.MaterialContents.SingleOrDefault(mc => mc.Material.Name == materialName) != null
                ? harvestableModel.MaterialContents.SingleOrDefault(mc => mc.Material.Name == materialName).Quantity
                : 0;
        }
    }
}
