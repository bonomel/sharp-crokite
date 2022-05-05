﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

using JetBrains.Annotations;

using SharpCrokite.Core.Commands;
using SharpCrokite.Core.PriceRetrievalService;
using SharpCrokite.Core.StaticDataUpdater;
using SharpCrokite.Core.StaticDataUpdater.Esi;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;
        private readonly IskPerHourViewModel iskPerHourViewModel;
        private readonly List<IContentViewModel> contentViewModels = new();
        private IContentViewModel currentContentViewModel;
        private PriceRetrievalServiceOption selectedPriceRetrievalServiceServiceOption;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [UsedImplicitly] public NavigatorViewModel NavigatorViewModel { get; set; }

        [UsedImplicitly] public RelayCommand DeletePricesCommand { get; private set; }
        [UsedImplicitly] public AsyncRelayCommand UpdatePricesCommand { get; private set; }
        [UsedImplicitly] public RelayCommand UpdateStaticDataCommand { get; private set; }
        [UsedImplicitly] public RelayCommand DeleteStaticDataCommand { get; private set; }

        [UsedImplicitly]
        public IContentViewModel CurrentContentViewModel
        {
            get => currentContentViewModel;
            set
            {
                currentContentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentContentViewModel));
            }
        }

        [UsedImplicitly] public IEnumerable<PriceRetrievalServiceOption> PriceRetrievalServiceOptions { get; set; }

        [UsedImplicitly]
        public PriceRetrievalServiceOption SelectedPriceRetrievalServiceOption
        {
            get
            {
                if (selectedPriceRetrievalServiceServiceOption == null)
                {
                    selectedPriceRetrievalServiceServiceOption = PriceRetrievalServiceOptions.First();
                    return selectedPriceRetrievalServiceServiceOption;
                }

                return selectedPriceRetrievalServiceServiceOption;
            }
            set
            {
                selectedPriceRetrievalServiceServiceOption = value;
                NotifyPropertyChanged(nameof(SelectedPriceRetrievalServiceOption));
            }
        }

        [UsedImplicitly] public bool UpdatePricesButtonEnabled => !UpdatePricesCommand.IsExecuting;

        public MainWindowViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository,
            NavigatorViewModel navigatorViewModel, IskPerHourViewModel iskPerHourViewModel, SurveyCalculatorViewModel surveyCalculatorViewModel)
        {
            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
            UpdatePricesCommand = new AsyncRelayCommand(OnUpdatePrices, CanUpdatePrices, () => NotifyPropertyChanged(nameof(UpdatePricesButtonEnabled)));
            DeletePricesCommand = new RelayCommand(OnDeletePrices, CanDeletePrices);

            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            this.iskPerHourViewModel = iskPerHourViewModel;
            currentContentViewModel = iskPerHourViewModel;

            contentViewModels.Add(iskPerHourViewModel);
            contentViewModels.Add(surveyCalculatorViewModel);

            NavigatorViewModel = navigatorViewModel;
            NavigatorViewModel.CurrentViewModelChanged += OnCurrentViewModelChanged;

            PriceRetrievalServiceOptions = PriceRetrievalOptionsProvider.Build();
        }

        private void OnCurrentViewModelChanged(Type parameter)
        {
            CurrentContentViewModel = contentViewModels.Single(viewmodel => viewmodel.GetType() == parameter);
        }

        private async Task OnUpdatePrices()
        {
            PriceUpdateHandler priceUpdateHandler = new((IPriceRetrievalService)Activator.CreateInstance(SelectedPriceRetrievalServiceOption.ServiceType), harvestableRepository, materialRepository);

            await Task.Run(() => priceUpdateHandler.UpdatePrices());
            await Task.Run(() => iskPerHourViewModel.UpdatePrices());
        }

        private void OnDeletePrices()
        {
            PriceUpdateHandler priceUpdateHandler = new((IPriceRetrievalService)Activator.CreateInstance(SelectedPriceRetrievalServiceOption.ServiceType), harvestableRepository, materialRepository);

            priceUpdateHandler.DeleteAllPrices();

            iskPerHourViewModel.UpdatePrices().FireAndForgetAsync();
        }

        private void OnUpdateStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            try
            {
                staticDataUpdateController.UpdateData();

                iskPerHourViewModel.ReloadStaticData();
            }
            catch (HttpRequestException ex)
            {
                _ = MessageBox.Show($"Something went wrong while updating the static data!\nMessage:\n{ex.Message}",
                    "Http Request Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                _ = MessageBox.Show($"Something went wrong while updating the static data!\nMessage:\n{ex.Message}",
                    "Argument Null Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnDeleteStaticData()
        {
            var staticDataUpdateController = new StaticDataUpdateController(new EsiStaticDataRetriever(),
                harvestableRepository, materialRepository);

            staticDataUpdateController.DeleteAllStaticData();

            iskPerHourViewModel.ReloadStaticData();
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

        private void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
