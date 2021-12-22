using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class StaticDataUpdater
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public StaticDataUpdater(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        internal void UpdateHarvestables(IEnumerable<HarvestableDto> harvestableDtos)
        {
            foreach (HarvestableDto harvestableDto in harvestableDtos)
            {
                Harvestable existingHarvestable = harvestableRepository.Get(harvestableDto.HarvestableId);

                if (existingHarvestable != null)
                {
                    existingHarvestable.Name = harvestableDto.Name;
                    existingHarvestable.Type = harvestableDto.Type;
                    existingHarvestable.Description = harvestableDto.Description;
                    existingHarvestable.Volume = harvestableDto.Volume;
                    existingHarvestable.Icon = harvestableDto.Icon;
                    existingHarvestable.IsCompressedVariantOfType = harvestableDto.IsCompressedVariantOfType;
                    existingHarvestable.MaterialContents = harvestableDto.MaterialContents.Select(dto => new MaterialContent()
                    {
                        HarvestableId = dto.HarvestableId,
                        Material = materialRepository.Get(dto.MaterialId),
                        Quantity = dto.Quantity
                    }).ToList();

                    harvestableRepository.Update(existingHarvestable);
                }
                else
                {
                    harvestableRepository.Add(new Harvestable()
                    {
                        HarvestableId = harvestableDto.HarvestableId,
                        Name = harvestableDto.Name,
                        Type = harvestableDto.Type,
                        Description = harvestableDto.Description,
                        Volume = harvestableDto.Volume,
                        Icon = harvestableDto.Icon,
                        IsCompressedVariantOfType = harvestableDto.IsCompressedVariantOfType,
                        MaterialContents = harvestableDto.MaterialContents.Select(dto => new MaterialContent()
                        {
                            HarvestableId = dto.HarvestableId,
                            Material = materialRepository.Get(dto.MaterialId),
                            Quantity = dto.Quantity
                        }).ToList()
                    });
                }
            }

            harvestableRepository.SaveChanges();
        }

        internal void UpdateMaterials(IEnumerable<MaterialDto> materialDtos)
        {
            foreach (MaterialDto materialDto in materialDtos)
            {
                Material existingMaterial = materialRepository.Get(materialDto.MaterialId);

                if (existingMaterial != null)
                {
                    existingMaterial.Name = materialDto.Name;
                    existingMaterial.Type = materialDto.Type;
                    existingMaterial.Description = materialDto.Description;
                    existingMaterial.Icon = materialDto.Icon;
                    existingMaterial.Quality = materialDto.Quality;

                    materialRepository.Update(existingMaterial);
                }
                else
                {
                    materialRepository.Add(new Material()
                    {
                        MaterialId = materialDto.MaterialId,
                        Name = materialDto.Name,
                        Type = materialDto.Type,
                        Description = materialDto.Description,
                        Icon = materialDto.Icon,
                        Quality = materialDto.Quality
                    });
                }
            }

            materialRepository.SaveChanges();
        }
    }
}