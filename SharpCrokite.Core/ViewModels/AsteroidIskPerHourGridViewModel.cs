using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Common;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class AsteroidIskPerHourGridViewModel : IskPerHourGridViewModel<AsteroidIskPerHour>, INotifyPropertyChanged
    {
        public AsteroidIskPerHourGridViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
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

        private ObservableCollection<AsteroidIskPerHour> LoadStaticData()
        {
            AsteroidHarvestableIskPerHourQuery asteroidHarvestableIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<AsteroidIskPerHour>(asteroidHarvestableIskPerHourQuery.Execute());
        }

        internal sealed override void ReloadStaticData()
        {
            HarvestableIskPerHourCollection = LoadStaticData();
        }

        protected override void CalculateCompressedIskPerHour(AsteroidIskPerHour asteroidIskPerHour)
        {
            decimal unitsPerSecond = YieldPerSecond / asteroidIskPerHour.Volume.Amount;

            decimal unitMarketPrice = asteroidIskPerHour.CompressedPrices != null
                                      && asteroidIskPerHour.CompressedPrices.Any()
                                      ? asteroidIskPerHour.CompressedPrices[SystemToUseForPrices].Amount
                                      : 0;

            decimal compressedValuePerHour = unitsPerSecond * unitMarketPrice * 3600;

            asteroidIskPerHour.CompressedIskPerHour = new Isk(compressedValuePerHour);
        }
    }
}
