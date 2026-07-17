using FoodDelivery.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Api.Data;

public class FoodDeliveryDbContext : DbContext
{
    public FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(o => o.Id);

            entity.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(o => o.CustomerPhone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(o => o.FoodItem)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(o => o.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(o => o.Quantity)
                .IsRequired();

            entity.Property(o => o.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(o => o.OrderDate)
                .IsRequired();
        });

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CustomerName = "Asha Verma",
                CustomerPhone = "9876543210",
                FoodItem = "Veg Burger",
                Quantity = 2,
                Price = 240.00m,
                DeliveryAddress = "12 Market Road, Hyderabad",
                Status = OrderStatus.Placed,
                OrderDate = new DateTime(2026, 7, 17, 10, 0, 0, DateTimeKind.Utc)
            },
            new Order
            {
                Id = 2,
                CustomerName = "Rahul Reddy",
                CustomerPhone = "9123456780",
                FoodItem = "Chicken Biryani",
                Quantity = 1,
                Price = 320.00m,
                DeliveryAddress = "5 Lake View, Secunderabad",
                Status = OrderStatus.Preparing,
                OrderDate = new DateTime(2026, 7, 17, 11, 30, 0, DateTimeKind.Utc)
            });
    }
}
