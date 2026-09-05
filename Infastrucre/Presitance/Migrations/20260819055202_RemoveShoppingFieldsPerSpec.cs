using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveShoppingFieldsPerSpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cosmetics_ExpirationDate",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "HomemadeFoods");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Cosmetics");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Accessories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "HomemadeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "HomemadeFoods",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cosmetics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Cosmetics",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Accessories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cosmetics_ExpirationDate",
                table: "Cosmetics",
                column: "ExpirationDate");
        }
    }
}
