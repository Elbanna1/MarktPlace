using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false),
                    NodeId = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActiveUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnabledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogConfigurations", x => x.Id);
                    table.CheckConstraint("CK_CatalogConfigurations_Area", "([Kind] = 1 AND [Area] = 0) OR ([Kind] = 2 AND [Area] > 0)");
                    table.CheckConstraint("CK_CatalogConfigurations_Price", "[Price] IS NULL OR [Price] > 0");
                    table.CheckConstraint("CK_CatalogConfigurations_Window", "[ActiveFrom] IS NULL OR [ActiveUntil] IS NULL OR [ActiveFrom] < [ActiveUntil]");
                });

            migrationBuilder.CreateIndex(
                name: "UX_CatalogConfigurations_Node",
                table: "CatalogConfigurations",
                columns: new[] { "Kind", "NodeType", "NodeId", "Area" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogConfigurations");
        }
    }
}
