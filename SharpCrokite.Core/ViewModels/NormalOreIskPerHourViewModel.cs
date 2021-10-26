using SharpCrokite.Core.Models;
using SharpCrokite.Core.Queries;
using SharpCrokite.Infrastructure.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace SharpCrokite.Core.ViewModels
{
    public class NormalOreIskPerHourViewModel : INotifyPropertyChanged
    {
        private readonly CultureInfo ci = new("en-us");

        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ObservableCollection<NormalOreIskPerHour> normalOreIskPerHourCollection = new();
        public ObservableCollection<NormalOreIskPerHour> NormalOreIskPerHourCollection
        {
            get => normalOreIskPerHourCollection;
            private set
            {
                if (Equals(value, normalOreIskPerHourCollection)) { return; }
                normalOreIskPerHourCollection = value;
                SetVisibilityForImprovedVariants();
                NotifyPropertyChanged(nameof(NormalOreIskPerHourCollection));
            }
        }

        private bool showImprovedVariantsIsChecked;
        public bool ShowImprovedVariantsIsChecked
        {
            get => showImprovedVariantsIsChecked;
            set
            {
                showImprovedVariantsIsChecked = value;
                SetVisibilityForImprovedVariants();
            }
        }

        private decimal yieldPerSecond = 50m;
        public string YieldPerSecondText
        {
            get => yieldPerSecond.ToString("F02", ci);
            set
            {
                if (yieldPerSecond.ToString("F02", ci) != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        yieldPerSecond = 0;
                        NotifyPropertyChanged(nameof(YieldPerSecondText));
                    }
                    else if (decimal.TryParse(value, NumberStyles.Float, ci, out decimal result))
                    {
                        if(yieldPerSecond != Math.Round(result, 2))
                        {
                            yieldPerSecond = result;
                            NotifyPropertyChanged(nameof(YieldPerSecondText));
                        }
                    }
                }
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public NormalOreIskPerHourViewModel(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;

            normalOreIskPerHourCollection = LoadNormalIskPerHour();

            showImprovedVariantsIsChecked = true;
        }

        internal void UpdateNormalOreIskPerHour()
        {
            NormalOreIskPerHourCollection = LoadNormalIskPerHour();
        }

        private void SetVisibilityForImprovedVariants()
        {
            normalOreIskPerHourCollection.Where(o => o.IsImprovedVariant).ToList().ForEach(o => o.Visible = showImprovedVariantsIsChecked);
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
