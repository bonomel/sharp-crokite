namespace MyEveToolset.StaticDataUpdater.JSONModels
{
    internal interface IGroupJSON
    {
        int category_id { get; }
        int group_id { get; }
        string name { get; }
        bool published { get; }
        int[] types { get; }
    }
}