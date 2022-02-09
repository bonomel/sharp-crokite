using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MoonOreIskPerHourViewModel : IskPerHourViewModel<MoonOreIskPerHour>, INotifyPropertyChanged, IContentViewModel
    {
        public MoonOreIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
            : base(harvestableRepository, materialRepository)
        {
            HarvestableIskPerHourCollection = LoadStaticData();

            if (harvestableIskPerHourCollection.Any())
            {
                UpdateMaterialPrices();
            }

            UpdateMaterialIskPerHour();
        }

        internal override void UpdatePrices()
        {
            UpdateMaterialPrices();
            UpdateMaterialIskPerHour();
        }

        protected override int BatchSize => 100;

        protected sealed override ObservableCollection<MoonOreIskPerHour> LoadStaticData()
        {
            MoonOreHarvestableIskPerHourQuery moonOreHarvestableIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<MoonOreIskPerHour>(moonOreHarvestableIskPerHourQuery.Execute());
        }
        internal override void ReloadStaticData()
        {
            HarvestableIskPerHourCollection = LoadStaticData();
        }
    }
}
