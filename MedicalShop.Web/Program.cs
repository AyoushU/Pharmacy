using Azure.Identity;
using MedicalShop.Application.Interfaces;
using MedicalShop.Application.Services;
using MedicalShop.Infrastructure.Identity;
using MedicalShop.Infrastructure.Persistence.Context;
using MedicalShop.Infrastructure.Persistence.Repositories;
using MedicalShop.Infrastructure.Persistence.unitOfWork;
using MedicalShop.Infrastructure.Repositories;
using MedicalShop.Web.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection was not found.");
// Azure Key Vault - Production Only
//if (builder.Environment.IsProduction())
//{
//    var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];

//    if (!string.IsNullOrWhiteSpace(keyVaultUri))
//    {
//        builder.Configuration.AddAzureKeyVault(
//            new Uri(keyVaultUri),
//            new ManagedIdentityCredential());
   // }

// End

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();

builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockLedgerRepository, StockLedgerRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPurchaseReturnRepository, PurchaseReturnRepository>(); //new code for purchase return
builder.Services.AddScoped<IPurchaseReturnService, PurchaseReturnService>(); // new code
builder.Services.AddScoped<ISaleReturnRepository, SaleReturnRepository>();
builder.Services.AddScoped<ISaleReturnRefundRepository,SaleReturnRefundRepository>();
builder.Services.AddScoped<ISaleReturnService, SaleReturnService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ICustomerPaymentRepository, CustomerPaymentRepository>();
builder.Services.AddScoped<ICustomerPaymentService, CustomerPaymentService>();
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password settings
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        // User settings
        options.User.RequireUniqueEmail = true;

        // Lockout settings
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(15);
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
        new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

var app = builder.Build();
var emailConfigured = !string.IsNullOrWhiteSpace(
    app.Configuration["AdminUser:Email"]);

var passwordConfigured = !string.IsNullOrWhiteSpace(
    app.Configuration["AdminUser:Password"]);

Console.WriteLine($"Admin Email Configured: {emailConfigured}");
Console.WriteLine($"Admin Password Configured: {passwordConfigured}");
await IdentitySeeder.SeedAsync(
    app.Services,
    app.Configuration);
app.UseMiddleware<GlobalExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}")
    .WithStaticAssets();


app.Run();
