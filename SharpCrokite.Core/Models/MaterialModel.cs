namespace SharpCrokite.Core.Models
{
    public class MaterialModel
    {
        public string Name;
        public int Quantity;
        public string Quality;

        public override string ToString()
        {
            return $"{Quantity} {Name}";
        }
    }
}