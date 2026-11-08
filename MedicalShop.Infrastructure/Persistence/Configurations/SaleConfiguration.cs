using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
    

    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(x => x.SaleId);

            builder.Property(x => x.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.InvoiceNumber)
                .IsUnique();

            builder.Property(x => x.SaleDate)
                .IsRequired();

            builder.Property(x => x.SubTotal)
                .HasPrecision(18, 2);

            builder.Property(x => x.GSTAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.DiscountAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.NetAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.PaymentMethod)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SaleDetails)
                .WithOne(x => x.Sale)
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(s => s.AmountPaid)
                .HasPrecision(18, 2);

            builder.Property(s => s.BalanceDue)
                .HasPrecision(18, 2);

            builder.Property(s => s.PaymentStatus)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
