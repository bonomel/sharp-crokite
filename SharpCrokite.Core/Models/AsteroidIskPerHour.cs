using System.Diagnostics;
using JetBrains.Annotations;

namespace SharpCrokite.Core.Models
{
    [DebuggerDisplay("{Type} - {Name}")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AsteroidIskPerHour : CompressableIskPerHour
    {
        public int Tritanium => GetQuantityOrDefault(nameof(Tritanium));
        public int Pyerite => GetQuantityOrDefault(nameof(Pyerite));
        public int Mexallon => GetQuantityOrDefault(nameof(Mexallon));
        public int Isogen => GetQuantityOrDefault(nameof(Isogen));
        public int Nocxium => GetQuantityOrDefault(nameof(Nocxium));
        public int Zydrine => GetQuantityOrDefault(nameof(Zydrine));
        public int Megacyte => GetQuantityOrDefault(nameof(Megacyte));
    }
}
