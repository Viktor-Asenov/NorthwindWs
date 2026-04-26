using Microsoft.EntityFrameworkCore;
using NorthwindWs.Data.Entities;

namespace NorthwindWs.Data;

public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(e => e.CustomerId);
            entity.Property(e => e.CustomerId).HasMaxLength(5);
            entity.Property(e => e.CompanyName).HasMaxLength(40).IsRequired();
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.Region).HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.Fax).HasMaxLength(24);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.CustomerId).HasMaxLength(5);
            entity.Property(e => e.Freight).HasColumnType("money");
            entity.Property(e => e.ShipName).HasMaxLength(40);
            entity.Property(e => e.ShipAddress).HasMaxLength(60);
            entity.Property(e => e.ShipCity).HasMaxLength(15);
            entity.Property(e => e.ShipRegion).HasMaxLength(15);
            entity.Property(e => e.ShipPostalCode).HasMaxLength(10);
            entity.Property(e => e.ShipCountry).HasMaxLength(15);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CustomerId);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("Order Details");
            entity.HasKey(e => new { e.OrderId, e.ProductId });
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(e => e.OrderId);

            entity.HasOne(e => e.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(e => e.ProductId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.ProductName).HasMaxLength(40).IsRequired();
            entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("money");
        });
    }
}
