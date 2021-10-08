using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEveToolset.Data.Models
{
    public class Material
    {
        public int MaterialId { get; set; }
        public List<Price> Prices { get; set; } = new List<Price>();
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
