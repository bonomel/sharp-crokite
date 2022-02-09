using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;

using JetBrains.Annotations;

using SharpCrokite.Core.Commands;
using SharpCrokite.Core.Models;
using SharpCrokite.Core.PriceUpdater;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MainWindowViewModel
    {
        [UsedImplicitly]
        public Guid Id { get; } = Guid.NewGuid();

        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        [UsedImplicitly]
        public IskPerHourViewModel<AsteroidIskPerHour> AsteroidIskPerHourViewModel { get; }

        [UsedImplicitly]
        public IskPerHourViewModel<MoonOreIskPerHour> MoonOreIskPerHourViewModel { get; }

        [UsedImplicitly]
        public IskPerHourViewModel<IceIskPerHour> IceIskPerHourViewModel { get; }

        public IContentViewModel CurrentViewModel { get; }

        private IEnumerable<IContentViewModel> contentViewModels;

        public MainWindowViewModel(AsteroidIskPerHourViewModel asteroidIskPerHourViewModel, 
            MoonOreIskPerHourViewModel moonOreIskPerHourViewModel, IceIskPerHourViewModel iceIskPerHourViewModel,
            HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            AsteroidIskPerHourViewModel = asteroidIskPerHourViewModel;
            MoonOreIskPerHourViewModel = moonOreIskPerHourViewModel;
            IceIskPerHourViewModel = iceIskPerHourViewModel;

            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
            UpdatePricesCommand = new RelayCommand(OnUpdatePrices, CanUpdatePrices);
            DeletePricesCommand = new RelayCommand(OnDeletePrices, CanDeletePrices);
        }

        [UsedImplicitly]
        public RelayCommand UpdatePricesCommand { get; private set; }

        private static bool CanUpdatePrices()
        {
            return true;
        }

        private void OnUpdatePrices()
        {
            PriceUpdateController priceUpdateController = new(new EveMarketerPriceRetriever(), harvestableRepository, materialRepository);
            priceUpdateController.UpdatePrices();

            AsteroidIskPerHourViewModel.UpdatePrices();
            MoonOreIskPerHourViewModel.UpdatePrices();
            IceIskPerHourViewModel.UpdatePrices();
        }

        [UsedImplicitly]
        public RelayCommand DeletePricesCommand { get; private set; }

        private static bool CanDeletePrices()
        {
            return true;
        }

        private void OnDeletePrices()
        {
            PriceUpdateController priceUpdateController = new(new EveMarketerPriceRetriever(), harvestableRepository, materialRepository);
            priceUpdateController.DeleteAllPrices();

            AsteroidIskPerHourViewModel.UpdatePrices();
            MoonOreIskPerHourViewModel.UpdatePrices();
            IceIskPerHourViewModel.UpdatePrices();
        }

        [UsedImplicitly]
        public RelayCommand UpdateStaticDataCommand { get; private set; }

        private void OnUpdateStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            try
            {
                staticDataUpdateController.UpdateData();
                AsteroidIskPerHourViewModel.ReloadStaticData();
                MoonOreIskPerHourViewModel.ReloadStaticData();
                IceIskPerHourViewModel.ReloadStaticData();
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

        private static bool CanUpdateStaticData()
        {
            return true;
        }

        [UsedImplicitly]
        public RelayCommand DeleteStaticDataCommand { get; private set; }

        private void OnDeleteStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            staticDataUpdateController.DeleteAllStaticData();
            AsteroidIskPerHourViewModel.ReloadStaticData();
            MoonOreIskPerHourViewModel.ReloadStaticData();
            IceIskPerHourViewModel.ReloadStaticData();
        }

        private bool CanDeleteStaticData()
        {
            return true;
        }
    }

    public interface IContentViewModel
    {
    }
}
