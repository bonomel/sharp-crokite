﻿using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharpCrokite.Core.ViewModels
{
    public class NormalOreIskPerHourViewModel : INotifyPropertyChanged
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ObservableCollection<NormalOreIskPerHour> normalOreIskPerHourCollection = new();
        public ObservableCollection<NormalOreIskPerHour> NormalOreIskPerHourCollection
        { 
            get => normalOreIskPerHourCollection;
            private set
            {
                if (Equals(value, normalOreIskPerHourCollection)) return;

                normalOreIskPerHourCollection = value;

                NotifyPropertyChanged(nameof(NormalOreIskPerHourCollection));
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public NormalOreIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            normalOreIskPerHourCollection = LoadNormalIskPerHour();
        }

        internal void UpdateNormalOreIskPerHour()
        {
            NormalOreIskPerHourCollection = LoadNormalIskPerHour();
        }

        private ObservableCollection<NormalOreIskPerHour> LoadNormalIskPerHour()
        {
            NormalOreIskPerHourQuery normalOreIskPerHourQuery = new(harvestableRepository, materialRepository);
            return new(normalOreIskPerHourQuery.Execute());
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
