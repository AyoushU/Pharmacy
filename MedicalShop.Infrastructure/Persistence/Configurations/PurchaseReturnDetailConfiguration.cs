using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{

    public class PurchaseReturnDetailConfiguration : IEntityTypeConfiguration<PurchaseReturnDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseReturnDetail> builder)
        {
            builder.ToTable("PurchaseReturnDetails");

            builder.HasKey(x => x.PurchaseReturnDetailId);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.PurchasePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.GSTPercentage)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.GSTAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            // PurchaseReturn → PurchaseReturnDetails
            builder.HasOne(x => x.PurchaseReturn)
                .WithMany(x => x.PurchaseReturnDetails)
                .HasForeignKey(x => x.PurchaseReturnId)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseReturnDetail → Original PurchaseDetail
            builder.HasOne(x => x.PurchaseDetail)
                .WithMany()
                .HasForeignKey(x => x.PurchaseDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseReturnDetail → Product
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseReturnDetail → Stock
            builder.HasOne(x => x.Stock)
                .WithMany()
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PurchaseReturnId);
            builder.HasIndex(x => x.PurchaseDetailId);
            builder.HasIndex(x => x.StockId);
        }
    }

}
