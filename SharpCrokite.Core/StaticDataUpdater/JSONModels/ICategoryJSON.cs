namespace MyEveToolset.StaticDataUpdater.JSONModels
{
    public interface ICategoryJSON
    {
        int category_id { get; }
        int[] groups { get; }
        string name { get; }
        bool published { get; }
    }
}