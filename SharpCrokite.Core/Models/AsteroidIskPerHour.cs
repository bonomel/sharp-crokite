using System.Collections.Generic;
using System.Diagnostics;
using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    [DebuggerDisplay("{Type} - {Name}")]
    public class AsteroidIskPerHour : HarvestableIskPerHour
    {
        internal Dictionary<int, Isk> CompressedPrices { get; set; }

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

        public int Tritanium => GetQuantityOrDefault(nameof(Tritanium));
        public int Pyerite => GetQuantityOrDefault(nameof(Pyerite));
        public int Mexallon => GetQuantityOrDefault(nameof(Mexallon));
        public int Isogen => GetQuantityOrDefault(nameof(Isogen));
        public int Nocxium => GetQuantityOrDefault(nameof(Nocxium));
        public int Zydrine => GetQuantityOrDefault(nameof(Zydrine));
        public int Megacyte => GetQuantityOrDefault(nameof(Megacyte));
    }
}
