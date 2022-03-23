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
    public class IceIskPerHourGridViewModel : IskPerHourGridViewModel<IceIskPerHour>, INotifyPropertyChanged
    {
        public IceIskPerHourGridViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
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

        protected override int BatchSize => 1;

        protected sealed override ObservableCollection<IceIskPerHour> LoadStaticData()
        {
            IceHarvestableIskPerHourQuery iceHarvestableIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<IceIskPerHour>(iceHarvestableIskPerHourQuery.Execute());
        }
    }
}
