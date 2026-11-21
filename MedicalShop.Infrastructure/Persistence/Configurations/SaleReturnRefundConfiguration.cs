using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
    public class SaleReturnRefundConfiguration : IEntityTypeConfiguration<SaleReturnRefund>
        {
            public void Configure(EntityTypeBuilder<SaleReturnRefund> builder)
            {
                builder.HasKey(x => x.SaleReturnRefundId);

                builder.Property(x => x.Amount)
                    .HasPrecision(18, 2);

                builder.Property(x => x.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(30);

                builder.Property(x => x.ReferenceNumber)
                    .HasMaxLength(100);

                builder.Property(x => x.Remarks)
                    .HasMaxLength(500);

                builder.HasOne(x => x.SaleReturn)
                    .WithMany()
                    .HasForeignKey(x => x.SaleReturnId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasIndex(x => x.SaleReturnId);
            }
        }
    }


