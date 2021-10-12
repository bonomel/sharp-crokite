namespace SharpCrokite.Core.StaticDataUpdater.JSONModels
{
    public interface ITypeJSON
    {
        int type_id { get; }
        float capacity { get; }
        string description { get; }
        int group_id { get; }
        int icon_id { get; }
        int market_group_id { get; }
        double mass { get; }
        string name { get; }
        decimal packaged_volume { get; }
        int portion_size { get; }
        bool published { get; }
        decimal radius { get; }
        decimal volume { get; }
    }
}
