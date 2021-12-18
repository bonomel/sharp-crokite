using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MoonOreIskPerHourViewModel : INotifyPropertyChanged
    {
        private const string MineralTypeString = "Mineral";
        private const int BatchSize = 100;

        private readonly CultureInfo ci = new("en-us");
        private readonly int systemToUseForPrices = 30000142; // Hard-coded Jita systemid - this will become a setting eventually

        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ObservableCollection<MoonOreIskPerHour> moonOreIskPerHourCollection = new();
        public ObservableCollection<MoonOreIskPerHour> MoonOreIskPerHourCollection
        {
            get => moonOreIskPerHourCollection;
            private set
            {
                if (Equals(value, moonOreIskPerHourCollection)) { return; }
                moonOreIskPerHourCollection = value;
                SetVisibilityForImprovedVariants();
                NotifyPropertyChanged(nameof(MoonOreIskPerHourCollection));
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
                        UpdateMaterialIskPerHour();
                        //UpdateCompressedIskPerHour();
                        NotifyPropertyChanged(nameof(YieldPerSecondText));
                    }
                    else if (decimal.TryParse(value, NumberStyles.Float, ci, out decimal result))
                    {
                        if(yieldPerSecond != Math.Round(result, 2))
                        {
                            yieldPerSecond = result;
                            UpdateMaterialIskPerHour();
                            //UpdateCompressedIskPerHour();
                            NotifyPropertyChanged(nameof(YieldPerSecondText));
                        }
                    }
                }
            }
        }

        private decimal reprocessingEfficiency = 0.782m;
        public string ReprocessingEfficiencyText
        {
            get => (reprocessingEfficiency * 100).ToString("F02", ci);
            set
            {
                if ((reprocessingEfficiency * 100).ToString("F02", ci) != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        reprocessingEfficiency = 0;
                        UpdateMaterialIskPerHour();
                        NotifyPropertyChanged(nameof(ReprocessingEfficiencyText));
                    }
                    else if (decimal.TryParse(value, NumberStyles.Float, ci, out decimal result))
                    {
                        if (result <= 100)
                        {
                            result /= 100;
                            if (reprocessingEfficiency != Math.Round(result, 4))
                            {
                                reprocessingEfficiency = result;
                                UpdateMaterialIskPerHour();
                                NotifyPropertyChanged(nameof(ReprocessingEfficiencyText));
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<Material> materialModels;

        public Guid Id { get; } = Guid.NewGuid();

        public MoonOreIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            moonOreIskPerHourCollection = LoadStaticData();

            if(moonOreIskPerHourCollection.Any())
            {
                UpdateMineralPrices();
                UpdateCompressedVariantPrices();
            }

            UpdateMaterialIskPerHour();
            //UpdateCompressedIskPerHour();
        }

        internal void ReloadStaticData()
        {
            MoonOreIskPerHourCollection = LoadStaticData();
        }

        internal void UpdatePrices()
        {
            UpdateMineralPrices();
            UpdateCompressedVariantPrices();

            UpdateMaterialIskPerHour();
            //UpdateCompressedIskPerHour();
        }

        private ObservableCollection<MoonOreIskPerHour> LoadStaticData()
        {
            MoonOreQuery moonOreQuery = new(harvestableRepository);
            return new(moonOreQuery.Execute());
        }

        private void UpdateMineralPrices()
        {
            materialModels = materialRepository.Find(m => m.Type == MineralTypeString || m.Type == "Moon Materials");
        }

        private void UpdateCompressedVariantPrices()
        {
            foreach (MoonOreIskPerHour moonOreIskPerHour in MoonOreIskPerHourCollection)
            {
                Harvestable compressedVariant = harvestableRepository.Find(h => h.HarvestableId == moonOreIskPerHour.CompressedVariantTypeId).SingleOrDefault();

                moonOreIskPerHour.CompressedPrices = compressedVariant?.Prices.ToDictionary(p => p.SystemId, p => new Isk(p.SellPercentile));
            }
        }

        private void UpdateMaterialIskPerHour()
        {
            foreach (MoonOreIskPerHour moonOreIskPerHour in MoonOreIskPerHourCollection)
            {
                CalculateMaterialIskPerHour(moonOreIskPerHour);
            }
        }

        //private void UpdateCompressedIskPerHour()
        //{
        //    foreach (MoonOreIskPerHour normalOreIskPerHour in MoonOreIskPerHourCollection)
        //    {
        //        CalculateCompressedIskPerHour(normalOreIskPerHour);
        //    }
        //}

        private void CalculateMaterialIskPerHour(MoonOreIskPerHour moonOreIskPerHour)
        {
            decimal batchValueAfterReprocessing = 0m;

            foreach (MaterialModel materialModel in moonOreIskPerHour.MaterialContent)
            {
                int materialsAfterReprocessing = Convert.ToInt32(Math.Floor(materialModel.Quantity * reprocessingEfficiency));

                decimal currentMarketPrice = GetSellPercentilePriceFromMaterial(materialModel);

                batchValueAfterReprocessing += materialsAfterReprocessing * currentMarketPrice;
            }

            decimal valuePerUnit = batchValueAfterReprocessing / BatchSize;
            decimal valuePerSquareMeters = valuePerUnit / moonOreIskPerHour.Volume.Amount;
            decimal valuePerSecond = valuePerSquareMeters * yieldPerSecond;
            decimal valuePerHour = valuePerSecond * 60 * 60;

            moonOreIskPerHour.MaterialIskPerHour = new Isk(valuePerHour);
        }

        private decimal GetSellPercentilePriceFromMaterial(MaterialModel material)
        {
            decimal sellPercentile = 0;

            if (materialModels.Single(m => m.Name == material.Name).Prices.Any())
            {
                IList<Price> prices = materialModels.Single(m => m.Name == material.Name).Prices;

                Price price = prices.SingleOrDefault(p => p.SystemId == systemToUseForPrices);

                if (price != null)
                {
                    sellPercentile = price.SellPercentile;
                }
            }

            return sellPercentile;
        }

        //private void CalculateCompressedIskPerHour(MoonOreIskPerHour moonOreIskPerHour)
        //{
        //    decimal yieldPerSecondDividedByVolume = yieldPerSecond / moonOreIskPerHour.Volume.Amount;
        //    decimal batchSizeCompensatedVolume = yieldPerSecondDividedByVolume / 100; //batch size

        //    decimal unitMarketPrice = moonOreIskPerHour.CompressedPrices.Any() ? moonOreIskPerHour.CompressedPrices[systemToUseForPrices].Amount : 0;

        //    decimal normalizedCompressedBatchValue = unitMarketPrice * batchSizeCompensatedVolume;
        //    decimal compressedValuePerHour = normalizedCompressedBatchValue * 60 * 60;

        //    moonOreIskPerHour.CompressedIskPerHour = new Isk(compressedValuePerHour);
        //}

        private void SetVisibilityForImprovedVariants()
        {
            moonOreIskPerHourCollection.Where(o => o.IsImprovedVariant).ToList().ForEach(o => o.Visible = showImprovedVariantsIsChecked);
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
