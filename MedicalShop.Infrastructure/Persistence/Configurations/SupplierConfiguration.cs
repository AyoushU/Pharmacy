
using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MedicalShop.Infrastructure.Persistence.Configurations
{

    public class SupplierConfiguration
        : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(
            EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(x => x.SupplierId);

            builder.Property(x => x.SupplierCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.SupplierCode)
                .IsUnique();

            builder.Property(x => x.SupplierName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.ContactPerson)
                .HasMaxLength(150);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.Email)
                .HasMaxLength(150);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.GSTNumber)
                .HasMaxLength(15);

            builder.Property(x => x.OpeningBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.HasIndex(x => x.SupplierName);
            builder.HasIndex(x => x.GSTNumber);
        }
    }
}
