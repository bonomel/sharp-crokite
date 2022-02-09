using System.Diagnostics;
using JetBrains.Annotations;

namespace SharpCrokite.Core.Models
{
    [DebuggerDisplay("{Type} - {Name}")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IceIskPerHour : CompressableIskPerHour
    {
        public int OxygenIsotopes => GetQuantityOrDefault("Oxygen Isotopes");
        public int HeliumIsotopes => GetQuantityOrDefault("Helium Isotopes");
        public int NitrogenIsotopes => GetQuantityOrDefault("Nitrogen Isotopes");
        public int HydrogenIsotopes => GetQuantityOrDefault("Hydrogen Isotopes");
        public int HeavyWater => GetQuantityOrDefault("Heavy Water");
        public int LiquidOzone => GetQuantityOrDefault("Liquid Ozone");
        public int StrontiumClathrates => GetQuantityOrDefault("Strontium Clathrates");
    }
}
