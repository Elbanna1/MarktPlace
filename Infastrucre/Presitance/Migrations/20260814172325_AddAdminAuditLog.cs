using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAuditLogs_Action_CreatedAt",
                table: "AdminAuditLogs",
                columns: new[] { "Action", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAuditLogs_AdminUserId_CreatedAt",
                table: "AdminAuditLogs",
                columns: new[] { "AdminUserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAuditLogs_CreatedAt",
                table: "AdminAuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAuditLogs_TargetType_CreatedAt",
                table: "AdminAuditLogs",
                columns: new[] { "TargetType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAuditLogs_TargetType_TargetId",
                table: "AdminAuditLogs",
                columns: new[] { "TargetType", "TargetId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAuditLogs");
        }
    }
}
