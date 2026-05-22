using RESTAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RESTAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PC> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PC>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Weight).HasColumnType("float(5)").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.FoundationDate).HasColumnType("date").IsRequired();
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasColumnType("char(10)").IsRequired();
            entity.Property(e => e.Name).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Description).HasColumnType("nvarchar(max)").IsRequired();
            
            entity.HasOne(c => c.Manufacturer)
                  .WithMany(m => m.Components)
                  .HasForeignKey(c => c.ComponentManufacturersId);
                  
            entity.HasOne(c => c.Type)
                  .WithMany(t => t.Components)
                  .HasForeignKey(c => c.ComponentTypesId);
        });

        modelBuilder.Entity<PCComponent>(entity =>
        {
            entity.HasKey(e => new { e.PCId, e.ComponentCode });
            entity.Property(e => e.ComponentCode).HasColumnType("char(10)");

            entity.HasOne(pc => pc.PC)
                  .WithMany(p => p.PCComponents)
                  .HasForeignKey(pc => pc.PCId);

            entity.HasOne(pc => pc.Component)
                  .WithMany(c => c.PCComponents)
                  .HasForeignKey(pc => pc.ComponentCode);
        });

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "INT", FullName = "Intel Corporation", FoundationDate = new DateTime(1968, 7, 18) },
            new ComponentManufacturer { Id = 2, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateTime(1969, 5, 1) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Components", FoundationDate = new DateTime(1994, 1, 1) }
        );

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Central Processing Unit" },
            new ComponentType { Id = 2, Abbreviation = "RAM", Name = "Random Access Memory" },
            new ComponentType { Id = 3, Abbreviation = "PSU", Name = "Power Supply Unit" }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "COMP-00001", Name = "Intel Core i9", Description = "High-end processor", ComponentManufacturersId = 1, ComponentTypesId = 1 },
            new Component { Code = "COMP-00002", Name = "AMD Ryzen 9", Description = "High-end AMD processor", ComponentManufacturersId = 2, ComponentTypesId = 1 },
            new Component { Code = "COMP-00003", Name = "Corsair Vengeance 32GB", Description = "Fast memory", ComponentManufacturersId = 3, ComponentTypesId = 2 }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC { Id = 1, Name = "Gaming Beast X", Weight = 12.5f, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new PC { Id = 2, Name = "Office Mini Pro", Weight = 4.2f, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new PC { Id = 3, Name = "Home Media Center", Weight = 8.0f, Warranty = 12, CreatedAt = new DateTime(2026, 5, 20, 10, 0, 0), Stock = 2 }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "COMP-00001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "COMP-00003", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "COMP-00002", Amount = 1 }
        );
    }
}