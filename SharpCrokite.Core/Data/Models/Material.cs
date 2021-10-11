using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEveToolset.Data.Models
{
    public class Material
    {
        public int MaterialId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public byte[] Icon { get; set; }
        public string Description { get; set; }
        public List<Price> Prices { get; set; } = new List<Price>();
    }
}