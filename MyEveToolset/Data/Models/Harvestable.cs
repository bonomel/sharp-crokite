using System.Collections.Generic;

namespace MyEveToolset.Data.Models
{
    public class Harvestable
    {
        public int HarvestableId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MaterialContent> MaterialContents { get; set; } = new List<MaterialContent>();
        public List<Price> Prices { get; set; } = new List<Price>();
        public int? IsCompressedVariantOfType { get; set; }
    }
}
