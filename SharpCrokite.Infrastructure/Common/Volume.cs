namespace SharpCrokite.Infrastructure.Common
{
    public class Volume
    {
        public decimal Amount { get; }

        public Volume(decimal amount)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Amount} m³";
        }
    }
}
