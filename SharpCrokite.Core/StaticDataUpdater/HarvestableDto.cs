using System.Collections.Generic;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class HarvestableDto
    {
        public int HarvestableId { get; init; }
        public string Type { get; init; }
        public byte[] Icon { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public List<MaterialContentDto> MaterialContents { get; init; } = new();
        public int? IsCompressedVariantOfType { get; set; }
        public decimal Volume { get; init; }
    }
}
