using SharpCrokite.Core.Models;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.DataAccess.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public class HarvestablesViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<Harvestable> harvestableRepository;
        private readonly IRepository<Material> materialRepository;
        private ObservableCollection<HarvestableModel> harvestables;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ObservableCollection<HarvestableModel> Harvestables
        {
            get => harvestables;
            set
            {
                if (harvestables != value)
                {
                    harvestables = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Harvestables)));
                }
            }
        }

        public HarvestablesViewModel()
        {
            SharpCrokiteDbContext dbContext = new();
            harvestableRepository = new HarvestableRepository(dbContext);
            materialRepository = new MaterialRepository(dbContext);

            LoadHarvestables();
        }

        private void LoadHarvestables()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            AllHarvestablesQuery allHarvestablesQuery = new(harvestableRepository, materialRepository);
            Harvestables = new(allHarvestablesQuery.Execute());
        }
    }
}
