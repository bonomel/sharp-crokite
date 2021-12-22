namespace SharpCrokite.Core.Models
{
    public class MaterialModel
    {
        public string Name { get; init; }
        public int Quantity { get; init; }
        public string Quality { get; init; }

        public override string ToString()
        {
            return $"{Quantity} {Name}";
        }
    }
}