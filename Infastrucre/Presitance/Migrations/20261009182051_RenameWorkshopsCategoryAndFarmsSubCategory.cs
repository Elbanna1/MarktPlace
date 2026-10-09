using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class RenameWorkshopsCategoryAndFarmsSubCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "NameAr",
                value: "دليل محافظتي");

            migrationBuilder.UpdateData(
                table: "HomeSections",
                keyColumn: "Id",
                keyValue: 102,
                column: "Title",
                value: "دليل محافظتي");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 10,
                column: "NameAr",
                value: "مزارع ومحاصيل");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "NameAr",
                value: "الورش والحرفيين");

            migrationBuilder.UpdateData(
                table: "HomeSections",
                keyColumn: "Id",
                keyValue: 102,
                column: "Title",
                value: "الورش والحرفيين");

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 10,
                column: "NameAr",
                value: "المزارع");
        }
    }
}
