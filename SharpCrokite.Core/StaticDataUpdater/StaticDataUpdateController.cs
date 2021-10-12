using SharpCrokite.Core.StaticDataUpdater.JSONModels;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System.Collections.Generic;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class StaticDataUpdateController
    {
        private readonly IRepository<Harvestable> harvestableRepository;
        private readonly IRepository<Material> materialRepository;
        private readonly StaticDataRetriever dataRetriever;
        private readonly EsiJSONToDataModelConverter converter;

        public StaticDataUpdateController(StaticDataRetriever dataRetriever, EsiJSONToDataModelConverter converter,
            IRepository<Harvestable> harvestableRepository, IRepository<Material> materialRepository)
        {
            this.dataRetriever = dataRetriever;
            this.converter = converter;
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
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
                Material existingMaterial = materialRepository.Get(material.MaterialId);
                byte[] icon = dataRetriever.GetIconForTypeId(material.MaterialId);

                if (existingMaterial != null)
                {
                    existingMaterial.Icon = icon;
                    materialRepository.Update(material);
                }
                else
                {
                    material.Icon = icon;
                    materialRepository.Add(material);
                }
            }

            foreach (Harvestable harvestable in harvestables)
            {
                Harvestable existingHarvestable = harvestableRepository.Get(harvestable.HarvestableId);
                byte[] icon = dataRetriever.GetIconForTypeId(harvestable.HarvestableId);

                if (existingHarvestable != null)
                {
                    harvestable.Icon = icon;
                    harvestableRepository.Update(harvestable);
                }
                else
                {
                    harvestable.Icon = icon;
                    harvestableRepository.Add(harvestable);
                }
            }

            harvestableRepository.SaveChanges();
            materialRepository.SaveChanges();
        }

        public void DeleteAllStaticData()
        {
            harvestableRepository.DeleteAll();
            materialRepository.DeleteAll();

            harvestableRepository.SaveChanges();
            materialRepository.SaveChanges();
        }
    }
}
