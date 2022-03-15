namespace SharpCrokite.Core.PriceUpdater
{
    public class PriceDto
    {
        public int TypeId { get; init; }
        public int SystemId { get; init; }
        public decimal BuyMax { get; init; }
        public decimal BuyMin { get; init; }
        public decimal BuyPercentile { get; init; }
        public decimal SellMax { get; init; }
        public decimal SellMin { get; init; }
        public decimal SellPercentile { get; init; }
    }
}