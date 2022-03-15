using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MoonOreIskPerHourGridViewModel : IskPerHourGridViewModel<MoonOreIskPerHour>, INotifyPropertyChanged
    {
        public MoonOreIskPerHourGridViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
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

        protected override int BatchSize => 100;

        private ObservableCollection<MoonOreIskPerHour> LoadStaticData()
        {
            MoonOreHarvestableIskPerHourQuery moonOreHarvestableIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<MoonOreIskPerHour>(moonOreHarvestableIskPerHourQuery.Execute());
        }

        internal override void ReloadStaticData()
        {
            HarvestableIskPerHourCollection = LoadStaticData();
        }

        protected override void CalculateCompressedIskPerHour(MoonOreIskPerHour iceIskPerHour)
        {
            decimal unitsPerSecond = YieldPerSecond / iceIskPerHour.Volume.Amount;

            decimal unitMarketPrice = iceIskPerHour.CompressedPrices != null
                                      && iceIskPerHour.CompressedPrices.Any()
                ? iceIskPerHour.CompressedPrices[SystemToUseForPrices].Amount
                : 0;

            decimal compressedValuePerHour = unitsPerSecond * unitMarketPrice * 3600;

            iceIskPerHour.CompressedIskPerHour = new Isk(compressedValuePerHour);
        }
    }
}
