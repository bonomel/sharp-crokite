﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpCrokite.DataAccess.Models;
using SharpCrokite.Infrastructure.Repositories;

namespace SharpCrokite.Core.PriceUpdater
{
    public class PriceUpdater
    {
        private readonly HarvestableRepository harvestableRepository;
        private readonly MaterialRepository materialRepository;

        public PriceUpdater(HarvestableRepository harvestableRepository, MaterialRepository materialRepository)
        {
            this.harvestableRepository = harvestableRepository;
            this.materialRepository = materialRepository;
        }

        internal async Task Update(IEnumerable<PriceDto> priceDtos)
        {
            foreach(PriceDto dto in priceDtos)
            {
                Harvestable harvestable = await harvestableRepository.GetAsync(dto.TypeId);
                Material material = await materialRepository.GetAsync(dto.TypeId);

                if(harvestable != null)
                {
                    var currentSystemPrices = harvestable.Prices.SingleOrDefault(p => p.SystemId == dto.SystemId);

                    if(currentSystemPrices != null)
                    {
                        currentSystemPrices.BuyMax = dto.BuyMax;
                        currentSystemPrices.BuyMin = dto.BuyMin;
                        currentSystemPrices.BuyPercentile = dto.BuyPercentile;
                        currentSystemPrices.SellMax = dto.SellMax;
                        currentSystemPrices.SellMin = dto.SellMin;
                        currentSystemPrices.SellPercentile = dto.SellPercentile;
                    }
                    else
                    {
                        harvestable.Prices.Add(new Price()
                        {
                            SystemId = dto.SystemId,
                            BuyMax = dto.BuyMax,
                            BuyMin = dto.BuyMin,
                            BuyPercentile = dto.BuyPercentile,
                            SellMax = dto.SellMax,
                            SellMin = dto.SellMin,
                            SellPercentile = dto.SellPercentile
                        });
                    }
                }
                else if(material != null)
                {
                    var currentSystemPrices = material.Prices.SingleOrDefault(p => p.SystemId == dto.SystemId);

                    if (currentSystemPrices != null)
                    {
                        currentSystemPrices.BuyMax = dto.BuyMax;
                        currentSystemPrices.BuyMin = dto.BuyMin;
                        currentSystemPrices.BuyPercentile = dto.BuyPercentile;
                        currentSystemPrices.SellMax = dto.SellMax;
                        currentSystemPrices.SellMin = dto.SellMin;
                        currentSystemPrices.SellPercentile = dto.SellPercentile;
                    }
                    else
                    {
                        material.Prices.Add(new Price
                        {
                            SystemId = dto.SystemId,
                            BuyMax = dto.BuyMax,
                            BuyMin = dto.BuyMin,
                            BuyPercentile = dto.BuyPercentile,
                            SellMax = dto.SellMax,
                            SellMin = dto.SellMin,
                            SellPercentile = dto.SellPercentile
                        });
                    }
                }
            }

            await harvestableRepository.SaveChangesAsync();
            await materialRepository.SaveChangesAsync();
        }
    }
}
