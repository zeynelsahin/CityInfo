using Bogus;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoContext : DbContext
{
    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlite("Data Source=CityInfo.db");
        // base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasKey(c => c.Id);
        modelBuilder.Entity<City>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<PointOfInterest>().HasKey(c => c.Id);
        modelBuilder.Entity<PointOfInterest>().Property(p => p.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<City>().HasData(
            new City("Trabzon") { Id = 1, Description = "Random string " },
            new City("Hatay") { Id = 2, Description = "Random string " },
            new City("İstanbul") { Id = 3, Description = "Random string " }
        );
        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Uzun Göl") { Id = 1, CityId = 1, Description = "Random string " },
            new PointOfInterest("İskenderun") { Id = 2, CityId = 2, Description = "Random string " },
            new PointOfInterest("Ayasofya Cami") { Id = 3, CityId = 3, Description = "Random string " },
            new PointOfInterest("Selimiye Cami") { Id = 4, CityId = 3, Description = "Random string " },
            new PointOfInterest("Sultan Ahmet Cami") { Id = 5, CityId = 3, Description = "Random string " },
            new PointOfInterest("Sümela Manastırı") { Id = 6, CityId = 1, Description = "Random string " }
        );

        base.OnModelCreating(modelBuilder);
    }
}