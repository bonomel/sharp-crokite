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
        private readonly EsiStaticDataRetriever dataRetriever;

        public StaticDataUpdateController(EsiStaticDataRetriever dataRetriever, IRepository<Harvestable> harvestableRepository, IRepository<Material> materialRepository)
        {
            this.dataRetriever = dataRetriever;
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        public void UpdateData()
        {
            IEnumerable<IEnumerable<ITypeJSON>> asteroidTypesPerGroup = dataRetriever.RetrieveAsteroidTypesPerGroup();
            IEnumerable<IEnumerable<ITypeJSON>> materialTypes = dataRetriever.RetrieveMaterialTypesPerGroup();
            IEnumerable<IMaterialContentJSON> materialContent = dataRetriever.RetrieveMaterialContent();

            IEnumerable<Material> materials = EsiJSONToDataModelConverter.CreateMaterialsFromJSON(materialTypes);
            IEnumerable<Harvestable> harvestables = EsiJSONToDataModelConverter.CreateHarvestablesFromJSON(asteroidTypesPerGroup, materialContent);

            UpdateHarvestables(harvestables);
            UpdateMaterial(materials);
        }

        private void UpdateHarvestables(IEnumerable<Harvestable> harvestables)
        {
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
        }
        private void UpdateMaterial(IEnumerable<Material> materials)
        {
            foreach (Material material in materials)
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
