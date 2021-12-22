using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    [DebuggerDisplay("{Type} - {Name}")]
    public class AsteroidIskPerHour : INotifyPropertyChanged
    {
        internal int Id { get; set; }
        public byte[] Icon { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string Description { get; internal set; }
        public Volume Volume { get; internal set; }
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

        internal int CompressedVariantTypeId { get; init; }

        internal Dictionary<int, Isk> CompressedPrices { get; set; }

        private Isk materialIskPerHour = new(0);
        public Isk MaterialIskPerHour
        {
            get => materialIskPerHour;
            internal set
            {
                if(materialIskPerHour != value)
                {
                    materialIskPerHour = value;
                    NotifyPropertyChanged(nameof(MaterialIskPerHour));
                }
            }
        }

        private Isk compressedIskPerHour = new(0);
        public Isk CompressedIskPerHour
        {
            get => compressedIskPerHour;
            internal set
            {
                if (compressedIskPerHour != value)
                {
                    compressedIskPerHour = value;
                    NotifyPropertyChanged(nameof(CompressedIskPerHour));
                }
            }
        }

        public List<MaterialModel> MaterialContent { get; internal init; } = new();
        public int Tritanium => GetQuantityOrDefault(nameof(Tritanium));
        public int Pyerite => GetQuantityOrDefault(nameof(Pyerite));
        public int Mexallon => GetQuantityOrDefault(nameof(Mexallon));
        public int Isogen => GetQuantityOrDefault(nameof(Isogen));
        public int Nocxium => GetQuantityOrDefault(nameof(Nocxium));
        public int Zydrine => GetQuantityOrDefault(nameof(Zydrine));
        public int Megacyte => GetQuantityOrDefault(nameof(Megacyte));

        private int GetQuantityOrDefault(string materialName)
        {
            return MaterialContent.Find(materialModel => materialModel.Name == materialName) != null
                ? MaterialContent.Single(materialModel => materialModel.Name == materialName).Quantity : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
