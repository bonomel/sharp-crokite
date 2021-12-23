using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    [DebuggerDisplay("{Type} - {Name}")]
    [UsedImplicitly]
    public class IceIskPerHour : HarvestableIskPerHour
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

        public int OxygenIsotopes => GetQuantityOrDefault("Oxygen Isotopes");
        public int HeliumIsotopes => GetQuantityOrDefault("Helium Isotopes");
        public int NitrogenIsotopes => GetQuantityOrDefault("Nitrogen Isotopes");
        public int HydrogenIsotopes => GetQuantityOrDefault("Hydrogen Isotopes");
        public int HeavyWater => GetQuantityOrDefault("Heavy Water");
        public int LiquidOzone => GetQuantityOrDefault("Liquid Ozone");
        public int StrontiumClathrates => GetQuantityOrDefault("Strontium Clathrates");
    }
}
