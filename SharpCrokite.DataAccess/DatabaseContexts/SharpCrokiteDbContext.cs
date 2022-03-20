using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using SharpCrokite.DataAccess.Models;

namespace SharpCrokite.DataAccess.DatabaseContexts
{
    public class SharpCrokiteDbContext : DbContext
    {
        public DbSet<Harvestable> Harvestables { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<MaterialContent> MaterialContents { get; set; }

        private string DbPath { get; }

        public SharpCrokiteDbContext()
        {
#if DEBUG
            DbPath = $"{Path.Combine(TryGetSolutionDirectoryInfo().FullName)}\\SharpCrokite.DataAccess\\SharpCrokite-Debug.SQLite.db";
#else
            DbPath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory)}\\SharpCrokite.SQLite.db";
#endif
        }

        private static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            _ = modelBuilder.Entity<Harvestable>().HasMany(h => h.Prices).WithOne(p => p.Harvestable).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            _ = modelBuilder.Entity<Material>().HasMany(m => m.Prices).WithOne(p => p.Material).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            _ = modelBuilder.Entity<MaterialContent>().HasOne(m => m.Material).WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
        }

        public void RunMigrations()
        {
            Database.Migrate();
        }
    }
}
