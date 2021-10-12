namespace SharpCrokite.DataAccess.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public Material Material { get; set; }
        public Harvestable Harvestable { get; set; }
        public int SystemId { get; set; }
        public decimal BuyMax { get; set; }
        public decimal BuyMin { get; set; }
        public decimal BuyPercentile { get; set; }
        public decimal SellMax { get; set; }
        public decimal SellMin { get; set; }
        public decimal SellPercentile { get; set; }
    }
}
