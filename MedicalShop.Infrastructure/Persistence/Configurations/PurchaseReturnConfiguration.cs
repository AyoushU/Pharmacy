using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{

    public class PurchaseReturnConfiguration : IEntityTypeConfiguration<PurchaseReturn>
    {
        public void Configure(EntityTypeBuilder<PurchaseReturn> builder)
        {
            builder.ToTable("PurchaseReturns");

            builder.HasKey(x => x.PurchaseReturnId);

            builder.Property(x => x.ReturnNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.ReturnNumber)
                .IsUnique();

            builder.Property(x => x.ReturnDate)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

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

            // Purchase → PurchaseReturns
            builder.HasOne(x => x.Purchase)
                .WithMany()
                .HasForeignKey(x => x.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.PurchaseId);
        }
    }

}
