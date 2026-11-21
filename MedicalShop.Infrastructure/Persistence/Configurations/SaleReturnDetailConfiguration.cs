using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
        public class SaleReturnDetailConfiguration : IEntityTypeConfiguration<SaleReturnDetail>
        {
            public void Configure(EntityTypeBuilder<SaleReturnDetail> builder)
            {
                builder.HasKey(x => x.SaleReturnDetailId);

                builder.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2);

                builder.Property(x => x.GSTPercentage)
                    .HasPrecision(5, 2);

                builder.Property(x => x.GSTAmount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.DiscountAmount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.TotalAmount)
                    .HasPrecision(18, 2);

                builder.HasOne(x => x.SaleDetail)
                    .WithMany()
                    .HasForeignKey(x => x.SaleDetailId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(x => x.Stock)
                    .WithMany()
                    .HasForeignKey(x => x.StockId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasIndex(x => x.SaleDetailId);
                builder.HasIndex(x => x.StockId);
            }
        }
    }

