using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class RestructureWorkshopTypesAndCraftsmanSpecializations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkshopTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CraftsmanSpecializations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "سباكة" });

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "كهربائي منازل و3 فاز" });

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "سيراميك وبورسلين" });

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "رخام وجرانيت" });

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 15,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "جبس بورد وبلدي" });

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 17,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 18,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 19,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 20,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 21,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 22,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "تكييف وتبريد" });

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 25,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 26,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 29,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 30,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 31,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 32,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 33,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 34,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 35,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 36,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 37,
                column: "IsActive",
                value: true);

            migrationBuilder.InsertData(
                table: "CraftsmanSpecializations",
                columns: new[] { "Id", "GroupName", "IsActive", "Name" },
                values: new object[,]
                {
                    { 38, "النيوماتيك", true, "نيوماتيك" },
                    { 39, "الخزائن والمفاتيح", true, "فني خزائن ومفاتيح" },
                    { 40, "الأرضيات", true, "إيبوكسي" }
                });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "ورشة باب وشباك" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "حداد" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "نجار موبيليا" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "ورشة ألوميتال ومطابخ" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "ورشة تنجيد إفرنجي" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "مراكز صيانة" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "دوكو سيارات" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "ميكانيكا سيارات" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "صيانة أجهزة منزلية" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "لحام أرجون ومعادن" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "مشغل ملابس" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "مطابع ومكتبات" });

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "IsActive",
                value: true);

            migrationBuilder.InsertData(
                table: "WorkshopTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 27, true, "ورشة تنجيد بلدي" },
                    { 28, true, "الحفر بالكمبيوتر CNC" },
                    { 29, true, "ورشة ستائر" }
                });

            // Merged craftsman specializations: listings on a merged-away id move to the surviving id.
            // Re-running is a no-op; listing ids, ownership, images and interactions are untouched.
            migrationBuilder.Sql(@"
UPDATE [Craftsmen] SET [Specialization] = 4 WHERE [Specialization] IN (5, 6);
UPDATE [Craftsmen] SET [Specialization] = 7 WHERE [Specialization] = 8;
UPDATE [Craftsmen] SET [Specialization] = 11 WHERE [Specialization] = 12;
UPDATE [Craftsmen] SET [Specialization] = 13 WHERE [Specialization] = 14;
UPDATE [Craftsmen] SET [Specialization] = 24 WHERE [Specialization] = 25;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkshopTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CraftsmanSpecializations");

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "سباك");

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "كهربائي منازل");

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "سيراميك");

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "رخام");

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "جبس بورد");

            migrationBuilder.UpdateData(
                table: "CraftsmanSpecializations",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "فني تكييف");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "ورشة نجارة");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ورشة حدادة");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "ورشة موبيليا");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "ورشة مطابخ");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "ورشة تنجيد");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "ورشة سيارات");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "ورشة دهان سيارات");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Name",
                value: "ورشة موتور");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "ورشة إصلاح أجهزة كهربائية");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "ورشة تصنيع معدني");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "ورشة ملابس");

            migrationBuilder.UpdateData(
                table: "WorkshopTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "ورشة طباعة");
        }
    }
}
