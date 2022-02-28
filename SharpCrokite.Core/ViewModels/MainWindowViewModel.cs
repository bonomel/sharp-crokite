﻿using System;
using System.Net.Http;
using System.Windows;

using JetBrains.Annotations;

using SharpCrokite.Core.Commands;
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
        public NavigatorViewModel NavigatorViewModel { get; set; }

        [UsedImplicitly]
        public RelayCommand DeletePricesCommand { get; private set; }

        [UsedImplicitly]
        public RelayCommand UpdatePricesCommand { get; private set; }

        [UsedImplicitly]
        public RelayCommand UpdateStaticDataCommand { get; private set; }

        [UsedImplicitly]
        public RelayCommand DeleteStaticDataCommand { get; private set; }

        // private IEnumerable<IContentViewModel> contentViewModels;

        public MainWindowViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository,
            NavigatorViewModel navigatorViewModel)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
            UpdatePricesCommand = new RelayCommand(OnUpdatePrices, CanUpdatePrices);
            DeletePricesCommand = new RelayCommand(OnDeletePrices, CanDeletePrices);

            //IskPerHourViewModel = iskPerHourViewModel;
            NavigatorViewModel = navigatorViewModel;
        }

        private void OnUpdatePrices()
        {
            PriceUpdateController priceUpdateController = new(new EveMarketerPriceRetriever(), harvestableRepository, materialRepository);
            priceUpdateController.UpdatePrices();

            //IskPerHourViewModel.UpdatePrices();
        }

        private void OnDeletePrices()
        {
            PriceUpdateController priceUpdateController = new(new EveMarketerPriceRetriever(), harvestableRepository, materialRepository);
            priceUpdateController.DeleteAllPrices();

            // IskPerHourViewModel.UpdatePrices();
        }

        private void OnUpdateStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            try
            {
                staticDataUpdateController.UpdateData();

                //IskPerHourViewModel.ReloadStaticData();
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
        private void OnDeleteStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            staticDataUpdateController.DeleteAllStaticData();

            //IskPerHourViewModel.ReloadStaticData();
        }

        private static bool CanUpdatePrices()
        {
            return true;
        }

        private static bool CanDeletePrices()
        {
            return true;
        }

        private static bool CanUpdateStaticData()
        {
            return true;
        }

        private static bool CanDeleteStaticData()
        {
            return true;
        }
    }
}
