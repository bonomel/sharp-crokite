using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace SharpCrokite.Core.ViewModels
{
    public class NormalOreIskPerHourViewModel : INotifyPropertyChanged
    {
        private readonly CultureInfo ci = new("en-us");

        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        private const string MineralTypeString = "Mineral";
        private int systemToUseForPrices = 30000142; // Hard-coded Jita systemid - this will become a setting
        private decimal defaultYieldPerSecond = 50.00m;
        private decimal defaultReprocessingEfficiency = 0.782m;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ObservableCollection<NormalOreIskPerHour> normalOreIskPerHourCollection = new();
        public ObservableCollection<NormalOreIskPerHour> NormalOreIskPerHourCollection
        {
            get => normalOreIskPerHourCollection;
            private set
            {
                if (Equals(value, normalOreIskPerHourCollection)) { return; }
                normalOreIskPerHourCollection = value;
                SetVisibilityForImprovedVariants();
                NotifyPropertyChanged(nameof(NormalOreIskPerHourCollection));
            }
        }

        private bool showImprovedVariantsIsChecked = true;
        public bool ShowImprovedVariantsIsChecked
        {
            get => showImprovedVariantsIsChecked;
            set
            {
                showImprovedVariantsIsChecked = value;
                SetVisibilityForImprovedVariants();
            }
        }

        private decimal yieldPerSecond = 50m;
        private IEnumerable<Material> mineralModels;

        public string YieldPerSecondText
        {
            get => yieldPerSecond.ToString("F02", ci);
            set
            {
                if (yieldPerSecond.ToString("F02", ci) != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        yieldPerSecond = 0;
                        NotifyPropertyChanged(nameof(YieldPerSecondText));
                        UpdateIskPerHourForYield();
                    }
                    else if (decimal.TryParse(value, NumberStyles.Float, ci, out decimal result))
                    {
                        if(yieldPerSecond != Math.Round(result, 2))
                        {
                            yieldPerSecond = result;
                            NotifyPropertyChanged(nameof(YieldPerSecondText));
                            UpdateIskPerHourForYield();
                        }
                    }
                }
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public NormalOreIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            normalOreIskPerHourCollection = LoadStaticDataWithPrices();
        }

        internal void UpdateNormalOreIskPerHour()
        {
            NormalOreIskPerHourCollection = LoadStaticDataWithPrices();
        }

        internal void UpdateIskPerHour() => UpdateIskPerHourWithUpdatedPrices(NormalOreIskPerHourCollection);

        private void UpdateIskPerHourWithUpdatedPrices(IEnumerable<NormalOreIskPerHour> normalOreIskPerHourCollection)
        {
            UpdatePrices();
            foreach (NormalOreIskPerHour normalOreIskPerHour in normalOreIskPerHourCollection)
            {
                CalculateMaterialIskPerHour(normalOreIskPerHour);
                CalculateCompressedIskPerHour(normalOreIskPerHour);
            }
        }

        private void UpdatePrices()
        {
            mineralModels = materialRepository.Find(m => m.Type == MineralTypeString);
        }

        private void UpdateIskPerHourForYield()
        {
            foreach (NormalOreIskPerHour normalOreIskPerHour in NormalOreIskPerHourCollection)
            {
                CalculateMaterialIskPerHour(normalOreIskPerHour);
                CalculateCompressedIskPerHour(normalOreIskPerHour);
            }
        }

        private void CalculateMaterialIskPerHour(NormalOreIskPerHour normalOreIskPerHour)
        {
            IEnumerable<KeyValuePair<string, int>> notEmptyMinerals = normalOreIskPerHour.Minerals.Where(m => m.Value != 0);

            decimal batchValueAfterReprocessing = new();

            foreach (KeyValuePair<string, int> mineral in notEmptyMinerals)
            {
                int mineralsAfterReprocessing = Convert.ToInt32(Math.Floor(mineral.Value * defaultReprocessingEfficiency));

                decimal currentMarketPrice = 0;

                if (mineralModels.Single(m => m.Name == mineral.Key).Prices.Any())
                {
                    currentMarketPrice = mineralModels.Single(m => m.Name == mineral.Key).Prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices) != null
                    ? mineralModels.Single(m => m.Name == mineral.Key).Prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices).SellPercentile
                    : 0;
                }

                batchValueAfterReprocessing += mineralsAfterReprocessing * currentMarketPrice;
            }

            decimal valuePerUnit = batchValueAfterReprocessing / 100; // batch size
            decimal valuePerSquareMeters = valuePerUnit / normalOreIskPerHour.Volume.Amount;
            decimal valuePerSecond = valuePerSquareMeters * yieldPerSecond;
            decimal valuePerHour = valuePerSecond * 60 * 60; // 3600 seconds = 1 hour

            normalOreIskPerHour.MaterialIskPerHour = new Isk(valuePerHour);
        }

        private void CalculateCompressedIskPerHour(NormalOreIskPerHour normalOreIskPerHour)
        {
            decimal yieldPerSecondDividedByVolume = yieldPerSecond / normalOreIskPerHour.Volume.Amount;
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

        private void SetVisibilityForImprovedVariants()
        {
            normalOreIskPerHourCollection.Where(o => o.IsImprovedVariant).ToList().ForEach(o => o.Visible = showImprovedVariantsIsChecked);
        }

        private ObservableCollection<NormalOreIskPerHour> LoadStaticDataWithPrices()
        {
            NormalOreIskPerHourQuery normalOreIskPerHourQuery = new(harvestableRepository);
            IEnumerable<NormalOreIskPerHour> normalIskPerHourCollection = normalOreIskPerHourQuery.Execute();
            UpdateIskPerHourWithUpdatedPrices(normalIskPerHourCollection);

            return new(normalIskPerHourCollection);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
