namespace SharpCrokite.DataAccess.Models
{
    public class MaterialContent
    {
        public int MaterialContentId { get; set; }
        public int HarvestableId { get; set; }
        public Material Material { get; set; }
        public int Quantity { get; set; }
    }
}
