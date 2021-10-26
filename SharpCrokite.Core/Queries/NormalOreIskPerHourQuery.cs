using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCrokite.Core.Queries
{
    public class NormalOreIskPerHourQuery
    {
        private const string MineralTypeString = "Mineral";

        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        private int systemToUseForPrices = 30000142; // Hard-coded Jita systemid - this will become a setting

        private decimal defaultYieldPerSecond = 50.00m;
        private decimal defaultReprocessingEfficiency = 0.782m;

        private static readonly string[] NormalOreTypes = new[]
        {
            "Veldspar", "Scordite", "Pyroxeres", "Plagioclase", "Kernite", "Omber", "Jaspet",
            "Dark Ochre", "Hemorphite", "Hedbergite", "Spodumain" ,"Crokite", "Bistot", "Arkonor"
        };

        public NormalOreIskPerHourQuery(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        internal IEnumerable<NormalOreIskPerHour> Execute()
        {
            List<NormalOreIskPerHour> normalOreIskPerHourCollection = new();

            IEnumerable<Harvestable> harvestableModels =
                harvestableRepository.Find(h => NormalOreTypes.Contains(h.Type) && h.IsCompressedVariantOfType == null);

            // create the wrapped objects based on model data
            foreach (Harvestable harvestableModel in harvestableModels)
            {
                normalOreIskPerHourCollection.Add(new()
                {
                    Id = harvestableModel.HarvestableId,
                    CompressedVariantTypeId = harvestableRepository.Find(h => h.IsCompressedVariantOfType == harvestableModel.HarvestableId).Single().HarvestableId,
                    Icon = harvestableModel.Icon,
                    Name = harvestableModel.Name,
                    Description = harvestableModel.Description,
                    Volume = new Volume(harvestableModel.Volume),
                    Type = harvestableModel.Type,
                    Minerals = harvestableModel.MaterialContents.ToDictionary(mc => mc.Material.Name, mc => mc.Quantity)
                });
            }

            foreach (string oreType in NormalOreTypes)
            {
                IEnumerable<NormalOreIskPerHour> normalOreIskPerHourPerType = normalOreIskPerHourCollection.Where(o => o.Type == oreType);

                if (normalOreIskPerHourPerType.Any())
                {
                    // first remove the redunant improved variant
                    _ = normalOreIskPerHourCollection.Remove(GetOreTypeWithHighestAmountOfMinerals(normalOreIskPerHourPerType));

                    // and then set the improved flag on the improved variants
                    normalOreIskPerHourPerType.Where(o => o != GetOreTypeWithLowestAmountOfMinerals(normalOreIskPerHourPerType)).ToList().ForEach(o => o.IsImprovedVariant = true);
                }
            }

            // get all types of minerals
            IEnumerable<Material> mineralModels = materialRepository.Find(m => m.Type == MineralTypeString);

            // order the ores by type
            normalOreIskPerHourCollection = normalOreIskPerHourCollection.OrderBy(o => o.Type).ToList();

            foreach(NormalOreIskPerHour normalOreIskPerHour in normalOreIskPerHourCollection)
            {
                CalculateMaterialIskPerHour(normalOreIskPerHour, mineralModels);
                CalculateCompressedIskPerHour(normalOreIskPerHour);
            }

            return normalOreIskPerHourCollection;
        }

        private void CalculateMaterialIskPerHour(NormalOreIskPerHour normalOreIskPerHour, IEnumerable<Material> minerals)
        {
            IEnumerable<KeyValuePair<string, int>> notEmptyMinerals = normalOreIskPerHour.Minerals.Where(m => m.Value != 0);

            decimal batchValueAfterReprocessing = new();

            foreach (KeyValuePair<string, int> mineral in notEmptyMinerals)
            {
                int mineralsAfterReprocessing = Convert.ToInt32(Math.Floor(mineral.Value * defaultReprocessingEfficiency));

                decimal currentMarketPrice = 0;

                if (minerals.Single(m => m.Name == mineral.Key).Prices.Any())
                {
                    currentMarketPrice = minerals.Single(m => m.Name == mineral.Key).Prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices) != null
                    ? minerals.Single(m => m.Name == mineral.Key).Prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices).SellPercentile
                    : 0;
                }

                batchValueAfterReprocessing += mineralsAfterReprocessing * currentMarketPrice;
            }

            decimal valuePerUnit = batchValueAfterReprocessing / 100; // batch size
            decimal valuePerSquareMeters = valuePerUnit / normalOreIskPerHour.Volume.Amount;
            decimal valuePerSecond = valuePerSquareMeters * defaultYieldPerSecond;
            decimal valuePerHour = valuePerSecond * 60 * 60; // 3600 seconds = 1 hour

            normalOreIskPerHour.MaterialIskPerHour = new Isk(valuePerHour);
        }

        private void CalculateCompressedIskPerHour(NormalOreIskPerHour normalOreIskPerHour)
        {
            decimal yieldPerSecondDividedByVolume = defaultYieldPerSecond / normalOreIskPerHour.Volume.Amount;
            decimal batchSizeCompensatedVolume = yieldPerSecondDividedByVolume / 100; //batch size

            Harvestable compressedVariant = harvestableRepository.Find(h => h.HarvestableId == normalOreIskPerHour.CompressedVariantTypeId).SingleOrDefault();

            decimal unitMarketPrice = 0;

            if (compressedVariant != null)
            {
                unitMarketPrice = compressedVariant.Prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices) != null
                    ? compressedVariant.Prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices).SellPercentile
                    : 0;
            }

            decimal normalizedCompressedBatchValue = unitMarketPrice * batchSizeCompensatedVolume;
            decimal compressedValuePerHour = normalizedCompressedBatchValue * 60 * 60;

            normalOreIskPerHour.CompressedIskPerHour = new Isk(compressedValuePerHour);
        }

        private static NormalOreIskPerHour GetOreTypeWithHighestAmountOfMinerals(IEnumerable<NormalOreIskPerHour> normalOreIskPerHourPerType)
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

        private static NormalOreIskPerHour GetOreTypeWithLowestAmountOfMinerals(IEnumerable<NormalOreIskPerHour> normalOreIskPerHourPerType)
        {
            return normalOreIskPerHourPerType.Aggregate((o1, o2) =>
                o1.Tritanium < o2.Tritanium ||
                o1.Pyerite < o2.Pyerite ||
                o1.Mexallon < o2.Mexallon ||
                o1.Isogen < o2.Isogen ||
                o1.Nocxium < o2.Nocxium ||
                o1.Zydrine < o2.Zydrine ||
                o1.Megacyte < o2.Megacyte
                ? o1 : o2);
        }
    }
}
