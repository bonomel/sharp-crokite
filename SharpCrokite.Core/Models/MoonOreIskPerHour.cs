using System.Diagnostics;
using JetBrains.Annotations;

namespace SharpCrokite.Core.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerDisplay("{Type} - {Name}")]
    public class MoonOreIskPerHour : HarvestableIskPerHour
    {
        public string Rarity4 => GetMaterialDisplayNameOrEmpty(nameof(Rarity4));
        public string Rarity8 => GetMaterialDisplayNameOrEmpty(nameof(Rarity8));
        public string Rarity16 => GetMaterialDisplayNameOrEmpty(nameof(Rarity16));
        public string Rarity32 => GetMaterialDisplayNameOrEmpty(nameof(Rarity32));
        public string Rarity64 => GetMaterialDisplayNameOrEmpty(nameof(Rarity64));
        public int Pyerite => GetQuantityOrDefault(nameof(Pyerite));
        public int Mexallon => GetQuantityOrDefault(nameof(Mexallon));

        private string GetMaterialDisplayNameOrEmpty(string qualityName)
        {
            MaterialModel model = MaterialContent.Find(materialModel => materialModel.Quality == qualityName);
            return model != null ? model.ToString() : string.Empty;
        }
    }
}
