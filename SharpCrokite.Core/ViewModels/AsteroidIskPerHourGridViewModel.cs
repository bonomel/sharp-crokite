using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SharpCrokite.Core.Commands;
using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class AsteroidIskPerHourGridViewModel : IskPerHourGridViewModel<AsteroidIskPerHour>, INotifyPropertyChanged
    {
        public AsteroidIskPerHourGridViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
            : base(harvestableRepository, materialRepository)
        {
            if (harvestableIskPerHourCollection.Any())
            {
                UpdateMaterialPrices().FireAndForgetAsync();
                UpdateCompressedVariantPrices().FireAndForgetAsync();
            }

            UpdateMaterialIskPerHour();
            UpdateCompressedIskPerHour();
        }

        internal override async Task UpdatePrices()
        {
            await UpdateMaterialPrices();
            await UpdateCompressedVariantPrices();

            UpdateMaterialIskPerHour();
            UpdateCompressedIskPerHour();
        }

        protected override int BatchSize => 100;

        protected sealed override ObservableCollection<AsteroidIskPerHour> LoadStaticData()
        {
            AsteroidHarvestableIskPerHourQuery asteroidHarvestableIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<AsteroidIskPerHour>(asteroidHarvestableIskPerHourQuery.Execute());
        }
    }
}
