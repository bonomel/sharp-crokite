namespace SharpCrokite.Core.ViewModels
{
    public class HarvestableViewModel
    {
        public int HarvestableId { get; set; }
        public byte[] Icon { get; internal set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string MaterialContents { get; set; }
        public string Description { get; set; }
        //public IList<MaterialContentViewModel> MaterialContents { get; set; } = new List<MaterialContentViewModel>();
        public HarvestableViewModel IsCompressedVariantOfType { get; set; }
    }
}
