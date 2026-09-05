using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleConditionalPriceAndReferralLinkEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "ReferralLinkEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferrerUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReferralCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralLinkEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferralLinkEvents_AspNetUsers_ReferrerUserId",
                        column: x => x.ReferrerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralLinkEvents_ReferralCode",
                table: "ReferralLinkEvents",
                column: "ReferralCode");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralLinkEvents_Referrer_Type_CreatedAt",
                table: "ReferralLinkEvents",
                columns: new[] { "ReferrerUserId", "EventType", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralLinkEvents");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
