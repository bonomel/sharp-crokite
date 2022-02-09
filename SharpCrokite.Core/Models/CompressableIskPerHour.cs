using System.Collections.Generic;
using SharpCrokite.Infrastructure.Common;

namespace SharpCrokite.Core.Models
{
    public abstract class CompressableIskPerHour : HarvestableIskPerHour
    {
        internal Dictionary<int, Isk> CompressedPrices { get; set; }

        private Isk compressedIskPerHour = new(0);
        public Isk CompressedIskPerHour
        {
            get => compressedIskPerHour;
            internal set
            {
                if (compressedIskPerHour != value)
                {
                    compressedIskPerHour = value;
                    NotifyPropertyChanged(nameof(CompressedIskPerHour));
                }
            }
        }
    }
}
