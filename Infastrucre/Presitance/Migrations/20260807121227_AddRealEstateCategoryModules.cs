using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddRealEstateCategoryModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApartmentDirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentDirections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentExchangeTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentExchangeTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentFinishingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentFinishingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentFloorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentFloorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentFurnishedStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentFurnishedStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentInstallmentProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentInstallmentProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentLegalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentOwnershipDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentOwnershipDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentOwnershipTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentOwnershipTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentPropertyAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentPropertyAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentReceptionPieces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentReceptionPieces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentReconciliationForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentReconciliationForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentRentInclusions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentRentInclusions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentRentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentRentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    ApartmentType = table.Column<int>(type: "int", nullable: true),
                    OtherApartmentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OwnershipType = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RoomsCount = table.Column<int>(type: "int", nullable: true),
                    BathroomsCount = table.Column<int>(type: "int", nullable: true),
                    ReceptionPieces = table.Column<int>(type: "int", nullable: true),
                    FloorType = table.Column<int>(type: "int", nullable: true),
                    FloorNumber = table.Column<int>(type: "int", nullable: true),
                    TotalFloors = table.Column<int>(type: "int", nullable: true),
                    ApartmentsPerFloor = table.Column<int>(type: "int", nullable: true),
                    HasElevator = table.Column<bool>(type: "bit", nullable: true),
                    FurnishedStatus = table.Column<int>(type: "int", nullable: true),
                    FinishingType = table.Column<int>(type: "int", nullable: true),
                    PropertyAge = table.Column<int>(type: "int", nullable: true),
                    Direction = table.Column<int>(type: "int", nullable: true),
                    ViewType = table.Column<int>(type: "int", nullable: true),
                    LegalStatus = table.Column<int>(type: "int", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseIssuer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReconciliationForm = table.Column<int>(type: "int", nullable: true),
                    OwnershipDocument = table.Column<int>(type: "int", nullable: true),
                    IsRegistered = table.Column<bool>(type: "bit", nullable: true),
                    HasViolations = table.Column<bool>(type: "bit", nullable: true),
                    ViolationDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Project = table.Column<int>(type: "int", nullable: true),
                    OtherProject = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    District = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    DownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentPeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InstallmentProvider = table.Column<int>(type: "int", nullable: true),
                    HasMaintenanceDeposit = table.Column<bool>(type: "bit", nullable: true),
                    MaintenanceDepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyFees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RentType = table.Column<int>(type: "int", nullable: true),
                    RentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RentDownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumRentPeriod = table.Column<int>(type: "int", nullable: true),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuitableFor = table.Column<int>(type: "int", nullable: true),
                    OwnerConditions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ExchangeWith = table.Column<int>(type: "int", nullable: true),
                    AcceptsDifferencePayment = table.Column<bool>(type: "bit", nullable: true),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExchangeDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apartments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApartmentSuitableFor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentSuitableFor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentViewTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentViewTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandAreaUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandAreaUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandBuildingCompletionRatios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandBuildingCompletionRatios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandContractDurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandContractDurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandDirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandDirections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandExchangeTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandExchangeTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandExistingBuildingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandExistingBuildingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandFacadesCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandFacadesCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandHarvestSeasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandHarvestSeasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandIrrigationSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandIrrigationSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandLegalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandMinimumRentPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandMinimumRentPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandOwnershipDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandOwnershipDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandQualityCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandQualityCertificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandReconciliationForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandReconciliationForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandRentInclusions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandRentInclusions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandRentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandRentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandRoadTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandRoadTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    LandType = table.Column<int>(type: "int", nullable: false),
                    OtherLandType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AreaUnit = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerMeter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FacadeLength = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FacadesCount = table.Column<int>(type: "int", nullable: true),
                    Direction = table.Column<int>(type: "int", nullable: true),
                    StreetWidth = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RoadType = table.Column<int>(type: "int", nullable: true),
                    InsideBuildingCordon = table.Column<bool>(type: "bit", nullable: true),
                    IsBuildable = table.Column<bool>(type: "bit", nullable: true),
                    AllowedBuildingRatio = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    AllowedFloorsCount = table.Column<int>(type: "int", nullable: true),
                    LegalStatus = table.Column<int>(type: "int", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseIssuer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReconciliationForm = table.Column<int>(type: "int", nullable: true),
                    OtherReconciliationForm = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OwnershipDocument = table.Column<int>(type: "int", nullable: true),
                    OtherOwnershipDocument = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HasSurveyPlan = table.Column<bool>(type: "bit", nullable: true),
                    HasViolations = table.Column<bool>(type: "bit", nullable: true),
                    ViolationDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Project = table.Column<int>(type: "int", nullable: true),
                    OtherProject = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    District = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    RentType = table.Column<int>(type: "int", nullable: true),
                    RentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumRentPeriod = table.Column<int>(type: "int", nullable: true),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractDuration = table.Column<int>(type: "int", nullable: true),
                    OwnerConditions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ExchangeWith = table.Column<int>(type: "int", nullable: true),
                    OtherExchangeWith = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AcceptsDifferencePayment = table.Column<bool>(type: "bit", nullable: true),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExchangeInSameGovernorateOnly = table.Column<bool>(type: "bit", nullable: true),
                    ExchangeDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsCurrentlyCultivated = table.Column<bool>(type: "bit", nullable: true),
                    CurrentCropType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CultivatedFeddans = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    HarvestSeason = table.Column<int>(type: "int", nullable: true),
                    SoilType = table.Column<int>(type: "int", nullable: true),
                    IrrigationSource = table.Column<int>(type: "int", nullable: true),
                    OtherIrrigationSource = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HasWell = table.Column<bool>(type: "bit", nullable: true),
                    HasIrrigationMachine = table.Column<bool>(type: "bit", nullable: true),
                    HasIrrigationNetwork = table.Column<bool>(type: "bit", nullable: true),
                    HasTrees = table.Column<bool>(type: "bit", nullable: true),
                    TreeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TreesCount = table.Column<int>(type: "int", nullable: true),
                    TreesAge = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HasFarmHouse = table.Column<bool>(type: "bit", nullable: true),
                    HasRestHouse = table.Column<bool>(type: "bit", nullable: true),
                    HasStorage = table.Column<bool>(type: "bit", nullable: true),
                    HasFence = table.Column<bool>(type: "bit", nullable: true),
                    HasResidentWorkers = table.Column<bool>(type: "bit", nullable: true),
                    IsOrganic = table.Column<bool>(type: "bit", nullable: true),
                    UsesChemicalFertilizers = table.Column<bool>(type: "bit", nullable: true),
                    HasQualityCertificate = table.Column<bool>(type: "bit", nullable: true),
                    QualityCertificate = table.Column<int>(type: "int", nullable: true),
                    HasGate = table.Column<bool>(type: "bit", nullable: true),
                    IsLeveledForBuilding = table.Column<bool>(type: "bit", nullable: true),
                    HasFoundations = table.Column<bool>(type: "bit", nullable: true),
                    HasExistingBuilding = table.Column<bool>(type: "bit", nullable: true),
                    ExistingBuildingType = table.Column<int>(type: "int", nullable: true),
                    BuildingCompletionRatio = table.Column<int>(type: "int", nullable: true),
                    CurrentFloorsCount = table.Column<int>(type: "int", nullable: true),
                    CanAddFloors = table.Column<bool>(type: "bit", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lands_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LandSoilTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandSoilTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandUtilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandUtilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealEstateListingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateListingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealEstateProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopEntrancesCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopEntrancesCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopExchangeTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopExchangeTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopFacadeDirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopFacadeDirections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopFacadesCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopFacadesCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopFinishingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopFinishingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopFloorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopFloorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopInstallmentProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopInstallmentProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopLegalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopLicenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopLicenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopOwnershipDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopOwnershipDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopPropertyAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopPropertyAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopReconciliationForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopReconciliationForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopRentInclusions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRentInclusions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopRentSuitableActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRentSuitableActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopRentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    SuitableActivity = table.Column<int>(type: "int", nullable: true),
                    OtherSuitableActivity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FloorType = table.Column<int>(type: "int", nullable: true),
                    CeilingHeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FacadeWidth = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FacadesCount = table.Column<int>(type: "int", nullable: true),
                    FacadeDirection = table.Column<int>(type: "int", nullable: true),
                    FinishingType = table.Column<int>(type: "int", nullable: true),
                    PropertyAge = table.Column<int>(type: "int", nullable: true),
                    HasBathroom = table.Column<bool>(type: "bit", nullable: true),
                    BathroomsCount = table.Column<int>(type: "int", nullable: true),
                    HasStorage = table.Column<bool>(type: "bit", nullable: true),
                    StorageArea = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HasGlassFacade = table.Column<bool>(type: "bit", nullable: true),
                    SuitableForRestaurantOrCafe = table.Column<bool>(type: "bit", nullable: true),
                    HasExtractorFan = table.Column<bool>(type: "bit", nullable: true),
                    HasPrivateEntrance = table.Column<bool>(type: "bit", nullable: true),
                    EntrancesCount = table.Column<int>(type: "int", nullable: true),
                    LegalStatus = table.Column<int>(type: "int", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseType = table.Column<int>(type: "int", nullable: true),
                    LicenseIssuer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LicenseIssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReconciliationForm = table.Column<int>(type: "int", nullable: true),
                    OtherReconciliationForm = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OwnershipDocument = table.Column<int>(type: "int", nullable: true),
                    OtherOwnershipDocument = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsRegistered = table.Column<bool>(type: "bit", nullable: true),
                    WasPreviouslyOperating = table.Column<bool>(type: "bit", nullable: true),
                    PreviousActivity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PreviousOperatingPeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VacancyReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Project = table.Column<int>(type: "int", nullable: true),
                    OtherProject = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    District = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    DownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentPeriod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InstallmentProvider = table.Column<int>(type: "int", nullable: true),
                    RentType = table.Column<int>(type: "int", nullable: true),
                    RentValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RentDownPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumRentPeriod = table.Column<int>(type: "int", nullable: true),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllowsActivityChange = table.Column<bool>(type: "bit", nullable: true),
                    OwnerConditions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ExchangeWith = table.Column<int>(type: "int", nullable: true),
                    AcceptsDifferencePayment = table.Column<bool>(type: "bit", nullable: true),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExchangeDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopSuitableActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSuitableActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopUtilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopUtilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentFeatureSelections",
                columns: table => new
                {
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Feature = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentFeatureSelections", x => new { x.ApartmentId, x.Feature });
                    table.ForeignKey(
                        name: "FK_ApartmentFeatureSelections_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApartmentImages_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentRentInclusionSelections",
                columns: table => new
                {
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Inclusion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentRentInclusionSelections", x => new { x.ApartmentId, x.Inclusion });
                    table.ForeignKey(
                        name: "FK_ApartmentRentInclusionSelections_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandImages_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandRentInclusionSelections",
                columns: table => new
                {
                    LandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Inclusion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandRentInclusionSelections", x => new { x.LandId, x.Inclusion });
                    table.ForeignKey(
                        name: "FK_LandRentInclusionSelections_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandUtilitySelections",
                columns: table => new
                {
                    LandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Utility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandUtilitySelections", x => new { x.LandId, x.Utility });
                    table.ForeignKey(
                        name: "FK_LandUtilitySelections_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopImages_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopRentInclusionSelections",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Inclusion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRentInclusionSelections", x => new { x.ShopId, x.Inclusion });
                    table.ForeignKey(
                        name: "FK_ShopRentInclusionSelections_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopRentSuitableActivitySelections",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Activity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRentSuitableActivitySelections", x => new { x.ShopId, x.Activity });
                    table.ForeignKey(
                        name: "FK_ShopRentSuitableActivitySelections_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopUtilitySelections",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Utility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopUtilitySelections", x => new { x.ShopId, x.Utility });
                    table.ForeignKey(
                        name: "FK_ShopUtilitySelections_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApartmentDirections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بحري", "North" },
                    { 2, "قبلي", "South" },
                    { 3, "شرقي", "East" },
                    { 4, "غربي", "West" },
                    { 5, "ناصية", "Corner" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentExchangeTargets",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "شقة", "Apartment" },
                    { 2, "فيلا", "Villa" },
                    { 3, "أرض", "Land" },
                    { 4, "محل", "Shop" },
                    { 5, "سيارة", "Car" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentFeatures",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مصعد", "Elevator" },
                    { 2, "جراج", "Garage" },
                    { 3, "أمن", "Security" },
                    { 4, "كاميرات مراقبة", "Surveillance cameras" },
                    { 5, "غاز طبيعي", "Natural gas" },
                    { 6, "عداد كهرباء", "Electricity meter" },
                    { 7, "عداد مياه", "Water meter" },
                    { 8, "إنترنت", "Internet" },
                    { 9, "تكييف", "Air conditioning" },
                    { 10, "مطبخ", "Kitchen" },
                    { 11, "بلكونة", "Balcony" },
                    { 12, "غرفة ملابس", "Dressing room" },
                    { 13, "غرفة غسيل", "Laundry room" },
                    { 14, "مخزن", "Storage" },
                    { 15, "مولد كهرباء", "Generator" },
                    { 16, "حديقة", "Garden" },
                    { 17, "مسبح", "Swimming pool" },
                    { 18, "نادي", "Club" },
                    { 19, "جيم", "Gym" },
                    { 20, "مدخل خاص", "Private entrance" },
                    { 21, "باب مصفح", "Armored door" },
                    { 22, "إنتركم", "Intercom" },
                    { 23, "دش مركزي", "Central satellite" },
                    { 24, "خزانات مياه", "Water tanks" },
                    { 25, "ألواح طاقة شمسية", "Solar panels" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentFinishingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "سوبر لوكس", "Super lux" },
                    { 2, "لوكس", "Lux" },
                    { 3, "تشطيب فاخر", "Luxury finishing" },
                    { 4, "نصف تشطيب", "Semi-finished" },
                    { 5, "طوب أحمر", "Red brick" },
                    { 6, "بدون تشطيب", "Unfinished" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentFloorTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أرضي", "Ground" },
                    { 2, "متكرر", "Repeated" },
                    { 3, "أخير", "Last" },
                    { 4, "رووف", "Roof" },
                    { 5, "بدروم", "Basement" },
                    { 6, "ميزانين", "Mezzanine" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentFurnishedStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نعم", "Yes" },
                    { 2, "لا", "No" },
                    { 3, "نصف مفروشة", "Semi-furnished" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentInstallmentProviders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مالك", "Owner" },
                    { 2, "شركة", "Company" },
                    { 3, "بنك", "Bank" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentLegalStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مرخصة", "Licensed" },
                    { 2, "تصالح", "Reconciliation" },
                    { 3, "غير مرخصة", "Unlicensed" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentOwnershipDocuments",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عقد نهائي", "Final contract" },
                    { 2, "عقد أخضر", "Green contract" },
                    { 3, "عقد ابتدائي", "Preliminary contract" },
                    { 4, "توكيل", "Power of attorney" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentOwnershipTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تمليك", "Freehold" },
                    { 2, "حكومي", "Government" },
                    { 3, "خاص", "Private" },
                    { 4, "اتحاد ملاك", "Owners association" },
                    { 5, "إسكان اجتماعي", "Social housing" },
                    { 6, "إسكان متميز", "Distinguished housing" },
                    { 7, "كمبوند", "Compound" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentPaymentMethods",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كاش", "Cash" },
                    { 2, "تقسيط", "Installments" },
                    { 3, "كاش أو تقسيط", "Cash or installments" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentPropertyAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "أقل من 5 سنوات", "Under 5 years" },
                    { 3, "5 - 10 سنوات", "5 - 10 years" },
                    { 4, "أكثر من 10 سنوات", "Over 10 years" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentReceptionPieces",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "1", "1" },
                    { 2, "2", "2" },
                    { 3, "3", "3" },
                    { 4, "4", "4" },
                    { 5, "مفتوح", "Open" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentReconciliationForms",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نموذج 1", "Form 1" },
                    { 2, "نموذج 3", "Form 3" },
                    { 3, "نموذج 8", "Form 8" },
                    { 4, "نموذج 10", "Form 10" },
                    { 5, "نموذج نهائي", "Final form" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentRentInclusions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كهرباء", "Electricity" },
                    { 2, "مياه", "Water" },
                    { 3, "غاز", "Gas" },
                    { 4, "إنترنت", "Internet" },
                    { 5, "صيانة", "Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentRentTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "يومي", "Daily" },
                    { 2, "أسبوعي", "Weekly" },
                    { 3, "شهري", "Monthly" },
                    { 4, "سنوي", "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentSuitableFor",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أفراد", "Individuals" },
                    { 2, "عائلات", "Families" },
                    { 3, "طلاب", "Students" },
                    { 4, "شركات", "Companies" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "شقة سكنية", "Residential apartment" },
                    { 2, "شقة إدارية", "Administrative apartment" },
                    { 3, "استوديو", "Studio" },
                    { 4, "دوبلكس", "Duplex" },
                    { 5, "بنتهاوس", "Penthouse" },
                    { 6, "رووف", "Roof" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ApartmentViewTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "شارع رئيسي", "Main street" },
                    { 2, "شارع جانبي", "Side street" },
                    { 3, "حديقة", "Garden" },
                    { 4, "نيل", "Nile" },
                    { 5, "بحر", "Sea" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NameAr" },
                values: new object[] { 11, "Real Estate", "عقارات" });

            migrationBuilder.InsertData(
                table: "LandAreaUnits",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "متر مربع", "Square meter" },
                    { 2, "فدان", "Feddan" }
                });

            migrationBuilder.InsertData(
                table: "LandBuildingCompletionRatios",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "25%", "25%" },
                    { 2, "50%", "50%" },
                    { 3, "75%", "75%" },
                    { 4, "مكتمل", "Completed" }
                });

            migrationBuilder.InsertData(
                table: "LandContractDurations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "سنة", "One year" },
                    { 2, "سنتان", "Two years" },
                    { 3, "ثلاث سنوات", "Three years" },
                    { 4, "حسب الاتفاق", "As agreed" }
                });

            migrationBuilder.InsertData(
                table: "LandDirections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بحري", "North" },
                    { 2, "قبلي", "South" },
                    { 3, "شرقي", "East" },
                    { 4, "غربي", "West" },
                    { 5, "ناصية", "Corner" }
                });

            migrationBuilder.InsertData(
                table: "LandExchangeTargets",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أرض", "Land" },
                    { 2, "شقة", "Apartment" },
                    { 3, "محل", "Shop" },
                    { 4, "فيلا", "Villa" },
                    { 5, "مزرعة", "Farm" },
                    { 6, "سيارة", "Car" },
                    { 7, "مصنع", "Factory" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandExistingBuildingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "منزل", "House" },
                    { 2, "مخزن", "Storage" },
                    { 3, "محل", "Shop" },
                    { 4, "مصنع", "Factory" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandFacadesCounts",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "واجهة واحدة", "One facade" },
                    { 2, "واجهتان", "Two facades" },
                    { 3, "ثلاث واجهات", "Three facades" },
                    { 4, "أربع واجهات", "Four facades" }
                });

            migrationBuilder.InsertData(
                table: "LandHarvestSeasons",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "صيفي", "Summer" },
                    { 2, "شتوي", "Winter" },
                    { 3, "طوال العام", "All year" }
                });

            migrationBuilder.InsertData(
                table: "LandIrrigationSources",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ترعة", "Canal" },
                    { 2, "بئر", "Well" },
                    { 3, "تنقيط", "Drip" },
                    { 4, "رش", "Sprinkler" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandLegalStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مرخصة", "Licensed" },
                    { 2, "تصالح", "Reconciliation" },
                    { 3, "غير مرخصة", "Unlicensed" }
                });

            migrationBuilder.InsertData(
                table: "LandMinimumRentPeriods",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "يوم", "Day" },
                    { 2, "أسبوع", "Week" },
                    { 3, "شهر", "Month" },
                    { 4, "سنة", "Year" }
                });

            migrationBuilder.InsertData(
                table: "LandOwnershipDocuments",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عقد نهائي", "Final contract" },
                    { 2, "عقد أخضر", "Green contract" },
                    { 3, "عقد ابتدائي", "Preliminary contract" },
                    { 4, "توكيل", "Power of attorney" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandQualityCertificates",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "Global GAP", "Global GAP" },
                    { 2, "Organic", "Organic" },
                    { 3, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandReconciliationForms",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نموذج 1", "Form 1" },
                    { 2, "نموذج 3", "Form 3" },
                    { 3, "نموذج 8", "Form 8" },
                    { 4, "نموذج 10", "Form 10" },
                    { 5, "نموذج نهائي", "Final form" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandRentInclusions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كهرباء", "Electricity" },
                    { 2, "مياه", "Water" },
                    { 3, "غاز", "Gas" },
                    { 4, "صيانة", "Maintenance" },
                    { 5, "حراسة", "Security" }
                });

            migrationBuilder.InsertData(
                table: "LandRentTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "يومي", "Daily" },
                    { 2, "أسبوعي", "Weekly" },
                    { 3, "شهري", "Monthly" },
                    { 4, "سنوي", "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "LandRoadTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أسفلت", "Asphalt" },
                    { 2, "إنترلوك", "Interlock" },
                    { 3, "ترابي", "Dirt" },
                    { 4, "ممهد", "Graded" }
                });

            migrationBuilder.InsertData(
                table: "LandSoilTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "طينية", "Clay" },
                    { 2, "رملية", "Sandy" },
                    { 3, "صفراء", "Yellow" },
                    { 4, "جيرية", "Limestone" },
                    { 5, "مختلطة", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "LandTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أرض سكنية", "Residential land" },
                    { 2, "أرض مباني", "Building land" },
                    { 3, "أرض تجارية", "Commercial land" },
                    { 4, "أرض إدارية", "Administrative land" },
                    { 5, "أرض صناعية", "Industrial land" },
                    { 6, "أرض زراعية", "Agricultural land" },
                    { 7, "أرض استصلاح", "Reclamation land" },
                    { 8, "مقابر", "Cemeteries" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LandUtilities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عداد كهرباء", "Electricity meter" },
                    { 2, "عداد مياه", "Water meter" },
                    { 3, "غاز طبيعي", "Natural gas" },
                    { 4, "صرف صحي", "Sewage" },
                    { 5, "إنترنت", "Internet" },
                    { 6, "بئر مياه", "Water well" },
                    { 7, "شبكة ري", "Irrigation network" },
                    { 8, "مسورة", "Walled" },
                    { 9, "بوابة", "Gate" },
                    { 10, "إنارة شارع", "Street lighting" },
                    { 11, "صرف زراعي", "Agricultural drainage" }
                });

            migrationBuilder.InsertData(
                table: "RealEstateListingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بيع", "Sale" },
                    { 2, "إيجار", "Rent" },
                    { 3, "بدل", "Exchange" }
                });

            migrationBuilder.InsertData(
                table: "RealEstateProjects",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ابني بيتك", "Ebni Beetak" },
                    { 2, "الإسكان الاجتماعي", "Social Housing" },
                    { 3, "سكن مصر", "Sakan Misr" },
                    { 4, "دار مصر", "Dar Misr" },
                    { 5, "جنة", "Janna" },
                    { 6, "بيت الوطن", "Beit Al Watan" },
                    { 7, "الحي الأول", "First District" },
                    { 8, "الحي الثاني", "Second District" },
                    { 9, "الحي الثالث", "Third District" },
                    { 10, "منطقة الخدمات", "Services Area" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShopEntrancesCounts",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "1", "1" },
                    { 2, "2", "2" },
                    { 3, "3", "3" },
                    { 4, "أكثر", "More" }
                });

            migrationBuilder.InsertData(
                table: "ShopExchangeTargets",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محل", "Shop" },
                    { 2, "شقة", "Apartment" },
                    { 3, "أرض", "Land" },
                    { 4, "فيلا", "Villa" },
                    { 5, "سيارة", "Car" },
                    { 6, "مصنع", "Factory" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShopFacadeDirections",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بحري", "North" },
                    { 2, "قبلي", "South" },
                    { 3, "شرقي", "East" },
                    { 4, "غربي", "West" },
                    { 5, "ناصية", "Corner" }
                });

            migrationBuilder.InsertData(
                table: "ShopFacadesCounts",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "واجهة واحدة", "One facade" },
                    { 2, "واجهتان", "Two facades" },
                    { 3, "ثلاث واجهات", "Three facades" },
                    { 4, "أربع واجهات", "Four facades" }
                });

            migrationBuilder.InsertData(
                table: "ShopFinishingTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "سوبر لوكس", "Super lux" },
                    { 2, "لوكس", "Lux" },
                    { 3, "نصف تشطيب", "Semi-finished" },
                    { 4, "طوب أحمر", "Red brick" },
                    { 5, "بدون تشطيب", "Unfinished" }
                });

            migrationBuilder.InsertData(
                table: "ShopFloorTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "أرضي", "Ground" },
                    { 2, "ميزانين", "Mezzanine" },
                    { 3, "أول", "First" },
                    { 4, "بدروم", "Basement" }
                });

            migrationBuilder.InsertData(
                table: "ShopInstallmentProviders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "المالك", "Owner" },
                    { 2, "شركة", "Company" },
                    { 3, "بنك", "Bank" }
                });

            migrationBuilder.InsertData(
                table: "ShopLegalStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مرخص", "Licensed" },
                    { 2, "تصالح", "Reconciliation" },
                    { 3, "غير مرخص", "Unlicensed" }
                });

            migrationBuilder.InsertData(
                table: "ShopLicenseTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تجارية", "Commercial" },
                    { 2, "إدارية", "Administrative" },
                    { 3, "صناعية", "Industrial" },
                    { 4, "خدمية", "Service" }
                });

            migrationBuilder.InsertData(
                table: "ShopOwnershipDocuments",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عقد نهائي", "Final contract" },
                    { 2, "عقد أخضر", "Green contract" },
                    { 3, "عقد ابتدائي", "Preliminary contract" },
                    { 4, "توكيل", "Power of attorney" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShopPaymentMethods",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كاش", "Cash" },
                    { 2, "تقسيط", "Installments" },
                    { 3, "كاش أو تقسيط", "Cash or installments" }
                });

            migrationBuilder.InsertData(
                table: "ShopPropertyAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "جديد", "New" },
                    { 2, "أقل من 5 سنوات", "Under 5 years" },
                    { 3, "5 - 10 سنوات", "5 - 10 years" },
                    { 4, "أكثر من 10 سنوات", "Over 10 years" }
                });

            migrationBuilder.InsertData(
                table: "ShopReconciliationForms",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "نموذج 1", "Form 1" },
                    { 2, "نموذج 3", "Form 3" },
                    { 3, "نموذج 8", "Form 8" },
                    { 4, "نموذج 10", "Form 10" },
                    { 5, "نموذج نهائي", "Final form" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShopRentInclusions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كهرباء", "Electricity" },
                    { 2, "مياه", "Water" },
                    { 3, "غاز", "Gas" },
                    { 4, "إنترنت", "Internet" },
                    { 5, "صيانة", "Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "ShopRentSuitableActivities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مطعم", "Restaurant" },
                    { 2, "كافيه", "Cafe" },
                    { 3, "سوبر ماركت", "Supermarket" },
                    { 4, "ملابس", "Clothing" },
                    { 5, "صيدلية", "Pharmacy" },
                    { 6, "مكتب", "Office" },
                    { 7, "عيادة", "Clinic" },
                    { 8, "معرض", "Showroom" },
                    { 9, "مخزن", "Storage" },
                    { 10, "أي نشاط", "Any activity" }
                });

            migrationBuilder.InsertData(
                table: "ShopRentTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "يومي", "Daily" },
                    { 2, "أسبوعي", "Weekly" },
                    { 3, "شهري", "Monthly" },
                    { 4, "سنوي", "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "ShopSuitableActivities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ملابس", "Clothing" },
                    { 2, "أحذية", "Shoes" },
                    { 3, "سوبر ماركت", "Supermarket" },
                    { 4, "مطعم", "Restaurant" },
                    { 5, "كافيه", "Cafe" },
                    { 6, "مخبز", "Bakery" },
                    { 7, "حلويات", "Sweets" },
                    { 8, "صيدلية", "Pharmacy" },
                    { 9, "مكتب", "Office" },
                    { 10, "عيادة", "Clinic" },
                    { 11, "معرض", "Showroom" },
                    { 12, "ورشة", "Workshop" },
                    { 13, "مخزن", "Storage" },
                    { 14, "مكتبة", "Bookstore" },
                    { 15, "أدوات كهربائية", "Electrical tools" },
                    { 16, "موبايلات", "Mobiles" },
                    { 17, "كمبيوتر", "Computers" },
                    { 18, "ذهب ومجوهرات", "Gold & jewellery" },
                    { 19, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ShopUtilities",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عداد كهرباء", "Electricity meter" },
                    { 2, "عداد مياه", "Water meter" },
                    { 3, "غاز طبيعي", "Natural gas" },
                    { 4, "صرف صحي", "Sewage" },
                    { 5, "إنترنت", "Internet" },
                    { 6, "تكييف", "Air conditioning" },
                    { 7, "كاميرات مراقبة", "Surveillance cameras" },
                    { 8, "نظام إنذار", "Alarm system" },
                    { 9, "نظام إطفاء حريق", "Fire extinguishing system" },
                    { 10, "أمن", "Security" },
                    { 11, "مصعد", "Elevator" },
                    { 12, "موقف سيارات", "Parking" },
                    { 13, "مولد كهرباء", "Generator" },
                    { 14, "لافتة جاهزة", "Ready sign" },
                    { 15, "تجهيزات مطعم", "Restaurant equipment" },
                    { 16, "تجهيزات كافيه", "Cafe equipment" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Name", "NameAr" },
                values: new object[,]
                {
                    { 47, 11, "Lands", "أراضي" },
                    { 48, 11, "Apartments", "شقق" },
                    { 49, 11, "Shops", "محلات" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentDirections_Name",
                table: "ApartmentDirections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentExchangeTargets_Name",
                table: "ApartmentExchangeTargets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentFeatures_Name",
                table: "ApartmentFeatures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentFeatureSelections_Feature",
                table: "ApartmentFeatureSelections",
                column: "Feature");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentFinishingTypes_Name",
                table: "ApartmentFinishingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentFloorTypes_Name",
                table: "ApartmentFloorTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentFurnishedStatuses_Name",
                table: "ApartmentFurnishedStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentImages_ApartmentId_SortOrder",
                table: "ApartmentImages",
                columns: new[] { "ApartmentId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentInstallmentProviders_Name",
                table: "ApartmentInstallmentProviders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentLegalStatuses_Name",
                table: "ApartmentLegalStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentOwnershipDocuments_Name",
                table: "ApartmentOwnershipDocuments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentOwnershipTypes_Name",
                table: "ApartmentOwnershipTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentPaymentMethods_Name",
                table: "ApartmentPaymentMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentPropertyAges_Name",
                table: "ApartmentPropertyAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentReceptionPieces_Name",
                table: "ApartmentReceptionPieces",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentReconciliationForms_Name",
                table: "ApartmentReconciliationForms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentRentInclusions_Name",
                table: "ApartmentRentInclusions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentRentInclusionSelections_Inclusion",
                table: "ApartmentRentInclusionSelections",
                column: "Inclusion");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentRentTypes_Name",
                table: "ApartmentRentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ApartmentType",
                table: "Apartments",
                column: "ApartmentType");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Area",
                table: "Apartments",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BathroomsCount",
                table: "Apartments",
                column: "BathroomsCount");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Center",
                table: "Apartments",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_CreatedAt",
                table: "Apartments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_FinishingType",
                table: "Apartments",
                column: "FinishingType");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_FloorType",
                table: "Apartments",
                column: "FloorType");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_FurnishedStatus",
                table: "Apartments",
                column: "FurnishedStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_IsPremium_IsFeatured",
                table: "Apartments",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_LegalStatus",
                table: "Apartments",
                column: "LegalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ListingType",
                table: "Apartments",
                column: "ListingType");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Negotiable",
                table: "Apartments",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_OwnershipDocument",
                table: "Apartments",
                column: "OwnershipDocument");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_OwnershipType",
                table: "Apartments",
                column: "OwnershipType");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Price",
                table: "Apartments",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Project",
                table: "Apartments",
                column: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_RoomsCount",
                table: "Apartments",
                column: "RoomsCount");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_Title",
                table: "Apartments",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ViewCount",
                table: "Apartments",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentSuitableFor_Name",
                table: "ApartmentSuitableFor",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentTypes_Name",
                table: "ApartmentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentViewTypes_Name",
                table: "ApartmentViewTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandAreaUnits_Name",
                table: "LandAreaUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandBuildingCompletionRatios_Name",
                table: "LandBuildingCompletionRatios",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandContractDurations_Name",
                table: "LandContractDurations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandDirections_Name",
                table: "LandDirections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandExchangeTargets_Name",
                table: "LandExchangeTargets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandExistingBuildingTypes_Name",
                table: "LandExistingBuildingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandFacadesCounts_Name",
                table: "LandFacadesCounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandHarvestSeasons_Name",
                table: "LandHarvestSeasons",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandImages_LandId_SortOrder",
                table: "LandImages",
                columns: new[] { "LandId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_LandIrrigationSources_Name",
                table: "LandIrrigationSources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandLegalStatuses_Name",
                table: "LandLegalStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandMinimumRentPeriods_Name",
                table: "LandMinimumRentPeriods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandOwnershipDocuments_Name",
                table: "LandOwnershipDocuments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandQualityCertificates_Name",
                table: "LandQualityCertificates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandReconciliationForms_Name",
                table: "LandReconciliationForms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandRentInclusions_Name",
                table: "LandRentInclusions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandRentInclusionSelections_Inclusion",
                table: "LandRentInclusionSelections",
                column: "Inclusion");

            migrationBuilder.CreateIndex(
                name: "IX_LandRentTypes_Name",
                table: "LandRentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandRoadTypes_Name",
                table: "LandRoadTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Area",
                table: "Lands",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_AreaUnit",
                table: "Lands",
                column: "AreaUnit");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Center",
                table: "Lands",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_CreatedAt",
                table: "Lands",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Direction",
                table: "Lands",
                column: "Direction");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_FacadesCount",
                table: "Lands",
                column: "FacadesCount");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_HasFence",
                table: "Lands",
                column: "HasFence");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_HasIrrigationNetwork",
                table: "Lands",
                column: "HasIrrigationNetwork");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_HasWell",
                table: "Lands",
                column: "HasWell");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_InsideBuildingCordon",
                table: "Lands",
                column: "InsideBuildingCordon");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_IrrigationSource",
                table: "Lands",
                column: "IrrigationSource");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_IsBuildable",
                table: "Lands",
                column: "IsBuildable");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_IsCurrentlyCultivated",
                table: "Lands",
                column: "IsCurrentlyCultivated");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_IsOrganic",
                table: "Lands",
                column: "IsOrganic");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_IsPremium_IsFeatured",
                table: "Lands",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_Lands_LandType",
                table: "Lands",
                column: "LandType");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_LegalStatus",
                table: "Lands",
                column: "LegalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ListingType",
                table: "Lands",
                column: "ListingType");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Negotiable",
                table: "Lands",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_OwnershipDocument",
                table: "Lands",
                column: "OwnershipDocument");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_PricePerMeter",
                table: "Lands",
                column: "PricePerMeter");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Project",
                table: "Lands",
                column: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_RoadType",
                table: "Lands",
                column: "RoadType");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_Title",
                table: "Lands",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_TotalPrice",
                table: "Lands",
                column: "TotalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_UserId",
                table: "Lands",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lands_ViewCount",
                table: "Lands",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_LandSoilTypes_Name",
                table: "LandSoilTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandTypes_Name",
                table: "LandTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandUtilities_Name",
                table: "LandUtilities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandUtilitySelections_Utility",
                table: "LandUtilitySelections",
                column: "Utility");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateListingTypes_Name",
                table: "RealEstateListingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateProjects_Name",
                table: "RealEstateProjects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopEntrancesCounts_Name",
                table: "ShopEntrancesCounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopExchangeTargets_Name",
                table: "ShopExchangeTargets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopFacadeDirections_Name",
                table: "ShopFacadeDirections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopFacadesCounts_Name",
                table: "ShopFacadesCounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopFinishingTypes_Name",
                table: "ShopFinishingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopFloorTypes_Name",
                table: "ShopFloorTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopImages_ShopId_SortOrder",
                table: "ShopImages",
                columns: new[] { "ShopId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopInstallmentProviders_Name",
                table: "ShopInstallmentProviders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopLegalStatuses_Name",
                table: "ShopLegalStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopLicenseTypes_Name",
                table: "ShopLicenseTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopOwnershipDocuments_Name",
                table: "ShopOwnershipDocuments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopPaymentMethods_Name",
                table: "ShopPaymentMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopPropertyAges_Name",
                table: "ShopPropertyAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopReconciliationForms_Name",
                table: "ShopReconciliationForms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopRentInclusions_Name",
                table: "ShopRentInclusions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopRentInclusionSelections_Inclusion",
                table: "ShopRentInclusionSelections",
                column: "Inclusion");

            migrationBuilder.CreateIndex(
                name: "IX_ShopRentSuitableActivities_Name",
                table: "ShopRentSuitableActivities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopRentSuitableActivitySelections_Activity",
                table: "ShopRentSuitableActivitySelections",
                column: "Activity");

            migrationBuilder.CreateIndex(
                name: "IX_ShopRentTypes_Name",
                table: "ShopRentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Area",
                table: "Shops",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Center",
                table: "Shops",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_CreatedAt",
                table: "Shops",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_FacadesCount",
                table: "Shops",
                column: "FacadesCount");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_FinishingType",
                table: "Shops",
                column: "FinishingType");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_FloorType",
                table: "Shops",
                column: "FloorType");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_HasBathroom",
                table: "Shops",
                column: "HasBathroom");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_HasStorage",
                table: "Shops",
                column: "HasStorage");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_IsPremium_IsFeatured",
                table: "Shops",
                columns: new[] { "IsPremium", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_LegalStatus",
                table: "Shops",
                column: "LegalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_LicenseType",
                table: "Shops",
                column: "LicenseType");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ListingType",
                table: "Shops",
                column: "ListingType");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Negotiable",
                table: "Shops",
                column: "Negotiable");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OwnershipDocument",
                table: "Shops",
                column: "OwnershipDocument");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Price",
                table: "Shops",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Project",
                table: "Shops",
                column: "Project");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_SuitableActivity",
                table: "Shops",
                column: "SuitableActivity");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_Title",
                table: "Shops",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_UserId",
                table: "Shops",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ViewCount",
                table: "Shops",
                column: "ViewCount");

            migrationBuilder.CreateIndex(
                name: "IX_ShopSuitableActivities_Name",
                table: "ShopSuitableActivities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopUtilities_Name",
                table: "ShopUtilities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopUtilitySelections_Utility",
                table: "ShopUtilitySelections",
                column: "Utility");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentDirections");

            migrationBuilder.DropTable(
                name: "ApartmentExchangeTargets");

            migrationBuilder.DropTable(
                name: "ApartmentFeatures");

            migrationBuilder.DropTable(
                name: "ApartmentFeatureSelections");

            migrationBuilder.DropTable(
                name: "ApartmentFinishingTypes");

            migrationBuilder.DropTable(
                name: "ApartmentFloorTypes");

            migrationBuilder.DropTable(
                name: "ApartmentFurnishedStatuses");

            migrationBuilder.DropTable(
                name: "ApartmentImages");

            migrationBuilder.DropTable(
                name: "ApartmentInstallmentProviders");

            migrationBuilder.DropTable(
                name: "ApartmentLegalStatuses");

            migrationBuilder.DropTable(
                name: "ApartmentOwnershipDocuments");

            migrationBuilder.DropTable(
                name: "ApartmentOwnershipTypes");

            migrationBuilder.DropTable(
                name: "ApartmentPaymentMethods");

            migrationBuilder.DropTable(
                name: "ApartmentPropertyAges");

            migrationBuilder.DropTable(
                name: "ApartmentReceptionPieces");

            migrationBuilder.DropTable(
                name: "ApartmentReconciliationForms");

            migrationBuilder.DropTable(
                name: "ApartmentRentInclusions");

            migrationBuilder.DropTable(
                name: "ApartmentRentInclusionSelections");

            migrationBuilder.DropTable(
                name: "ApartmentRentTypes");

            migrationBuilder.DropTable(
                name: "ApartmentSuitableFor");

            migrationBuilder.DropTable(
                name: "ApartmentTypes");

            migrationBuilder.DropTable(
                name: "ApartmentViewTypes");

            migrationBuilder.DropTable(
                name: "LandAreaUnits");

            migrationBuilder.DropTable(
                name: "LandBuildingCompletionRatios");

            migrationBuilder.DropTable(
                name: "LandContractDurations");

            migrationBuilder.DropTable(
                name: "LandDirections");

            migrationBuilder.DropTable(
                name: "LandExchangeTargets");

            migrationBuilder.DropTable(
                name: "LandExistingBuildingTypes");

            migrationBuilder.DropTable(
                name: "LandFacadesCounts");

            migrationBuilder.DropTable(
                name: "LandHarvestSeasons");

            migrationBuilder.DropTable(
                name: "LandImages");

            migrationBuilder.DropTable(
                name: "LandIrrigationSources");

            migrationBuilder.DropTable(
                name: "LandLegalStatuses");

            migrationBuilder.DropTable(
                name: "LandMinimumRentPeriods");

            migrationBuilder.DropTable(
                name: "LandOwnershipDocuments");

            migrationBuilder.DropTable(
                name: "LandQualityCertificates");

            migrationBuilder.DropTable(
                name: "LandReconciliationForms");

            migrationBuilder.DropTable(
                name: "LandRentInclusions");

            migrationBuilder.DropTable(
                name: "LandRentInclusionSelections");

            migrationBuilder.DropTable(
                name: "LandRentTypes");

            migrationBuilder.DropTable(
                name: "LandRoadTypes");

            migrationBuilder.DropTable(
                name: "LandSoilTypes");

            migrationBuilder.DropTable(
                name: "LandTypes");

            migrationBuilder.DropTable(
                name: "LandUtilities");

            migrationBuilder.DropTable(
                name: "LandUtilitySelections");

            migrationBuilder.DropTable(
                name: "RealEstateListingTypes");

            migrationBuilder.DropTable(
                name: "RealEstateProjects");

            migrationBuilder.DropTable(
                name: "ShopEntrancesCounts");

            migrationBuilder.DropTable(
                name: "ShopExchangeTargets");

            migrationBuilder.DropTable(
                name: "ShopFacadeDirections");

            migrationBuilder.DropTable(
                name: "ShopFacadesCounts");

            migrationBuilder.DropTable(
                name: "ShopFinishingTypes");

            migrationBuilder.DropTable(
                name: "ShopFloorTypes");

            migrationBuilder.DropTable(
                name: "ShopImages");

            migrationBuilder.DropTable(
                name: "ShopInstallmentProviders");

            migrationBuilder.DropTable(
                name: "ShopLegalStatuses");

            migrationBuilder.DropTable(
                name: "ShopLicenseTypes");

            migrationBuilder.DropTable(
                name: "ShopOwnershipDocuments");

            migrationBuilder.DropTable(
                name: "ShopPaymentMethods");

            migrationBuilder.DropTable(
                name: "ShopPropertyAges");

            migrationBuilder.DropTable(
                name: "ShopReconciliationForms");

            migrationBuilder.DropTable(
                name: "ShopRentInclusions");

            migrationBuilder.DropTable(
                name: "ShopRentInclusionSelections");

            migrationBuilder.DropTable(
                name: "ShopRentSuitableActivities");

            migrationBuilder.DropTable(
                name: "ShopRentSuitableActivitySelections");

            migrationBuilder.DropTable(
                name: "ShopRentTypes");

            migrationBuilder.DropTable(
                name: "ShopSuitableActivities");

            migrationBuilder.DropTable(
                name: "ShopUtilities");

            migrationBuilder.DropTable(
                name: "ShopUtilitySelections");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Lands");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
