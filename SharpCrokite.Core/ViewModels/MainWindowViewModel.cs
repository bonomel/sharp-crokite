using System;
using System.Net.Http;
using System.Windows;

using SharpCrokite.Core.Commands;
using SharpCrokite.Core.PriceUpdater;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public HarvestablesViewModel HarvestablesViewModel { get; }
        public AsteroidIskPerHourViewModel AsteroidIskPerHourViewModel { get; }
        public MoonOreIskPerHourViewModel MoonOreIskPerHourViewModel { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public MainWindowViewModel(HarvestablesViewModel harvestablesViewModel, AsteroidIskPerHourViewModel asteroidIskPerHourViewModel,
            MoonOreIskPerHourViewModel moonOreIskPerHourViewModel, HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            HarvestablesViewModel = harvestablesViewModel;
            AsteroidIskPerHourViewModel = asteroidIskPerHourViewModel;
            MoonOreIskPerHourViewModel = moonOreIskPerHourViewModel;

            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
            UpdatePricesCommand = new RelayCommand(OnUpdatePrices, CanUpdatePrices);
            DeletePricesCommand = new RelayCommand(OnDeletePrices, CanDeletePrices);
        }


        public RelayCommand UpdatePricesCommand { get; private set; }

        private bool CanUpdatePrices()
        {
            return true;
        }

        private void OnUpdatePrices()
        {
            PriceUpdateController priceUpdateController = new(new EveMarketerPriceRetriever(), harvestableRepository, materialRepository);

            priceUpdateController.UpdatePrices();
            HarvestablesViewModel.UpdateHarvestables();
            AsteroidIskPerHourViewModel.UpdatePrices();
            MoonOreIskPerHourViewModel.UpdatePrices();
        }

        public RelayCommand DeletePricesCommand { get; private set; }

        private bool CanDeletePrices()
        {
            return true;
        }

        private void OnDeletePrices()
        {
            PriceUpdateController priceUpdateController = new(new EveMarketerPriceRetriever(), harvestableRepository, materialRepository);

            priceUpdateController.DeleteAllPrices();
            HarvestablesViewModel.UpdateHarvestables();
            AsteroidIskPerHourViewModel.UpdatePrices();
            MoonOreIskPerHourViewModel.UpdatePrices();
        }

        public RelayCommand UpdateStaticDataCommand { get; private set; }

        private void OnUpdateStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            try
            {
                staticDataUpdateController.UpdateData();
                HarvestablesViewModel.UpdateHarvestables();
                AsteroidIskPerHourViewModel.ReloadStaticData();
                MoonOreIskPerHourViewModel.ReloadStaticData();
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
                harvestableRepository, materialRepository);

            staticDataUpdateController.DeleteAllStaticData();
            HarvestablesViewModel.UpdateHarvestables();
            AsteroidIskPerHourViewModel.ReloadStaticData();
            MoonOreIskPerHourViewModel.ReloadStaticData();
        }

        private bool CanDeleteStaticData()
        {
            return true;
        }
    }
}
