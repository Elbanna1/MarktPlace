using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertisementPaymentSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdPaymentSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentMethodIds = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnabledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdPaymentSettings", x => x.Id);
                    table.CheckConstraint("CK_AdPaymentSettings_Price", "[Price] >= 0");
                    table.CheckConstraint("CK_AdPaymentSettings_SingleRow", "[Id] = 1");
                });

            migrationBuilder.InsertData(
                table: "AdPaymentSettings",
                columns: new[] { "Id", "Currency", "EnabledAt", "Instructions", "IsEnabled", "PaymentMethodIds", "Price", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, "EGP", null, null, false, null, 0m, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Purpose_Status_SubmittedAt",
                table: "Payments",
                columns: new[] { "Purpose", "Status", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "UX_Payments_AdPayment_OnePending",
                table: "Payments",
                columns: new[] { "TargetType", "TargetId" },
                unique: true,
                filter: "[Purpose] = 6 AND [Status] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdPaymentSettings");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Purpose_Status_SubmittedAt",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "UX_Payments_AdPayment_OnePending",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "Payments");
        }
    }
}
