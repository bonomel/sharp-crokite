﻿using SharpCrokite.Core.Commands;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Net.Http;
using System.Windows;

namespace SharpCrokite.Core.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        private HarvestablesViewModel harvestablesViewModel;
        public HarvestablesViewModel HarvestablesViewModel
        {
            get => harvestablesViewModel;
            private set => SetProperty(ref harvestablesViewModel, value);
        }

        public MainWindowViewModel(HarvestablesViewModel harvestablesViewModel, HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestablesViewModel = harvestablesViewModel;
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
        }

        public MainWindowViewModel()
        {
            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
        }

        public RelayCommand UpdateStaticDataCommand { get; private set; }

        private void OnUpdateStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

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
                harvestableRepository, materialRepository);

            staticDataUpdateController.DeleteAllStaticData();
        }

        private bool CanDeleteStaticData()
        {
            return true;
        }
    }
}
