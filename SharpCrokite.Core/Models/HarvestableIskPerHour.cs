using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    public abstract class HarvestableIskPerHour : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public int? CompressedVariantTypeId { get; set; }
        [UsedImplicitly]
        public byte[] Icon { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        [UsedImplicitly]
        public string Description { get; set; }
        public Volume Volume { get; set; }
        public bool IsImprovedVariant { get; set; }

        public List<MaterialModel> MaterialContent { get; set; } = new();

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

        private protected void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}