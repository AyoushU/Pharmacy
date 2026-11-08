using global::MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
 
    public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
        {
            builder.ToTable("PurchaseDetails");

            builder.HasKey(x => x.PurchaseDetailId);

            builder.Property(x => x.BatchNumber)
                .HasMaxLength(50);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.PurchasePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.GSTPercentage)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            // PurchaseDetail → Product
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
