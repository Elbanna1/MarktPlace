using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurableBannerDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // مدة الإعلان, defaulted to the 30 days the module used to have compiled in, so every
            // placement row that already exists keeps behaving exactly as it did before this column.
            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "BannerPlacementSettings",
                type: "int",
                nullable: false,
                defaultValue: 30);

            // Every booking made before this column existed was sold as a 30-day run, so that is the
            // length it is stamped with. A zero here would make an existing booking expire the day it
            // started the next time its window were re-anchored.
            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "BannerBookings",
                type: "int",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.UpdateData(
                table: "BannerPlacementSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AllowedFormats", "DurationDays" },
                values: new object[] { ".jpg,.jpeg,.png,.webp,.gif", 30 });

            migrationBuilder.UpdateData(
                table: "BannerPlacementSettings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AllowedFormats", "DurationDays" },
                values: new object[] { ".jpg,.jpeg,.png,.webp,.gif", 30 });

            migrationBuilder.UpdateData(
                table: "BannerPlacementSettings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AllowedFormats", "DurationDays" },
                values: new object[] { ".jpg,.jpeg,.png,.webp,.gif", 30 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "BannerPlacementSettings");

            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "BannerBookings");

            migrationBuilder.UpdateData(
                table: "BannerPlacementSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "AllowedFormats",
                value: ".jpg,.jpeg,.png,.webp");

            migrationBuilder.UpdateData(
                table: "BannerPlacementSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "AllowedFormats",
                value: ".jpg,.jpeg,.png,.webp");

            migrationBuilder.UpdateData(
                table: "BannerPlacementSettings",
                keyColumn: "Id",
                keyValue: 3,
                column: "AllowedFormats",
                value: ".jpg,.jpeg,.png,.webp");
        }
    }
}
