using System;

namespace SharpCrokite.Infrastructure.Common
{
    public class Volume : IComparable
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

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Volume comparableVolume = obj as Volume;

            if (Amount > comparableVolume.Amount) return 1;
            else if (Amount < comparableVolume.Amount) return -1;
            else return 0;
        }
    }
}
