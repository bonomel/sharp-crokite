using System.Collections.Generic;

namespace SharpCrokite.DataAccess.Models
{
    public class Material
    {
        public int MaterialId { get; init; }
        public string Type { get; set; }
        public string Name { get; set; }
        public byte[] Icon { get; set; }
        public string Description { get; set; }
        public List<Price> Prices { get; set; } = new();
    }
}