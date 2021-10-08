using MyEveToolset.Data;
using MyEveToolset.Data.Models;
using MyEveToolset.StaticDataUpdater.JSONModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEveToolset.StaticDataUpdater
{
    public class StaticDataUpdateController
    {
        private readonly MyEveToolDbContext dbContext;
        private readonly StaticDataRetriever dataRetriever;
        private readonly EsiJSONToDataModelConverter converter;

        public StaticDataUpdateController(MyEveToolDbContext dbContext, StaticDataRetriever dataRetriever, EsiJSONToDataModelConverter converter)
        {
            this.dbContext = dbContext;
            this.dataRetriever = dataRetriever;
            this.converter = converter;
        }

        public void DeleteData()
        {
            dbContext.Materials.RemoveRange(dbContext.Materials);
            dbContext.Harvestables.RemoveRange(dbContext.Harvestables);
            dbContext.SaveChanges();
        }

        public void UpdateData()
        {
            // ASYNC TEST
            //var task = new Task<IEnumerable<IEnumerable<ITypeJSON>>>(() => dataRetriever.RetrieveAsteroidTypesPerGroup());
            //task.Start();
            //await task;
            //IEnumerable<IEnumerable<ITypeJSON>> asteroidTypesPerGroup = task.Result;
            // END ASYNC TEST

            IEnumerable<IEnumerable<ITypeJSON>> asteroidTypesPerGroup = dataRetriever.RetrieveAsteroidTypesPerGroup();

            IEnumerable<IEnumerable<ITypeJSON>> materialTypes = dataRetriever.RetrieveMaterialTypesPerGroup();

            IEnumerable<IMaterialContentJSON> materialContent = dataRetriever.RetrieveMaterialContent();

            IEnumerable<Material> materials = converter.CreateMaterialsFromJSON(materialTypes);
            IEnumerable<Harvestable> harvestables = converter.CreateHarvestablesFromJSON(asteroidTypesPerGroup, materialContent);

            foreach(Material material in materials)
            {
                Material existingMaterial = dbContext.Materials.Find(material.MaterialId);
                byte[] icon = dataRetriever.GetIconForTypeId(material.MaterialId);

                if (existingMaterial != null)
                {
                    existingMaterial.Name = material.Name;
                    existingMaterial.Description = material.Description;
                    existingMaterial.Icon = icon;
                }
                else
                {
                    material.Icon = icon;
                    dbContext.Add(material);
                }
            }

            foreach (Harvestable harvestable in harvestables)
            {
                Harvestable existingHarvestable = dbContext.Harvestables.Find(harvestable.HarvestableId);
                byte[] icon = dataRetriever.GetIconForTypeId(harvestable.HarvestableId);

                if (existingHarvestable != null)
                {
                    existingHarvestable.Name = harvestable.Name;
                    existingHarvestable.Description = harvestable.Description;
                    existingHarvestable.IsCompressedVariantOfType = harvestable.IsCompressedVariantOfType;
                    existingHarvestable.MaterialContents = harvestable.MaterialContents;
                    existingHarvestable.Icon = icon;
                }
                else
                {
                    harvestable.Icon = icon;
                    dbContext.Add(harvestable);
                }
            }

            dbContext.SaveChanges();
        }
    }
}
