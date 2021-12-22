using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    public abstract class HarvestableIskPerHour : INotifyPropertyChanged
    {
        [UsedImplicitly]
        internal int Id { get; set; }
        [UsedImplicitly]
        public byte[] Icon { get; internal set; }
        public string Name { get; internal init; }
        public string Type { get; internal init; }
        [UsedImplicitly]
        public string Description { get; internal set; }
        public Volume Volume { get; internal init; }
        internal bool IsImprovedVariant { get; set; }

        private bool visible;
        public bool Visible
        {
            get => !IsImprovedVariant || visible;
            internal set
            {
                visible = value;
                NotifyPropertyChanged(nameof(Visible));
            }
        }

        public List<MaterialModel> MaterialContent { get; internal init; } = new();

        private Isk materialIskPerHour = new(0);
        public Isk MaterialIskPerHour
        {
            get => materialIskPerHour;
            internal set
            {
                if (materialIskPerHour != value)
                {
                    materialIskPerHour = value;
                    NotifyPropertyChanged(nameof(MaterialIskPerHour));
                }
            }
        }

        private protected int GetQuantityOrDefault(string materialName)
        {
            return MaterialContent.Find(materialModel => materialModel.Name == materialName) != null
                ? MaterialContent.Single(materialModel => materialModel.Name == materialName).Quantity : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private protected void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}