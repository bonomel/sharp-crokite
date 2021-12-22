using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    [DebuggerDisplay("{Type} - {Name}")]
    public class MoonOreIskPerHour : INotifyPropertyChanged
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

        //internal int CompressedVariantTypeId { get; init; }

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

        public string Rarity4Material => GetMaterialDisplayNameOrEmpty(nameof(Rarity4Material));
        public string Rarity8Material => GetMaterialDisplayNameOrEmpty(nameof(Rarity8Material));
        public string Rarity16Material => GetMaterialDisplayNameOrEmpty(nameof(Rarity16Material));
        public string Rarity32Material => GetMaterialDisplayNameOrEmpty(nameof(Rarity32Material));
        public string Rarity64Material => GetMaterialDisplayNameOrEmpty(nameof(Rarity64Material));

        private string GetMaterialDisplayNameOrEmpty(string qualityName)
        {
            MaterialModel model = MaterialContent.Find(materialModel => materialModel.Quality == qualityName);
            return model != null ? model.ToString() : string.Empty;
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
