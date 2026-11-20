using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseReturnNumberSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "PurchaseReturnNumberSequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "PurchaseReturnNumberSequence");
        }
    }
}
