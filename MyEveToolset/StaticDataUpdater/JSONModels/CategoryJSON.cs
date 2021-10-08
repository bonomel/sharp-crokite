namespace MyEveToolset.StaticDataUpdater.JSONModels
{
    public class CategoryJSON : ICategoryJSON
    {
        public int category_id { get; set; }
        public int[] groups { get; set; }
        public string name { get; set; }
        public bool published { get; set; }
    }
}
