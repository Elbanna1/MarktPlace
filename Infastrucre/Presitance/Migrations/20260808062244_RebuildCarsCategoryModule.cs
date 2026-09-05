using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class RebuildCarsCategoryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ---- Columns the Cars specification retired ----
            //
            // The scaffolder paired three of these with new columns of the same SQL type and turned
            // them into renames: RentPeriod -> VehicleType, LicenseValid -> ServiceCenterInspection
            // and IsRunning -> ReadyToWork. Every one of those pairings is wrong — مدة الإيجار is not
            // نوع المركبة, and "الرخصة سارية" is not "يقبل الفحص في مركز خدمة" — so a rename would have
            // carried the old values into columns that mean something else entirely and no error would
            // ever have been raised. They are dropped and re-added instead, which is what the model
            // change actually is.
            migrationBuilder.DropColumn(
                name: "LicenseDuration",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "RentPeriod",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "LicenseValid",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsRunning",
                table: "Advertisements");

            // نوع المعدة became an enum. The old column holds Arabic names ("لودر", "حفار"), which
            // SQL Server cannot convert to int — an ALTER here fails outright on any seeded database —
            // so the column is replaced rather than altered.
            migrationBuilder.DropColumn(
                name: "MachineType",
                table: "Advertisements");

            migrationBuilder.AddColumn<int>(
                name: "MachineType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ServiceCenterInspection",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReadyToWork",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Features",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Scope",
                table: "Features",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptsHigherPriced",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptsLowerPriced",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccidentsCount",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivityType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AirbagsDeployed",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllMaintenanceAtDealer",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssemblyCountry",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BucketCapacity",
                table: "Advertisements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChangedParts",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChassisChanged",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChassisIntact",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConditionReport",
                table: "Advertisements",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CurrentlyWorking",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyPrice",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailedAddress",
                table: "Advertisements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DifferenceAmount",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DownPayment",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriveSystem",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EngineChanged",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EngineOverhauled",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EngineWorks",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExchangeDetails",
                table: "Advertisements",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FuelIncluded",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GearboxWorks",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasHelmet",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasInsurance",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasMaintenanceBook",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasRefundableDeposit",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyPrice",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HydraulicSystemWorks",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InspectionAllowed",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectionLocation",
                table: "Advertisements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstallmentMonths",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InstallmentsAccepted",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceExpiryDate",
                table: "Advertisements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInsured",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMoving",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpiryDate",
                table: "Advertisements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LicenseInOwnerName",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseIssueDate",
                table: "Advertisements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LicenseStatus",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LicenseTransferable",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumDistanceKm",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumRentPeriod",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyInstallment",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPrice",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotorcycleType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OperatingLicenseExpiryDate",
                table: "Advertisements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperatingLicenseStatus",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OperatingWeightTons",
                table: "Advertisements",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginCountry",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherBrand",
                table: "Advertisements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherMachineType",
                table: "Advertisements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherVehicleType",
                table: "Advertisements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartsChanged",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengersCount",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PowerUnit",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PowerValue",
                table: "Advertisements",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousOwners",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentSystems",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "Advertisements",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeatsCount",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicalCondition",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsageFields",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsageType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoPath",
                table: "Advertisements",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Advertisements",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeeklyPrice",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مقاولات", "مقاولات عامة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مقاولات", "تشطيبات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مقاولات", "أعمال العزل" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مقاولات", "أعمال الهدم" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مقاولات", "أعمال الحفر" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مقاولات", "أعمال الخرسانة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نقل ولوجستيات", "شحن داخلي" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نقل ولوجستيات", "شحن دولي" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نقل ولوجستيات", "نقل أثاث" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نقل ولوجستيات", "تخزين" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تجارية", "استيراد" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تجارية", "تصدير" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تجارية", "استيراد وتصدير" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تجارية", "توزيع" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تكنولوجيا", "برمجيات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تكنولوجيا", "تطوير مواقع إلكترونية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تكنولوجيا", "تطبيقات الموبايل" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تكنولوجيا", "شبكات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تكنولوجيا", "كاميرات مراقبة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تكنولوجيا", "أنظمة أمنية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تسويق ودعاية", "إعلانات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تسويق ودعاية", "تصميم جرافيك" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تسويق ودعاية", "طباعة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تسويق ودعاية", "تصوير" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تسويق ودعاية", "تسويق إلكتروني" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مالية ومحاسبة", "محاسبة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مالية ومحاسبة", "مراجعة حسابات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Group", "Name" },
                values: new object[] { "مالية ومحاسبة", "استشارات ضريبية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Group", "Name" },
                values: new object[] { "قانونية", "مكتب محاماة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Group", "Name" },
                values: new object[] { "قانونية", "استشارات قانونية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Group", "Name" },
                values: new object[] { "هندسية", "مكتب هندسي" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Group", "Name" },
                values: new object[] { "هندسية", "تصميم معماري" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Group", "Name" },
                values: new object[] { "هندسية", "إشراف هندسي" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Group", "Name" },
                values: new object[] { "هندسية", "أعمال المساحة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Group", "Name" },
                values: new object[] { "طبية", "مستلزمات طبية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Group", "Name" },
                values: new object[] { "طبية", "خدمات طبية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تعليم وتدريب", "تدريب" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تعليم وتدريب", "دورات تدريبية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تعليم وتدريب", "حضانات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Group", "Name" },
                values: new object[] { "تعليم وتدريب", "مراكز تعليمية" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نظافة وصيانة", "شركات نظافة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نظافة وصيانة", "مكافحة حشرات" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Group", "Name" },
                values: new object[] { "نظافة وصيانة", "صيانة عامة" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Group", "Name" },
                values: new object[] { "أخرى", "أخرى" });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 7 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 11 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 11 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 7 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التكنولوجيا", 11 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 11 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 5 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "الراحة", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 11 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "تجهيزات إضافية", 2 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "التجهيزات الخارجية", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Group", "Name", "Scope" },
                values: new object[] { "أنظمة الأمان", "نظام مراقبة ضغط الإطارات TPMS", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 1 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "أنظمة الأمان", 3 });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Group", "Scope" },
                values: new object[] { "تجهيزات إضافية", 2 });

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "Group", "Name", "Scope" },
                values: new object[,]
                {
                    { 42, "أنظمة الأمان", "Immobilizer", 1 },
                    { 43, "أنظمة الأمان", "ISOFIX", 1 },
                    { 44, "الراحة", "ستائر خلفية", 1 },
                    { 45, "الراحة", "زجاج فاميه", 1 },
                    { 46, "التكنولوجيا", "شاشة عدادات رقمية", 1 },
                    { 47, "التكنولوجيا", "USB", 3 },
                    { 48, "التكنولوجيا", "AUX", 1 },
                    { 49, "التكنولوجيا", "شاحن لاسلكي", 1 },
                    { 50, "التكنولوجيا", "نظام صوت Premium", 1 },
                    { 51, "التجهيزات الخارجية", "جنوط سبور", 5 },
                    { 52, "التجهيزات الخارجية", "إضاءة Xenon", 1 },
                    { 53, "التجهيزات الخارجية", "إضاءة Laser", 1 },
                    { 54, "التجهيزات الخارجية", "Spoiler", 1 },
                    { 55, "التجهيزات الخارجية", "Roof Rails", 1 },
                    { 56, "أخرى", "استبن", 1 },
                    { 57, "أخرى", "عدة السيارة", 1 },
                    { 58, "أخرى", "مفتاح احتياطي", 1 },
                    { 59, "أخرى", "كتيب السيارة الأصلي", 1 },
                    { 60, "الراحة", "مقاعد جلد", 2 },
                    { 61, "الراحة", "مقاعد قماش", 2 },
                    { 62, "تجهيزات إضافية", "حساسات ركن", 2 },
                    { 63, "الأمان", "CBS", 4 },
                    { 64, "الأمان", "مانع سرقة", 4 },
                    { 65, "الأمان", "إنذار", 12 },
                    { 66, "الأمان", "فرامل ديسك أمامي", 4 },
                    { 67, "الأمان", "فرامل ديسك خلفي", 4 },
                    { 68, "الراحة", "تشغيل بالبصمة", 4 },
                    { 69, "الراحة", "شاشة رقمية", 4 },
                    { 70, "الراحة", "USB Charger", 4 },
                    { 71, "التجهيزات", "صندوق خلفي", 4 },
                    { 72, "التجهيزات", "صندوق جانبي", 4 },
                    { 73, "التجهيزات", "زجاج أمامي", 4 },
                    { 74, "التجهيزات", "Hand Guards", 4 },
                    { 75, "التجهيزات", "Crash Bar", 4 },
                    { 76, "التجهيزات", "LED", 4 },
                    { 77, "التجهيزات", "كابينة مغلقة", 8 },
                    { 78, "التجهيزات", "كرسي هوائي", 8 },
                    { 79, "التجهيزات", "نظام هيدروليك إضافي", 8 },
                    { 80, "التجهيزات", "وصلة كسارة", 8 },
                    { 81, "التجهيزات", "وصلة حفار", 8 },
                    { 82, "التجهيزات", "وصلة شوكة", 8 },
                    { 83, "التجهيزات", "وصلة جردل إضافي", 8 },
                    { 84, "وسائل الأمان", "ROPS", 8 },
                    { 85, "وسائل الأمان", "FOPS", 8 },
                    { 86, "وسائل الأمان", "طفاية حريق", 8 },
                    { 87, "وسائل الأمان", "إنذار رجوع للخلف", 8 },
                    { 88, "وسائل الأمان", "كاميرات", 8 },
                    { 89, "وسائل الأمان", "أحزمة أمان", 8 }
                });

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 10,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 11,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 12,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 13,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 14,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 15,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 16,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 17,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 18,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 19,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 20,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 21,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 22,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 23,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 24,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 25,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 26,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 27,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 28,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 29,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 30,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 31,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 32,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 33,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 34,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 35,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 36,
                column: "Group",
                value: "مستلزمات الأنشطة التجارية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 37,
                column: "Group",
                value: "مستلزمات الأنشطة التجارية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 38,
                column: "Group",
                value: "مستلزمات الأنشطة التجارية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 39,
                column: "Group",
                value: "مستلزمات الأنشطة التجارية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 40,
                column: "Group",
                value: "مستلزمات الأنشطة التجارية");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 41,
                column: "Group",
                value: "أخرى");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "موردو الصناعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Group",
                value: "موردو الزراعة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "Group",
                value: "موردو الثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Group",
                value: "موردو مواد البناء");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 31,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 32,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 33,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 34,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 35,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 36,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 37,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 38,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 39,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 40,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 41,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 42,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 43,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 44,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 45,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 46,
                column: "Group",
                value: "موردو التجارة والتجزئة");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 47,
                column: "Group",
                value: "أخرى");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "المواد الغذائية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Group",
                value: "المنتجات الزراعية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Group",
                value: "الأزياء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "Group",
                value: "الأزياء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Group",
                value: "الأزياء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "Group",
                value: "الأزياء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Group",
                value: "المنتجات المنزلية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "Group",
                value: "المنتجات المنزلية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "Group",
                value: "المنتجات المنزلية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "Group",
                value: "المنتجات المنزلية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Group",
                value: "المنتجات المنزلية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Group",
                value: "المنتجات المنزلية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 31,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 32,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 33,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 34,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 35,
                column: "Group",
                value: "مواد البناء");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 36,
                column: "Group",
                value: "السيارات");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 37,
                column: "Group",
                value: "السيارات");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 38,
                column: "Group",
                value: "السيارات");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 39,
                column: "Group",
                value: "السيارات");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 40,
                column: "Group",
                value: "الزراعة والثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 41,
                column: "Group",
                value: "الزراعة والثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 42,
                column: "Group",
                value: "الزراعة والثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 43,
                column: "Group",
                value: "الزراعة والثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 44,
                column: "Group",
                value: "الزراعة والثروة الحيوانية");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 45,
                column: "Group",
                value: "أخرى");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "Scope",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "AcceptsHigherPriced",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AcceptsLowerPriced",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AccidentsCount",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ActivityType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AirbagsDeployed",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AllMaintenanceAtDealer",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AssemblyCountry",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "BucketCapacity",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ChangedParts",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ChassisChanged",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ChassisIntact",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ConditionReport",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CurrentlyWorking",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DailyPrice",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DetailedAddress",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DifferenceAmount",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DownPayment",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DriveSystem",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "EngineChanged",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "EngineOverhauled",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "EngineWorks",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ExchangeDetails",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "FuelIncluded",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "GearboxWorks",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HasHelmet",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HasInsurance",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HasMaintenanceBook",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HasRefundableDeposit",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HourlyPrice",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HydraulicSystemWorks",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "InspectionAllowed",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "InspectionLocation",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "InstallmentMonths",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "InstallmentsAccepted",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "InsuranceExpiryDate",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsInsured",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsMoving",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "LicenseExpiryDate",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "LicenseInOwnerName",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "LicenseIssueDate",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "LicenseStatus",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "LicenseTransferable",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MaximumDistanceKm",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MaximumRentPeriod",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MonthlyInstallment",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MotorcycleType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OperatingLicenseExpiryDate",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OperatingLicenseStatus",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OperatingWeightTons",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OriginCountry",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OtherBrand",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OtherMachineType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "OtherVehicleType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PartsChanged",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PassengersCount",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PowerUnit",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PowerValue",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PreviousOwners",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "RentSystems",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Route",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "SeatsCount",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "StartType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "TechnicalCondition",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "UsageFields",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "UsageType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "VideoPath",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "WeeklyPrice",
                table: "Advertisements");

            // The mirror image of the Up: these were replacements, not renames, so rolling back drops
            // the new columns and re-creates the old ones empty. The values cannot come back — that is
            // the honest outcome of a replacement, and it is why the Up does not pretend otherwise.
            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ServiceCenterInspection",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ReadyToWork",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MachineType",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "MachineType",
                table: "Advertisements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentPeriod",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LicenseValid",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRunning",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseDuration",
                table: "Advertisements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Power",
                table: "Advertisements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Construction", "General Contracting" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Construction", "Finishing" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Construction", "Insulation" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Construction", "Demolition" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Construction", "Excavation" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Construction", "Concrete" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Logistics", "Domestic Shipping" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Logistics", "International Shipping" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Logistics", "Furniture Moving" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Logistics", "Warehousing" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Commercial", "Import" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Commercial", "Export" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Commercial", "Import & Export" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Commercial", "Distribution" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Technology", "Software" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Technology", "Website Development" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Technology", "Mobile Applications" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Technology", "Networking" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Technology", "CCTV" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Technology", "Security Systems" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Marketing", "Advertising" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Marketing", "Graphic Design" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Marketing", "Printing" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Marketing", "Photography" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Marketing", "Digital Marketing" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Financial", "Accounting" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Financial", "Auditing" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Financial", "Tax Consulting" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Legal", "Law Firm" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Legal", "Legal Consulting" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Engineering", "Engineering Office" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Engineering", "Architecture" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Engineering", "Engineering Supervision" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Engineering", "Surveying" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Medical", "Medical Supplies" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Medical", "Medical Services" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Education", "Training" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Education", "Courses" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Education", "Nurseries" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Education", "Educational Centers" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Cleaning & Maintenance", "Cleaning Companies" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Cleaning & Maintenance", "Pest Control" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Cleaning & Maintenance", "General Maintenance" });

            migrationBuilder.UpdateData(
                table: "CompanyFields",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Group", "Name" },
                values: new object[] { "Other", "Other" });

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 37,
                column: "Name",
                value: "TPMS");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 10,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 11,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 12,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 13,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 14,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 15,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 16,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 17,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 18,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 19,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 20,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 21,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 22,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 23,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 24,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 25,
                column: "Group",
                value: "Agricultural Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 26,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 27,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 28,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 29,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 30,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 31,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 32,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 33,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 34,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 35,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 36,
                column: "Group",
                value: "Business Supplies");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 37,
                column: "Group",
                value: "Business Supplies");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 38,
                column: "Group",
                value: "Business Supplies");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 39,
                column: "Group",
                value: "Business Supplies");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 40,
                column: "Group",
                value: "Business Supplies");

            migrationBuilder.UpdateData(
                table: "SupplierSpecializations",
                keyColumn: "Id",
                keyValue: 41,
                column: "Group",
                value: "Other");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "Industrial Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Group",
                value: "Agriculture Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Group",
                value: "Agriculture Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Group",
                value: "Agriculture Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Group",
                value: "Agriculture Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Group",
                value: "Agriculture Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Group",
                value: "Agriculture Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "Group",
                value: "Livestock Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Group",
                value: "Construction Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 31,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 32,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 33,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 34,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 35,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 36,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 37,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 38,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 39,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 40,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 41,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 42,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 43,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 44,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 45,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 46,
                column: "Group",
                value: "Business & Retail Suppliers");

            migrationBuilder.UpdateData(
                table: "SupplierTypes",
                keyColumn: "Id",
                keyValue: 47,
                column: "Group",
                value: "Other");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Group",
                value: "Agricultural Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Group",
                value: "Fashion");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "Group",
                value: "Fashion");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Group",
                value: "Fashion");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "Group",
                value: "Fashion");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Group",
                value: "Home Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "Group",
                value: "Home Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "Group",
                value: "Home Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "Group",
                value: "Home Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Group",
                value: "Home Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Group",
                value: "Home Products");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 29,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 30,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 31,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 32,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 33,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 34,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 35,
                column: "Group",
                value: "Building Materials");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 36,
                column: "Group",
                value: "Automotive");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 37,
                column: "Group",
                value: "Automotive");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 38,
                column: "Group",
                value: "Automotive");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 39,
                column: "Group",
                value: "Automotive");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 40,
                column: "Group",
                value: "Agriculture & Livestock");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 41,
                column: "Group",
                value: "Agriculture & Livestock");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 42,
                column: "Group",
                value: "Agriculture & Livestock");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 43,
                column: "Group",
                value: "Agriculture & Livestock");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 44,
                column: "Group",
                value: "Agriculture & Livestock");

            migrationBuilder.UpdateData(
                table: "TradeTypes",
                keyColumn: "Id",
                keyValue: 45,
                column: "Group",
                value: "Other");
        }
    }
}
