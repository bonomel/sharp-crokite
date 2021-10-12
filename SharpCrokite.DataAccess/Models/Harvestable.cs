using System.Collections.Generic;

namespace SharpCrokite.DataAccess.Models
{
    public class Harvestable
    {
        public int HarvestableId { get; set; }
        public string Type { get; set; }
        public byte[] Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MaterialContent> MaterialContents { get; set; } = new List<MaterialContent>();
        public List<Price> Prices { get; set; } = new List<Price>();
        public int? IsCompressedVariantOfType { get; set; }
    }
}
