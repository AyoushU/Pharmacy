using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{

    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks");

            builder.HasKey(x => x.StockId);

            builder.Property(x => x.BatchNumber)
                .HasMaxLength(50);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.PurchasePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.SellingPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            // Stock → Product
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Same product can have multiple batches
            builder.HasIndex(x => new
            {
                x.ProductId,
                x.BatchNumber
            });
        }
    }
}
