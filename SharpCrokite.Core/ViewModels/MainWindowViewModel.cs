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

        [UsedImplicitly] public AsyncRelayCommand DeletePricesCommand { get; private set; }
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
            get => selectedPriceRetrievalServiceServiceOption ??= PriceRetrievalServiceOptions.First();
            set
            {
                selectedPriceRetrievalServiceServiceOption = value;
                NotifyPropertyChanged(nameof(SelectedPriceRetrievalServiceOption));
            }
        }

        [UsedImplicitly] public bool UpdatePricesButtonEnabled => !UpdatePricesCommand.IsExecuting;

        [UsedImplicitly] public bool DeletePricesButtonEnabled => !DeletePricesCommand.IsExecuting;

        public MainWindowViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository,
            NavigatorViewModel navigatorViewModel, IskPerHourViewModel iskPerHourViewModel, SurveyCalculatorViewModel surveyCalculatorViewModel)
        {
            UpdateStaticDataCommand = new RelayCommand(OnUpdateStaticData, CanUpdateStaticData);
            DeleteStaticDataCommand = new RelayCommand(OnDeleteStaticData, CanDeleteStaticData);
            UpdatePricesCommand = new AsyncRelayCommand(OnUpdatePrices, CanUpdatePrices, () => NotifyPropertyChanged(nameof(UpdatePricesButtonEnabled)));
            DeletePricesCommand = new AsyncRelayCommand(OnDeletePrices, CanDeletePrices, () => NotifyPropertyChanged(nameof(DeletePricesButtonEnabled)));

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

        private async Task OnDeletePrices()
        {
            PriceUpdateHandler priceUpdateHandler = new((IPriceRetrievalService)Activator.CreateInstance(SelectedPriceRetrievalServiceOption.ServiceType), harvestableRepository, materialRepository);

            await Task.Run(() => priceUpdateHandler.DeleteAllPrices());
            await Task.Run(() => iskPerHourViewModel.UpdatePrices());
        }

        private void OnUpdateStaticData()
        {
            StaticDataUpdateController staticDataUpdateController = new(new EsiStaticDataRetriever(), harvestableRepository, materialRepository);

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
            StaticDataUpdateController staticDataUpdateController = new(new EsiStaticDataRetriever(), harvestableRepository, materialRepository);

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
