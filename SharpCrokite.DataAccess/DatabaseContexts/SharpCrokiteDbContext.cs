using Microsoft.EntityFrameworkCore;

using SharpCrokite.DataAccess.Models;

namespace SharpCrokite.DataAccess.DatabaseContexts
{
    public class SharpCrokiteDbContext : DbContext
    {
        public DbSet<Harvestable> Harvestables { get; set; }
        public DbSet<MaterialContent> MaterialContents { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Price> Prices { get; set; }

        public string DbPath { get; set; }

        public SharpCrokiteDbContext()
        {
            DbPath = @"C:\Projects\sharp-crokite\SharpCrokite.DataAccess\myevetool.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Harvestable>().HasMany(h => h.Prices).WithOne(p => p.Harvestable).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            modelBuilder.Entity<Material>().HasMany(m => m.Prices).WithOne(p => p.Material).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            modelBuilder.Entity<MaterialContent>().HasOne(m => m.Material).WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
        }
    }
}
