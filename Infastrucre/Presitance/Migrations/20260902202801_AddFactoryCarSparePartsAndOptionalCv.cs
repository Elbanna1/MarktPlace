using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddFactoryCarSparePartsAndOptionalCv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CvFileUrl",
                table: "JobRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "CvFilePath",
                table: "JobRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.InsertData(
                table: "ProductionSpecialties",
                columns: new[] { "Id", "Name" },
                values: new object[] { 39, "قطع غيار السيارات" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductionSpecialties",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.AlterColumn<string>(
                name: "CvFileUrl",
                table: "JobRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CvFilePath",
                table: "JobRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
