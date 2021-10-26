using SharpCrokite.Infrastructure.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace SharpCrokite.Core.Models
{
    public class NormalOreIskPerHour : INotifyPropertyChanged
    {
        private bool visible = true;
        private Isk materialIskPerHour = new(0);
        private Isk compressedIskPerHour = new(0);

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        internal int Id { get; set; }
        public byte[] Icon { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string Description { get; internal set; }
        public Volume Volume { get; internal set; }
        internal bool IsImprovedVariant { get; set; }

        public bool Visible
        {
            get => !IsImprovedVariant || visible;
            internal set
            {
                visible = value;
                NotifyPropertyChanged(nameof(Visible));
            }
        }

        internal int CompressedVariantTypeId { get; set; }
        
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
        public Dictionary<string, int> Minerals { get; internal set; } = new() { };
        public int Tritanium => Minerals.GetValueOrDefault(nameof(Tritanium));
        public int Pyerite => Minerals.GetValueOrDefault(nameof(Pyerite));
        public int Mexallon => Minerals.GetValueOrDefault(nameof(Mexallon));
        public int Isogen => Minerals.GetValueOrDefault(nameof(Isogen));
        public int Nocxium => Minerals.GetValueOrDefault(nameof(Nocxium));
        public int Zydrine => Minerals.GetValueOrDefault(nameof(Zydrine));
        public int Megacyte => Minerals.GetValueOrDefault(nameof(Megacyte));

        private void NotifyPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
