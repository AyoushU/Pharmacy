using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalShop.Infrastructure.Persistence.Configurations
{
    public class StockLedgerConfiguration
        : IEntityTypeConfiguration<StockLedger>
    {
        public void Configure(EntityTypeBuilder<StockLedger> builder)
        {
            builder.ToTable("StockLedger");

            builder.HasKey(x => x.StockLedgerId);

            builder.Property(x => x.TransactionType)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.BalanceQuantity)
                .IsRequired();

            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(50);

            builder.Property(x => x.Remarks)
                .HasMaxLength(250);

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            // StockLedger → Product
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // StockLedger → Stock
            builder.HasOne(x => x.Stock)
                .WithMany()
                .HasForeignKey(x => x.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            // Useful for stock history queries
            builder.HasIndex(x => new
            {
                x.ProductId,
                x.TransactionDate
            });
        }
    }
}
