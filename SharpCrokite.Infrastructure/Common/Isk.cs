using System;
using System.Globalization;

namespace SharpCrokite.Infrastructure.Common
{
    public class Isk : IComparable
    {
        public decimal Amount { get; }

        private static NumberFormatInfo IskNumberFormatInfo => new()
        {
            CurrencyDecimalSeparator = ",",
            CurrencyDecimalDigits = 2,
            CurrencyGroupSeparator = ".",
            CurrencyGroupSizes = new[] { 3 },
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

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Isk comparableIsk = obj as Isk;

            if (comparableIsk != null && Amount > comparableIsk.Amount) return 1;
            if (comparableIsk != null && Amount < comparableIsk.Amount) return -1;
            return 0;
        }
    }
}
