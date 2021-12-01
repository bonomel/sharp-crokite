// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
namespace SharpCrokite.Core.StaticDataUpdater.Esi.EsiJsonModels
{
    public class EsiCategoryJson
    {

        public int category_id { get; set; }
        public int[] groups { get; set; }
        public string name { get; set; }
        public bool published { get; set; }
    }
}
