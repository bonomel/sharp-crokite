﻿namespace SharpCrokite.Core.Models
{
    public class HarvestableModel
    {
        public int HarvestableId { get; internal set; }
        public byte[] Icon { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string Price { get; internal set; }
        public string MaterialContents { get; internal set; }
        public string Description { get; internal set; }
        public HarvestableModel IsCompressedVariantOfType { get; internal set; }
        public decimal Volume { get; internal set; }
    }
}
