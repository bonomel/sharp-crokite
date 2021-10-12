//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using System;
//using System.Collections.Generic;

//namespace SharpCrokite.Infrastructure
//{
//    public interface ISharpCrokiteDbContext
//    { 
//        public DbSet<IHarvestable> Harvestables { get; set; }
//        public DbSet<IMaterialContent> MaterialContents { get; set; }
//        public DbSet<IMaterial> Materials { get; set; }
//        public DbSet<IPrice> Prices { get; set; }
//        public int SaveChanges();
//        public EntityEntry Add(object entity);
//        public object Find(Type entityType, params object[] keyValues);
//        public TEntity Find<TEntity>(params object[] keyValues);
//    }

//    public interface IMaterial
//    {
//        public int MaterialId { get; set; }
//        public string Type { get; set; }
//        public string Name { get; set; }
//        public byte[] Icon { get; set; }
//        public string Description { get; set; }
//        public IEnumerable<IPrice> Prices { get; set; }
//    }

//    public interface IPrice
//    {
//        public int PriceId { get; set; }
//        public IMaterial Material { get; set; }
//        public IHarvestable Harvestable { get; set; }
//        public int SystemId { get; set; }
//        public decimal BuyMax { get; set; }
//        public decimal BuyMin { get; set; }
//        public decimal BuyPercentile { get; set; }
//        public decimal SellMax { get; set; }
//        public decimal SellMin { get; set; }
//        public decimal SellPercentile { get; set; }
//    }

//    public interface IHarvestable
//    {
//        public int HarvestableId { get; set; }
//        public string Type { get; set; }
//        public byte[] Icon { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public IEnumerable<IMaterialContent> MaterialContents { get; set; }
//        public IEnumerable<IPrice> Prices { get; set; }
//        public int? IsCompressedVariantOfType { get; set; }
//    }

//    public interface IMaterialContent
//    {
//        public int MaterialContentId { get; set; }
//        public int HarvestableId { get; set; }
//        public int MaterialId { get; set; }
//        public int Quantity { get; set; }
//    }
//}
