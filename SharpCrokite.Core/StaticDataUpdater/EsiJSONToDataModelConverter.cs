using SharpCrokite.Core.StaticDataUpdater.JSONModels;
using SharpCrokite.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public static class EsiJSONToDataModelConverter
    {
        internal static IEnumerable<Material> CreateMaterialsFromJSON(IEnumerable<IEnumerable<ITypeJSON>> materialTypesPerGroup)
        {
            List<Material> materials = new();

            foreach (IEnumerable<ITypeJSON> group in materialTypesPerGroup)
            {
                foreach (ITypeJSON type in group)
                {
                    materials.Add(
                        new Material
                        {
                            MaterialId = type.type_id,
                            Name = type.name,
                            Description = RemoveHtmlFromString(type.description)
                        }
                    );
                }
            }

            return materials;
        }

        internal static IEnumerable<Harvestable> CreateHarvestablesFromJSON(IEnumerable<IEnumerable<ITypeJSON>> asteroidTypesPerGroup,
            IEnumerable<IMaterialContentJSON> materialContent)
        {
            List<Harvestable> harvestables = new();
            List<IMaterialContentJSON> materials = materialContent.ToList();

            foreach(IEnumerable<ITypeJSON> group in asteroidTypesPerGroup)
            {
                foreach(ITypeJSON type in group)
                {
                    List<MaterialContent> materialContents = new();

                    var harvestable = new Harvestable
                    {
                        HarvestableId = type.type_id,
                        Name = type.name,
                        Description = RemoveHtmlFromString(type.description),
                        MaterialContents = materials
                            .Where(m => m.typeID == type.type_id)
                            .Select(m => new MaterialContent
                            {
                                HarvestableId = type.type_id,
                                MaterialId = m.materialTypeID,
                                Quantity = m.quantity
                            }
                            ).ToList()
                    };

                    harvestables.Add(harvestable);
                }
            }

            SetCompressedVariantIds(harvestables);

            return harvestables;
        }

        private static string RemoveHtmlFromString(string stringToRemoveFrom)
        {
            return Regex.Replace(stringToRemoveFrom, "<.*?>", string.Empty);
        }

        private static void SetCompressedVariantIds(List<Harvestable> harvestables)
        {
            foreach(var harvestable in harvestables)
            {
                if(harvestable.Name.Contains("Compressed"))
                {
                    string lookupString = harvestable.Name.Replace("Compressed ", "");

                    harvestable.IsCompressedVariantOfType = harvestables.Single(h => h.Name == lookupString).HarvestableId;
                }
            }
        }
    }
}