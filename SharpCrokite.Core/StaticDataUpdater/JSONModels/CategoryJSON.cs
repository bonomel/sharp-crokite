﻿namespace SharpCrokite.Core.StaticDataUpdater.JSONModels
{
    public class CategoryJSON
    {
        public int category_id { get; set; }
        public int[] groups { get; set; }
        public string name { get; set; }
        public bool published { get; set; }
    }
}
