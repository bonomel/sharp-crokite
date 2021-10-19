using SharpCrokite.Core.Commands;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.DataAccess.DatabaseContexts;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Net.Http;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly SharpCrokiteDbContext dbContext;
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public MainWindowViewModel()
        {
            dbContext = new SharpCrokiteDbContext();
            harvestableRepository = new HarvestableRepository(dbContext);
            materialRepository = new MaterialRepository(dbContext);

            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
        }

        public RelayCommand UpdateStaticDataCommand { get; private set; }

        private void OnUpdateStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));

            try
            {
                staticDataUpdateController.UpdateData();
            }
            catch (HttpRequestException ex)
            {
                _ = MessageBox.Show($"Something went wrong while updating the static data!\nMessage:\n{ex.Message}",
                    "Http Request Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                _ = MessageBox.Show($"Something went wrong while updating the static data!\nMessage:\n{ex.Message}",
                    "Http Request Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUpdateStaticData()
        {
            return true;
        }
        public RelayCommand DeleteStaticDataCommand { get; private set; }

        private void OnDeleteStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                new HarvestableRepository(dbContext), new MaterialRepository(dbContext));

            staticDataUpdateController.DeleteAllStaticData();
        }

        private bool CanDeleteStaticData()
        {
            return true;
        }
    }
}
