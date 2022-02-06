using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public abstract class IskPerHourViewModel<T> where T : HarvestableIskPerHour
    {
        [UsedImplicitly]
        public Guid Id { get; } = Guid.NewGuid();

        private const string MineralTypeString = "Mineral";
        private const string MoonMaterialsTypeString = "Moon Materials";
        private const string IceProductsTypeString = "Ice Product";
        private const string TwoDecimalsFormatString = "F02";

        protected abstract int BatchSize { get; }

        private readonly CultureInfo invariantCultureInfo = new("en-us");
        private protected readonly int SystemToUseForPrices = 30000142; // Hard-coded Jita systemid - this will become a setting eventually

        protected readonly HarvestableRepository HarvestableRepository;
        private readonly MaterialRepository materialRepository;

        private IEnumerable<Material> materialModels;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        // ReSharper disable once InconsistentNaming
        private protected ObservableCollection<T> harvestableIskPerHourCollection = new();
        public ObservableCollection<T> HarvestableIskPerHourCollection
        {
            get => harvestableIskPerHourCollection;
            protected set
            {
                if (Equals(value, harvestableIskPerHourCollection)) { return; }
                harvestableIskPerHourCollection = value;
                SetVisibilityForImprovedVariants();
                NotifyPropertyChanged(nameof(HarvestableIskPerHourCollection));
            }
        }

        private protected decimal YieldPerSecond = 50m;
        [UsedImplicitly]
        public string YieldPerSecondText
        {
            get => YieldPerSecond.ToString(TwoDecimalsFormatString, invariantCultureInfo);
            set
            {
                if (YieldPerSecond.ToString(TwoDecimalsFormatString, invariantCultureInfo) != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        YieldPerSecond = 0;
                        UpdateIskPerHour();
                        NotifyPropertyChanged(nameof(YieldPerSecondText));
                    }
                    else if (decimal.TryParse(value, NumberStyles.Float, invariantCultureInfo, out decimal result))
                    {
                        if (YieldPerSecond != Math.Round(result, 2))
                        {
                            YieldPerSecond = result;
                            UpdateIskPerHour();
                            NotifyPropertyChanged(nameof(YieldPerSecondText));
                        }
                    }
                }
            }
        }

        private bool showImprovedVariantsIsChecked;
        [UsedImplicitly]
        public bool ShowImprovedVariantsIsChecked
        {
            get => showImprovedVariantsIsChecked;
            set
            {
                showImprovedVariantsIsChecked = value;
                SetVisibilityForImprovedVariants();
            }
        }

        protected IskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            HarvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        private decimal reprocessingEfficiency = 0.782m;
        [UsedImplicitly]
        public string ReprocessingEfficiencyText
        {
            get => (reprocessingEfficiency * 100).ToString(TwoDecimalsFormatString, invariantCultureInfo);
            set
            {
                if ((reprocessingEfficiency * 100).ToString(TwoDecimalsFormatString, invariantCultureInfo) != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        reprocessingEfficiency = 0;
                        UpdateMaterialIskPerHour();
                        NotifyPropertyChanged(nameof(ReprocessingEfficiencyText));
                    }
                    else if (decimal.TryParse(value, NumberStyles.Float, invariantCultureInfo, out decimal result))
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

        protected abstract ObservableCollection<T> LoadStaticData();

        internal abstract void ReloadStaticData();

        internal abstract void UpdatePrices();

        protected virtual void UpdateIskPerHour()
        {
            UpdateMaterialIskPerHour();
        }

        private void CalculateMaterialIskPerHour(T harvestableIskPerHour)
        {
            decimal batchValueAfterReprocessing = 0m;

            foreach (MaterialModel materialModel in harvestableIskPerHour.MaterialContent)
            {
                int materialsAfterReprocessing = Convert.ToInt32(Math.Floor(materialModel.Quantity * reprocessingEfficiency));

                decimal currentMarketPrice = GetSellPercentilePriceFromMaterial(materialModel);

                batchValueAfterReprocessing += materialsAfterReprocessing * currentMarketPrice;
            }

            decimal valuePerUnit = batchValueAfterReprocessing / BatchSize;
            decimal valuePerSquareMeters = valuePerUnit / harvestableIskPerHour.Volume.Amount;
            decimal valuePerSecond = valuePerSquareMeters * YieldPerSecond;
            decimal valuePerHour = valuePerSecond * 60 * 60;

            harvestableIskPerHour.MaterialIskPerHour = new Isk(valuePerHour);
        }

        private protected void UpdateMaterialPrices()
        {
            materialModels = materialRepository.Find(material => material.Type == MineralTypeString
                || material.Type == MoonMaterialsTypeString || material.Type == IceProductsTypeString);
        }

        private protected void UpdateMaterialIskPerHour()
        {
            foreach (T harvestableIskPerHour in HarvestableIskPerHourCollection)
            {
                CalculateMaterialIskPerHour(harvestableIskPerHour);
            }
        }

        private decimal GetSellPercentilePriceFromMaterial(MaterialModel material)
        {
            decimal sellPercentile = 0;

            if (materialModels.Single(m => m.Name == material.Name).Prices.Any())
            {
                IList<Price> prices = materialModels.Single(m => m.Name == material.Name).Prices;

                Price price = prices.SingleOrDefault(p => p.SystemId == SystemToUseForPrices);

                if (price != null)
                {
                    sellPercentile = price.SellPercentile;
                }
            }

            return sellPercentile;
        }

        private void SetVisibilityForImprovedVariants()
        {
            harvestableIskPerHourCollection.Where(o => o.IsImprovedVariant).ToList().ForEach(o => o.Visible = showImprovedVariantsIsChecked);
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