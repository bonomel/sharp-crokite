using System.Collections.Generic;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class HarvestableDto
    {
        public int HarvestableId { get; set; }
        public string Type { get; set; }
        public byte[] Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MaterialContentDto> MaterialContents { get; set; } = new List<MaterialContentDto>();
        public int? IsCompressedVariantOfType { get; set; }
        public decimal Volume { get; set; }
    }
}
