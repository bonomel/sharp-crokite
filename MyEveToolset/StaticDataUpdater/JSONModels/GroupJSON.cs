namespace MyEveToolset.StaticDataUpdater.JSONModels
{
    public class GroupJSON : IGroupJSON
    {
        public int category_id { get; set; }
        public int group_id { get; set; }
        public string name { get; set; }
        public bool published { get; set; }
        public int[] types { get; set; }
    }
}
