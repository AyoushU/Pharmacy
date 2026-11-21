using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
        public class SaleReturnConfiguration : IEntityTypeConfiguration<SaleReturn>
        {
            public void Configure(EntityTypeBuilder<SaleReturn> builder)
            {
                builder.HasKey(x => x.SaleReturnId);

                builder.Property(x => x.ReturnNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.HasIndex(x => x.ReturnNumber)
                    .IsUnique();

                builder.Property(x => x.Reason)
                    .HasMaxLength(500);

                builder.Property(x => x.SubTotal)
                    .HasPrecision(18, 2);

                builder.Property(x => x.GSTAmount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.NetAmount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.RefundAmount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.PendingRefundAmount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.RefundStatus)
                    .IsRequired()
                    .HasMaxLength(30);

                builder.Property(x => x.Status)
                    .IsRequired()
                    .HasMaxLength(30);

                builder.HasOne(x => x.Sale)
                    .WithMany()
                    .HasForeignKey(x => x.SaleId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasMany(x => x.SaleReturnDetails)
                    .WithOne(x => x.SaleReturn)
                    .HasForeignKey(x => x.SaleReturnId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }


