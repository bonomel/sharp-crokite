using System.Globalization;

namespace SharpCrokite.Infrastructure.Common
{
    public class Isk
    {
        public decimal Amount { get; }

        private static NumberFormatInfo IskNumberFormatInfo => new()
        {
            CurrencyDecimalSeparator = ",",
            CurrencyDecimalDigits = 2,
            CurrencyGroupSeparator = ".",
            CurrencyGroupSizes = new int[] { 3 },
            CurrencySymbol = "ISK",
            CurrencyPositivePattern = 3,
            CurrencyNegativePattern = 8
        };

        public Isk(decimal amount)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return Amount != 0 ? $"{Amount.ToString("C", IskNumberFormatInfo)}" : "N/A";
        }
    }
}
