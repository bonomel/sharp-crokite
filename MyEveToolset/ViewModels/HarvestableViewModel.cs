using System.Collections.Generic;

namespace MyEveToolset.ViewModels
{
    public class HarvestableViewModel
    {
        public int HarvestableId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string MaterialContents { get; set; }
        public string Description { get; set; }
        //public IList<MaterialContentViewModel> MaterialContents { get; set; } = new List<MaterialContentViewModel>();
        public int? IsCompressedVariantOfType { get; set; }
    }
}
