using global::MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{


    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchases");

            builder.HasKey(x => x.PurchaseId);

            builder.Property(x => x.PurchaseNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.PurchaseNumber)
                .IsUnique();

            builder.Property(x => x.PurchaseDate)
                .IsRequired();

            builder.Property(x => x.InvoiceNumber)
                .HasMaxLength(50);

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.GSTAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.NetAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(30)
                .IsRequired();

            // Supplier → Purchase
            builder.HasOne(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Purchase → PurchaseDetails
            builder.HasMany(x => x.PurchaseDetails)
                .WithOne(x => x.Purchase)
                .HasForeignKey(x => x.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
