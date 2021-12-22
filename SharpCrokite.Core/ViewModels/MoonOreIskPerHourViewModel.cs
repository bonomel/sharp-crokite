using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MoonOreIskPerHourViewModel : IskPerHourViewModel<MoonOreIskPerHour>, INotifyPropertyChanged
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

        internal void UpdatePrices()
        {
            UpdateMaterialPrices();
            UpdateMaterialIskPerHour();
        }

        protected sealed override ObservableCollection<MoonOreIskPerHour> LoadStaticData()
        {
            MoonOreIskPerHourQuery moonOreIskPerHourQuery = new(HarvestableRepository);
            return new ObservableCollection<MoonOreIskPerHour>(moonOreIskPerHourQuery.Execute());
        }
    }
}
