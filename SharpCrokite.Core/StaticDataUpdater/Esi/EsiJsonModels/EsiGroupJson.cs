// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
namespace SharpCrokite.Core.StaticDataUpdater.Esi.EsiJsonModels
{
    public class EsiGroupJson
    {
        public int category_id { get; set; }
        public int group_id { get; set; }
        public string name { get; set; }
        public bool published { get; set; }
        public int[] types { get; set; }
    }
}
