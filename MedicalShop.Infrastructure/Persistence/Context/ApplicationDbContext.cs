using MedicalShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalShop.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();
        public DbSet<Stock> Stocks => Set<Stock>();

        public DbSet<StockLedger> StockLedgers => Set<StockLedger>();

        public DbSet<Sale> Sales => Set<Sale>();

        public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }

        public DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }

        public DbSet<SaleReturn> SaleReturns { get; set; }

        public DbSet<SaleReturnDetail> SaleReturnDetails { get; set; }
        public DbSet<SaleReturnRefund> SaleReturnRefunds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasSequence<int>("PurchaseReturnNumberSequence")
             .StartsAt(1)
             .IncrementsBy(1);
            modelBuilder.HasSequence<int>("SaleReturnNumberSequence")
             .StartsAt(1)
             .IncrementsBy(1);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}
