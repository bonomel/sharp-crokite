using JetBrains.Annotations;
using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class HarvestableModel
    {
        public int HarvestableId { get; internal set; }
        public byte[] Icon { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public Isk Price { get; internal set; }
        public string MaterialContents { get; internal set; }
        public string Description { get; internal set; }
        public HarvestableModel IsCompressedVariantOfType { get; internal set; }
        public decimal Volume { get; internal set; }
    }
}
