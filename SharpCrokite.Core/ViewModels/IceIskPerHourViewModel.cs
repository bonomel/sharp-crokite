using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class IceIskPerHourViewModel : IskPerHourViewModel<IceIskPerHour>, INotifyPropertyChanged, IContentViewModel
    {
        public IceIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
            : base(harvestableRepository, materialRepository)
        {
            HarvestableIskPerHourCollection = LoadStaticData();

            if (harvestableIskPerHourCollection.Any())
            {
                UpdateMaterialPrices();
                UpdateCompressedVariantPrices();
            }

            UpdateMaterialIskPerHour();
            UpdateCompressedIskPerHour();
        }

        internal override void UpdatePrices()
        {
            UpdateMaterialPrices();
            UpdateCompressedVariantPrices();

            UpdateMaterialIskPerHour();
            UpdateCompressedIskPerHour();
        }

        protected override int BatchSize => 1;

        protected sealed override ObservableCollection<IceIskPerHour> LoadStaticData()
        {
            IceHarvestableIskPerHourQuery iceHarvestableIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<IceIskPerHour>(iceHarvestableIskPerHourQuery.Execute());
        }

        internal sealed override void ReloadStaticData()
        {
            HarvestableIskPerHourCollection = LoadStaticData();
        }

        protected override void UpdateIskPerHour()
        {
            base.UpdateIskPerHour();
            UpdateCompressedIskPerHour();
        }

        private void UpdateCompressedVariantPrices()
        {
            foreach (IceIskPerHour iceIskPerHour in HarvestableIskPerHourCollection)
            {
                Harvestable compressedVariant = HarvestableRepository.Find(h => h.HarvestableId == iceIskPerHour.CompressedVariantTypeId).SingleOrDefault();

                iceIskPerHour.CompressedPrices = compressedVariant?.Prices.ToDictionary(p => p.SystemId, p => new Isk(p.SellPercentile));
            }
        }

        private void UpdateCompressedIskPerHour()
        {
            foreach (IceIskPerHour iceIskPerHour in HarvestableIskPerHourCollection)
            {
                CalculateCompressedIskPerHour(iceIskPerHour);
            }
        }

        private void CalculateCompressedIskPerHour(IceIskPerHour iceIskPerHour)
        {
            decimal yieldPerSecondDividedByVolume = YieldPerSecond / iceIskPerHour.Volume.Amount;
            decimal batchSizeCompensatedVolume = yieldPerSecondDividedByVolume / BatchSize;

            decimal unitMarketPrice = iceIskPerHour.CompressedPrices != null
                                      && iceIskPerHour.CompressedPrices.Any()
                                      ? iceIskPerHour.CompressedPrices[SystemToUseForPrices].Amount
                                      : 0;

            decimal normalizedCompressedBatchValue = unitMarketPrice * batchSizeCompensatedVolume;
            decimal compressedValuePerHour = normalizedCompressedBatchValue * 60 * 60;

            iceIskPerHour.CompressedIskPerHour = new Isk(compressedValuePerHour);
        }
    }
}
