using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <summary>
    /// صلاحيات المسؤولين — the two tables behind the Dashboard permission system, and nothing else.
    /// <para>
    /// Purely additive: two new tables, three indexes, no column on an existing table and no data
    /// change. The pages themselves are declared in code (AdminPageCatalog), so adding a Dashboard page
    /// later needs no migration at all — only these grant rows are data.
    /// </para>
    /// <para>
    /// The SuperAdmin role is not seeded here. Roles live in Identity's own AspNetRoles table and are
    /// ensured idempotently at start-up by IdentityDataSeeder, which is also where the one-time
    /// promotion of an existing administrator happens.
    /// </para>
    /// </summary>
    public partial class AddAdminRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminPageGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PageKey = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrantedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPageGrants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminPageGrants_AspNetUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminPagePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminPageGrantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Permission = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminPagePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminPagePermissions_AdminPageGrants_AdminPageGrantId",
                        column: x => x.AdminPageGrantId,
                        principalTable: "AdminPageGrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminPageGrants_AdminUserId_PageKey",
                table: "AdminPageGrants",
                columns: new[] { "AdminUserId", "PageKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminPageGrants_PageKey",
                table: "AdminPageGrants",
                column: "PageKey");

            migrationBuilder.CreateIndex(
                name: "IX_AdminPagePermissions_GrantId_Permission",
                table: "AdminPagePermissions",
                columns: new[] { "AdminPageGrantId", "Permission" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminPagePermissions");

            migrationBuilder.DropTable(
                name: "AdminPageGrants");
        }
    }
}
