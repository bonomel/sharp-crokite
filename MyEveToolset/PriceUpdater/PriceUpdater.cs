using MyEveToolset.Data;
using MyEveToolset.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyEveToolset.PriceUpdater
{
    public class PriceUpdater
    {
        private readonly MyEveToolDbContext dbContext;

        public PriceUpdater(MyEveToolDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal void Update(IList<PriceDto> priceDtos)
        {
            foreach(PriceDto dto in priceDtos)
            {
                Harvestable harvestable = dbContext.Find<Harvestable>(dto.TypeId);
                Material material = dbContext.Find<Material>(dto.TypeId);

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
                        material.Prices.Add(new Price()
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

            dbContext.SaveChanges();
        }
    }
}
