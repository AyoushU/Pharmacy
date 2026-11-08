using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{

    public class CustomerConfiguration
        : IEntityTypeConfiguration<Customer>
    {
        public void Configure(
            EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(x => x.CustomerId);

            builder.Property(x => x.CustomerCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.CustomerCode)
                .IsUnique();

            builder.Property(x => x.CustomerName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.Email)
                .HasMaxLength(150);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.OpeningBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.HasIndex(x => x.CustomerName);
            builder.HasIndex(x => x.Phone);
        }
    }
}
