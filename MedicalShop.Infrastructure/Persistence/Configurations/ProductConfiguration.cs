using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration  : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.ProductCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.ProductCode)
                .IsUnique();

            builder.Property(x => x.ProductName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Manufacturer)
                .HasMaxLength(150);

            builder.Property(x => x.BatchNumber)
                .HasMaxLength(50);

            builder.Property(x => x.PurchasePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.SellingPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.GSTPercentage)
                .HasPrecision(5, 2);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ProductName);

            builder.HasIndex(x => x.ExpiryDate);
        }
    }
}
