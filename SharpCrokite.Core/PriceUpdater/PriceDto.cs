namespace SharpCrokite.Core.PriceUpdater
{
    internal class PriceDto
    {
        public int TypeId { get; set; }
        public int SystemId { get; set; }
        public decimal BuyMax { get; set; }
        public decimal BuyMin { get; set; }
        public decimal BuyPercentile { get; set; }
        public decimal SellMax { get; set; }
        public decimal SellMin { get; set; }
        public decimal SellPercentile { get; set; }
    }
}