using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
    }
}
