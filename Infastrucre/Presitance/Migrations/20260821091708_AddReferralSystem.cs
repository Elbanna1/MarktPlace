using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddReferralSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferralCode",
                table: "AspNetUsers",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferrerUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReferredUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReferralCode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferrerNotified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.Id);
                    table.CheckConstraint("CK_Referrals_NoSelfReferral", "[ReferrerUserId] <> [ReferredUserId]");
                    table.ForeignKey(
                        name: "FK_Referrals_AspNetUsers_ReferredUserId",
                        column: x => x.ReferredUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Referrals_AspNetUsers_ReferrerUserId",
                        column: x => x.ReferrerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReferralCode",
                table: "AspNetUsers",
                column: "ReferralCode",
                unique: true,
                filter: "[ReferralCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferralCode",
                table: "Referrals",
                column: "ReferralCode");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferredUser",
                table: "Referrals",
                column: "ReferredUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_Referrer_ReferredUser",
                table: "Referrals",
                columns: new[] { "ReferrerUserId", "ReferredUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_Referrer_Status_CreatedAt",
                table: "Referrals",
                columns: new[] { "ReferrerUserId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_Status_CreatedAt",
                table: "Referrals",
                columns: new[] { "Status", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReferralCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReferralCode",
                table: "AspNetUsers");
        }
    }
}
