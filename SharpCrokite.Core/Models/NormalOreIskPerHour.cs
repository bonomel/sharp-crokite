using System.Collections.Generic;

namespace SharpCrokite.Core.Models
{
    public class NormalOreIskPerHour
    {
        public byte[] Icon { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Volume { get; set; }
        public Dictionary<string, int> Minerals { get; set; } = new() { };
        public int Tritanium => Minerals.GetValueOrDefault(nameof(Tritanium));
        public int Pyerite => Minerals.GetValueOrDefault(nameof(Pyerite));
        public int Mexallon => Minerals.GetValueOrDefault(nameof(Mexallon));
        public int Isogen => Minerals.GetValueOrDefault(nameof(Isogen));
        public int Nocxium => Minerals.GetValueOrDefault(nameof(Nocxium));
        public int Zydrine => Minerals.GetValueOrDefault(nameof(Zydrine));
        public int Megacyte => Minerals.GetValueOrDefault(nameof(Megacyte));
        public string MaterialIskPerHour { get; set; }
        public string CompressedIskPerHour { get; set; }
    }
}
