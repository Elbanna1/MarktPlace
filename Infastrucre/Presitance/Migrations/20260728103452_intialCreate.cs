using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class intialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetOtpHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PasswordResetOtpExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetTokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PasswordResetVerified = table.Column<bool>(type: "bit", nullable: false),
                    PasswordResetVerifiedExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilitySeasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilitySeasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeeHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeeHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeeProductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeeProductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeePurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeePurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CamelAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CamelBreeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelBreeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CamelGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CamelHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CamelPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CamelVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CraftsmanSpecializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftsmanSpecializations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FarmingMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmingMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FarmTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorseAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorseBreeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseBreeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorseGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorseHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorsePurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorsePurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorseTrainingLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseTrainingLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorseVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobExperienceLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExperienceLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockBreeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockBreeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockProductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockProductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LivestockVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MerchantSaleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantSaleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InstaPayIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AccountHolderName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetBreeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetBreeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetTrainingLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTrainingLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionSpecialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatAges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatBreeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatBreeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatHealthStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatHealthStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatPurposes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatVaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierSpecializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierSpecializations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WholesaleTradeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesaleTradeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AnimalType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Production = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Birds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AnimalType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Birds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Camels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    OtherBreed = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Camels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CompanyField = table.Column<int>(type: "int", nullable: false),
                    OtherCompanyField = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Craftsmen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Specialization = table.Column<int>(type: "int", nullable: false),
                    OtherSpecialization = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ExperienceLevel = table.Column<int>(type: "int", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMapsUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AdTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AdDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Craftsmen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Craftsmen_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FactoryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductionSpecialty = table.Column<int>(type: "int", nullable: false),
                    OtherSpecialty = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FarmName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FarmType = table.Column<int>(type: "int", nullable: false),
                    OtherFarmType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AreaInFeddan = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailableQuantity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AvailabilitySeason = table.Column<int>(type: "int", nullable: true),
                    FarmingMethod = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fish",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AnimalType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fish_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FruitVegetableMerchants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StallName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MerchantName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SaleType = table.Column<int>(type: "int", nullable: false),
                    ProductDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitVegetableMerchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FruitVegetableMerchants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Horses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    OtherBreed = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    TrainingLevel = table.Column<int>(type: "int", nullable: true),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Horses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobOpportunities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    JobField = table.Column<int>(type: "int", nullable: false),
                    OtherJobField = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RequiredExperience = table.Column<int>(type: "int", nullable: false),
                    WorkType = table.Column<int>(type: "int", nullable: false),
                    SalaryType = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpportunities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOpportunities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicantName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JobField = table.Column<int>(type: "int", nullable: false),
                    OtherJobField = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<int>(type: "int", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CvFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CvFileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CvFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    IntroVideoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IntroVideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Livestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    OtherBreed = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Production = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livestock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livestock_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LostFoundPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LostDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FoundDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFoundPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LostFoundPosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AnimalType = table.Column<int>(type: "int", nullable: false),
                    OtherType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherAnimals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    OtherBreed = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    TrainingLevel = table.Column<int>(type: "int", nullable: true),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SheepGoats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    OtherBreed = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    Vaccination = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SheepGoats_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SupplierType = table.Column<int>(type: "int", nullable: false),
                    OtherSupplierType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SuppliedProduct = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SupplyDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WholesaleTraders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TraderName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TradeType = table.Column<int>(type: "int", nullable: false),
                    OtherTradeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProductsName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProductDetails = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    SaleType = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMaps = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesaleTraders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WholesaleTraders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    WorkshopType = table.Column<int>(type: "int", nullable: false),
                    OtherWorkshopType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GoogleMapsUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AdTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AdDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workshops_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Centers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Centers_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ScreenshotUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ScreenshotPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RejectReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ActivationStartAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivationEndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BeeImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeeImages_Bees_BeeId",
                        column: x => x.BeeId,
                        principalTable: "Bees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BirdImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BirdImages_Birds_BirdId",
                        column: x => x.BirdId,
                        principalTable: "Birds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CamelImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CamelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamelImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CamelImages_Camels_CamelId",
                        column: x => x.CamelId,
                        principalTable: "Camels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyImages_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CraftsmanImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CraftsmanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftsmanImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CraftsmanImages_Craftsmen_CraftsmanId",
                        column: x => x.CraftsmanId,
                        principalTable: "Craftsmen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactoryImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoryImages_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmImages_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FishImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FishImages_Fish_FishId",
                        column: x => x.FishId,
                        principalTable: "Fish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FruitVegetableMerchantImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FruitVegetableMerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitVegetableMerchantImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FruitVegetableMerchantImages_FruitVegetableMerchants_FruitVegetableMerchantId",
                        column: x => x.FruitVegetableMerchantId,
                        principalTable: "FruitVegetableMerchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorseImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HorseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorseImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorseImages_Horses_HorseId",
                        column: x => x.HorseId,
                        principalTable: "Horses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOpportunityImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobOpportunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpportunityImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOpportunityImages_JobOpportunities_JobOpportunityId",
                        column: x => x.JobOpportunityId,
                        principalTable: "JobOpportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivestockImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LivestockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivestockImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LivestockImages_Livestock_LivestockId",
                        column: x => x.LivestockId,
                        principalTable: "Livestock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LostFoundComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFoundComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LostFoundComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LostFoundComments_LostFoundPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "LostFoundPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LostFoundImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFoundImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LostFoundImages_LostFoundPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "LostFoundPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LostFoundLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostFoundLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LostFoundLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LostFoundLikes_LostFoundPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "LostFoundPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherAnimalImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtherAnimalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAnimalImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherAnimalImages_OtherAnimals_OtherAnimalId",
                        column: x => x.OtherAnimalId,
                        principalTable: "OtherAnimals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetImages_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SheepGoatImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SheepGoatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheepGoatImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SheepGoatImages_SheepGoats_SheepGoatId",
                        column: x => x.SheepGoatId,
                        principalTable: "SheepGoats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierImages_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WholesaleTraderImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WholesaleTraderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesaleTraderImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WholesaleTraderImages_WholesaleTraders_WholesaleTraderId",
                        column: x => x.WholesaleTraderId,
                        principalTable: "WholesaleTraders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopImages_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Negotiable = table.Column<bool>(type: "bit", nullable: false),
                    ListingType = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstPublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManufacturingYear = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kilometers = table.Column<int>(type: "int", nullable: true),
                    Transmission = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<int>(type: "int", nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: true),
                    EngineCC = table.Column<int>(type: "int", nullable: true),
                    LicenseValid = table.Column<bool>(type: "bit", nullable: true),
                    LicenseDuration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstOwner = table.Column<bool>(type: "bit", nullable: true),
                    BodyType = table.Column<int>(type: "int", nullable: true),
                    DoorsCount = table.Column<int>(type: "int", nullable: true),
                    CoolingType = table.Column<int>(type: "int", nullable: true),
                    MachineType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkingHours = table.Column<int>(type: "int", nullable: true),
                    Power = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RentPeriod = table.Column<int>(type: "int", nullable: true),
                    MinimumRentPeriod = table.Column<int>(type: "int", nullable: true),
                    DriverIncluded = table.Column<bool>(type: "bit", nullable: true),
                    DamageLevel = table.Column<int>(type: "int", nullable: true),
                    IsRunning = table.Column<bool>(type: "bit", nullable: true),
                    SellAsParts = table.Column<bool>(type: "bit", nullable: true),
                    InterestedIn = table.Column<int>(type: "int", nullable: true),
                    DifferencePayment = table.Column<bool>(type: "bit", nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    GoogleMapsUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProductionSpecialty = table.Column<int>(type: "int", nullable: true),
                    OtherProductionSpecialty = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FarmType = table.Column<int>(type: "int", nullable: true),
                    OtherFarmType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FarmingMethod = table.Column<int>(type: "int", nullable: true),
                    AvailabilitySeason = table.Column<int>(type: "int", nullable: true),
                    CompanyField = table.Column<int>(type: "int", nullable: true),
                    OtherCompanyField = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SupplierType = table.Column<int>(type: "int", nullable: true),
                    OtherSupplierType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TradeType = table.Column<int>(type: "int", nullable: true),
                    OtherTradeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SaleType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisements_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisements_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisements_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementFeatures",
                columns: table => new
                {
                    AdvertisementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementFeatures", x => new { x.AdvertisementId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_AdvertisementFeatures_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdvertisementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementViews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewerUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementViews_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AvailabilitySeasons",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "متوفر الآن" },
                    { 2, "طوال العام" },
                    { 3, "موسمي" }
                });

            migrationBuilder.InsertData(
                table: "BeeHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "BeeProductions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عسل", "Honey" },
                    { 2, "شمع", "Wax" },
                    { 3, "غذاء ملكات", "Royal jelly" },
                    { 4, "عكبر", "Propolis" },
                    { 5, "حبوب لقاح", "Pollen" }
                });

            migrationBuilder.InsertData(
                table: "BeePurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "إنتاج عسل", "Honey production" },
                    { 2, "تلقيح المحاصيل", "Pollination" },
                    { 3, "تربية وإنتاج", "Breeding" },
                    { 4, "إنتاج ملكات", "Queen production" },
                    { 5, "بيع", "Sale" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "BeeTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كرنيولي", "Carniolan" },
                    { 2, "إيطالي", "Italian" },
                    { 3, "بلدي مصري", "Egyptian Baladi" },
                    { 4, "بكفاست", "Buckfast" },
                    { 5, "قوقازي", "Caucasian" },
                    { 6, "هجين", "Hybrid" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "BirdAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "BirdGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "BirdHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "BirdPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تربية", "Breeding" },
                    { 2, "إنتاج بيض", "Egg production" },
                    { 3, "إنتاج لحوم", "Meat production" },
                    { 4, "زينة", "Ornamental" },
                    { 5, "صيد", "Hunting" },
                    { 6, "بيع", "Sale" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "BirdTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "دجاج", "Chickens" },
                    { 2, "دجاج بلدي", "Baladi Chickens" },
                    { 3, "بط", "Ducks" },
                    { 4, "إوز", "Geese" },
                    { 5, "رومي", "Turkey" },
                    { 6, "حمام", "Pigeons" },
                    { 7, "سمان", "Quail" },
                    { 8, "نعام", "Ostrich" },
                    { 9, "كناري", "Canary" },
                    { 10, "ببغاء", "Parrots" },
                    { 11, "طيور الحب", "Love Birds" },
                    { 12, "صقور", "Falcons" },
                    { 13, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "BirdVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "CamelAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "CamelBreeds",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مغربي", "Maghrabi" },
                    { 2, "سوداني", "Sudani" },
                    { 3, "صومالي", "Somali" },
                    { 4, "بشاري", "Bishari" },
                    { 5, "فلاحي", "Falahi" },
                    { 6, "هجن سباق", "Racing (Hijin)" },
                    { 7, "خليط", "Crossbreed" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "CamelGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "CamelHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "CamelPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تربية", "Breeding" },
                    { 2, "سباق الهجن", "Camel racing" },
                    { 3, "إنتاج ألبان", "Milk production" },
                    { 4, "إنتاج لحوم", "Meat production" },
                    { 5, "نقل", "Transport" },
                    { 6, "بيع", "Sale" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "CamelVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "NameAr" },
                values: new object[,]
                {
                    { 1, "Cars", "سيارات" },
                    { 2, "Workshops & Craftsmen", "الورش والحرفيين" },
                    { 3, "Lost & Found", "المفقودات" },
                    { 4, "Business", "رجال أعمال" },
                    { 5, "Jobs", "الوظائف" },
                    { 6, "Animals", "الحيوانات" }
                });

            migrationBuilder.InsertData(
                table: "CompanyFields",
                columns: new[] { "Id", "Code", "Group", "GroupAr", "Name" },
                values: new object[,]
                {
                    { 1, "GeneralContracting", "Construction", "مقاولات", "General Contracting" },
                    { 2, "Finishing", "Construction", "مقاولات", "Finishing" },
                    { 3, "Insulation", "Construction", "مقاولات", "Insulation" },
                    { 4, "Demolition", "Construction", "مقاولات", "Demolition" },
                    { 5, "Excavation", "Construction", "مقاولات", "Excavation" },
                    { 6, "Concrete", "Construction", "مقاولات", "Concrete" },
                    { 7, "DomesticShipping", "Logistics", "نقل ولوجستيات", "Domestic Shipping" },
                    { 8, "InternationalShipping", "Logistics", "نقل ولوجستيات", "International Shipping" },
                    { 9, "FurnitureMoving", "Logistics", "نقل ولوجستيات", "Furniture Moving" },
                    { 10, "Warehousing", "Logistics", "نقل ولوجستيات", "Warehousing" },
                    { 11, "Import", "Commercial", "تجارية", "Import" },
                    { 12, "Export", "Commercial", "تجارية", "Export" },
                    { 13, "ImportAndExport", "Commercial", "تجارية", "Import & Export" },
                    { 14, "Distribution", "Commercial", "تجارية", "Distribution" },
                    { 15, "Software", "Technology", "تكنولوجيا", "Software" },
                    { 16, "WebsiteDevelopment", "Technology", "تكنولوجيا", "Website Development" },
                    { 17, "MobileApplications", "Technology", "تكنولوجيا", "Mobile Applications" },
                    { 18, "Networking", "Technology", "تكنولوجيا", "Networking" },
                    { 19, "CCTV", "Technology", "تكنولوجيا", "CCTV" },
                    { 20, "SecuritySystems", "Technology", "تكنولوجيا", "Security Systems" },
                    { 21, "Advertising", "Marketing", "تسويق ودعاية", "Advertising" },
                    { 22, "GraphicDesign", "Marketing", "تسويق ودعاية", "Graphic Design" },
                    { 23, "Printing", "Marketing", "تسويق ودعاية", "Printing" },
                    { 24, "Photography", "Marketing", "تسويق ودعاية", "Photography" },
                    { 25, "DigitalMarketing", "Marketing", "تسويق ودعاية", "Digital Marketing" },
                    { 26, "Accounting", "Financial", "مالية ومحاسبة", "Accounting" },
                    { 27, "Auditing", "Financial", "مالية ومحاسبة", "Auditing" },
                    { 28, "TaxConsulting", "Financial", "مالية ومحاسبة", "Tax Consulting" },
                    { 29, "LawFirm", "Legal", "قانونية", "Law Firm" },
                    { 30, "LegalConsulting", "Legal", "قانونية", "Legal Consulting" },
                    { 31, "EngineeringOffice", "Engineering", "هندسية", "Engineering Office" },
                    { 32, "Architecture", "Engineering", "هندسية", "Architecture" },
                    { 33, "EngineeringSupervision", "Engineering", "هندسية", "Engineering Supervision" },
                    { 34, "Surveying", "Engineering", "هندسية", "Surveying" },
                    { 35, "MedicalSupplies", "Medical", "طبية", "Medical Supplies" },
                    { 36, "MedicalServices", "Medical", "طبية", "Medical Services" },
                    { 37, "Training", "Education", "تعليم وتدريب", "Training" },
                    { 38, "Courses", "Education", "تعليم وتدريب", "Courses" },
                    { 39, "Nurseries", "Education", "تعليم وتدريب", "Nurseries" },
                    { 40, "EducationalCenters", "Education", "تعليم وتدريب", "Educational Centers" },
                    { 41, "CleaningCompanies", "Cleaning & Maintenance", "نظافة وصيانة", "Cleaning Companies" },
                    { 42, "PestControl", "Cleaning & Maintenance", "نظافة وصيانة", "Pest Control" },
                    { 43, "GeneralMaintenance", "Cleaning & Maintenance", "نظافة وصيانة", "General Maintenance" },
                    { 44, "Other", "Other", "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "CraftsmanSpecializations",
                columns: new[] { "Id", "GroupName", "Name" },
                values: new object[,]
                {
                    { 1, "النجارة", "نجار أثاث" },
                    { 2, "النجارة", "نجار أبواب" },
                    { 3, "النجارة", "نجار مطابخ" },
                    { 4, "السباكة", "سباك" },
                    { 5, "السباكة", "تأسيس سباكة" },
                    { 6, "السباكة", "صيانة سباكة" },
                    { 7, "الكهرباء", "كهربائي منازل" },
                    { 8, "الكهرباء", "كهربائي صناعي" },
                    { 9, "الدهانات", "نقاش" },
                    { 10, "الدهانات", "دهانات ديكورية" },
                    { 11, "الأرضيات", "سيراميك" },
                    { 12, "الأرضيات", "بورسلين" },
                    { 13, "الأرضيات", "رخام" },
                    { 14, "الأرضيات", "جرانيت" },
                    { 15, "الأرضيات", "باركيه" },
                    { 16, "الجبس", "جبس بورد" },
                    { 17, "الجبس", "ديكورات جبس" },
                    { 18, "الحدادة", "حداد" },
                    { 19, "الحدادة", "حداد كريتال" },
                    { 20, "الألوميتال", "ألوميتال" },
                    { 21, "الزجاج", "زجاج" },
                    { 22, "اللحام", "لحام كهرباء" },
                    { 23, "اللحام", "لحام أرجون" },
                    { 24, "التكييف", "فني تكييف" },
                    { 25, "التكييف", "فني تبريد" },
                    { 26, "الأجهزة المنزلية", "صيانة غسالات" },
                    { 27, "الأجهزة المنزلية", "صيانة ثلاجات" },
                    { 28, "الأجهزة المنزلية", "صيانة بوتاجازات" },
                    { 29, "الأجهزة المنزلية", "صيانة سخانات" },
                    { 30, "المطابخ", "تركيب مطابخ" },
                    { 31, "الستائر", "تركيب ستائر" },
                    { 32, "الطاقة الشمسية", "تركيب طاقة شمسية" },
                    { 33, "التنظيف", "تنظيف منازل" },
                    { 34, "التنظيف", "تنظيف شركات" },
                    { 35, "نقل الأثاث", "فك وتركيب أثاث" },
                    { 36, "نقل الأثاث", "نقل أثاث" },
                    { 37, "أخرى", "أخرى" }
                });

            migrationBuilder.InsertData(
                table: "EducationLevels",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بدون مؤهل", "No qualification" },
                    { 2, "دبلوم", "Diploma" },
                    { 3, "ثانوية عامة", "High school" },
                    { 4, "معهد", "Institute" },
                    { 5, "بكالوريوس", "Bachelor" },
                    { 6, "ماجستير", "Master" },
                    { 7, "دكتوراه", "Doctorate" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ExperienceLevels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "أقل من سنة" },
                    { 2, "1-3 سنوات" },
                    { 3, "3-5 سنوات" },
                    { 4, "5-10 سنوات" },
                    { 5, "أكثر من 10 سنوات" }
                });

            migrationBuilder.InsertData(
                table: "FarmTypes",
                columns: new[] { "Id", "Group", "GroupAr", "Name" },
                values: new object[,]
                {
                    { 1, "نباتية", "نباتية", "مزرعة خضروات" },
                    { 2, "نباتية", "نباتية", "مزرعة فواكه" },
                    { 3, "نباتية", "نباتية", "مزرعة موالح" },
                    { 4, "نباتية", "نباتية", "مزرعة نخيل" },
                    { 5, "نباتية", "نباتية", "مزرعة زيتون" },
                    { 6, "نباتية", "نباتية", "مزرعة أعشاب طبية وعطرية" },
                    { 7, "نباتية", "نباتية", "مزرعة زهور ونباتات زينة" },
                    { 8, "نباتية", "نباتية", "مزرعة محاصيل حقلية" },
                    { 9, "نباتية", "نباتية", "صوبة زراعية" },
                    { 10, "حيوانية", "حيوانية", "مزرعة دواجن" },
                    { 11, "حيوانية", "حيوانية", "مزرعة مواشي" },
                    { 12, "حيوانية", "حيوانية", "مزرعة أغنام" },
                    { 13, "حيوانية", "حيوانية", "مزرعة ماعز" },
                    { 14, "حيوانية", "حيوانية", "مزرعة جمال" },
                    { 15, "حيوانية", "حيوانية", "مزرعة أرانب" },
                    { 16, "إنتاج حيواني", "إنتاج حيواني", "مزرعة ألبان" },
                    { 17, "إنتاج حيواني", "إنتاج حيواني", "مزرعة بيض" },
                    { 18, "إنتاج حيواني", "إنتاج حيواني", "تسمين عجول" },
                    { 19, "إنتاج حيواني", "إنتاج حيواني", "تسمين دواجن" },
                    { 20, "مزارع مائية", "مزارع مائية", "مزرعة أسماك" },
                    { 21, "مزارع مائية", "مزارع مائية", "مزرعة جمبري" },
                    { 22, "أخرى", "أخرى", "أخرى" }
                });

            migrationBuilder.InsertData(
                table: "FarmingMethods",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "Organic", "عضوي" },
                    { 2, "Conventional", "تقليدي" },
                    { 3, "Mixed", "مختلط" },
                    { 4, "NotSpecified", "غير محدد" }
                });

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ABS" },
                    { 2, "EBD" },
                    { 3, "ESP" },
                    { 4, "TCS (مانع الانزلاق)" },
                    { 5, "Airbags" },
                    { 6, "كاميرا خلفية" },
                    { 7, "كاميرات 360°" },
                    { 8, "حساسات ركن أمامية" },
                    { 9, "حساسات ركن خلفية" },
                    { 10, "شاشة" },
                    { 11, "Apple CarPlay" },
                    { 12, "Android Auto" },
                    { 13, "بلوتوث" },
                    { 14, "GPS" },
                    { 15, "مثبت سرعة" },
                    { 16, "مثبت سرعة تكيفي" },
                    { 17, "فتحة سقف" },
                    { 18, "سقف بانوراما" },
                    { 19, "تكييف" },
                    { 20, "تكييف أوتوماتيك" },
                    { 21, "مقاعد كهربائية" },
                    { 22, "مقاعد مدفأة" },
                    { 23, "مقاعد مبردة" },
                    { 24, "فرش جلد" },
                    { 25, "عجلة قيادة متعددة الوظائف" },
                    { 26, "تشغيل بدون مفتاح" },
                    { 27, "بصمة" },
                    { 28, "ريموت" },
                    { 29, "مرايا كهربائية" },
                    { 30, "طي مرايا كهربائي" },
                    { 31, "زجاج كهربائي" },
                    { 32, "إضاءة LED" },
                    { 33, "فوانيس ضباب" },
                    { 34, "جنوط" },
                    { 35, "حساس إضاءة" },
                    { 36, "حساس مطر" },
                    { 37, "TPMS" },
                    { 38, "مساعد صعود المرتفعات" },
                    { 39, "مثبت نزول المنحدرات" },
                    { 40, "إنذار ضد السرقة" },
                    { 41, "سنتر لوك" }
                });

            migrationBuilder.InsertData(
                table: "FishAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "زريعة", "Fry" },
                    { 2, "إصبعيات", "Juvenile" },
                    { 3, "بالغ", "Adult" },
                    { 4, "أمهات", "Breeder" }
                });

            migrationBuilder.InsertData(
                table: "FishHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "FishPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "زينة", "Ornamental" },
                    { 2, "استهلاك", "Consumption" },
                    { 3, "استزراع سمكي", "Fish farming" },
                    { 4, "تربية وإنتاج", "Breeding" },
                    { 5, "بيع", "Sale" },
                    { 6, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "FishTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بلطي", "Tilapia" },
                    { 2, "قرموط", "Catfish" },
                    { 3, "بوري", "Mullet" },
                    { 4, "مبروك", "Carp" },
                    { 5, "قاروص", "Sea Bass" },
                    { 6, "دنيس", "Sea Bream" },
                    { 7, "جمبري", "Shrimp" },
                    { 8, "أسماك زينة", "Ornamental Fish" },
                    { 9, "سمك ذهبي", "Goldfish" },
                    { 10, "كوي", "Koi" },
                    { 11, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "الفيوم" });

            migrationBuilder.InsertData(
                table: "HorseAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "HorseBreeds",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "عربي أصيل", "Arabian Horse" },
                    { 2, "عربي مصري", "Egyptian Arabian" },
                    { 3, "إنجليزي أصيل", "Thoroughbred" },
                    { 4, "بربري", "Barb" },
                    { 5, "أندلسي", "Andalusian" },
                    { 6, "فريزيان", "Friesian" },
                    { 7, "كوارتر", "Quarter Horse" },
                    { 8, "بوني", "Pony" },
                    { 9, "خليط", "Crossbreed" },
                    { 10, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HorseGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "HorseHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "HorsePurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ركوب", "Riding" },
                    { 2, "سباق", "Racing" },
                    { 3, "تربية وإنتاج", "Breeding" },
                    { 4, "عروض", "Shows" },
                    { 5, "عمل", "Work" },
                    { 6, "بيع", "Sale" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "HorseTrainingLevels",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "غير مدرب", "Untrained" },
                    { 2, "تدريب أساسي", "Basic training" },
                    { 3, "تدريب متقدم", "Advanced training" },
                    { 4, "تدريب احترافي", "Professional training" }
                });

            migrationBuilder.InsertData(
                table: "HorseVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "JobExperienceLevels",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بدون خبرة", "No experience" },
                    { 2, "أقل من سنة", "Less than a year" },
                    { 3, "1 - 3 سنوات", "1 - 3 years" },
                    { 4, "3 - 5 سنوات", "3 - 5 years" },
                    { 5, "أكثر من 5 سنوات", "More than 5 years" }
                });

            migrationBuilder.InsertData(
                table: "JobFields",
                columns: new[] { "Id", "Group", "GroupAr", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "Administration & Business", "الإدارة والأعمال", "مدير", "Manager" },
                    { 2, "Administration & Business", "الإدارة والأعمال", "سكرتير", "Secretary" },
                    { 3, "Administration & Business", "الإدارة والأعمال", "موظف إداري", "Administrative Employee" },
                    { 4, "Administration & Business", "الإدارة والأعمال", "خدمة عملاء", "Customer Service" },
                    { 5, "Administration & Business", "الإدارة والأعمال", "مبيعات", "Sales" },
                    { 6, "Administration & Business", "الإدارة والأعمال", "محاسب", "Accountant" },
                    { 7, "Administration & Business", "الإدارة والأعمال", "موارد بشرية", "Human Resources" },
                    { 8, "Programming & Technology", "البرمجة والتكنولوجيا", "مبرمج", "Programmer" },
                    { 9, "Programming & Technology", "البرمجة والتكنولوجيا", "مطور مواقع", "Web Developer" },
                    { 10, "Programming & Technology", "البرمجة والتكنولوجيا", "مطور تطبيقات", "Application Developer" },
                    { 11, "Programming & Technology", "البرمجة والتكنولوجيا", "مصمم UI/UX", "UI/UX Designer" },
                    { 12, "Programming & Technology", "البرمجة والتكنولوجيا", "دعم فني", "Technical Support" },
                    { 13, "Programming & Technology", "البرمجة والتكنولوجيا", "شبكات", "Networks" },
                    { 14, "Engineering", "الهندسة", "مهندس مدني", "Civil Engineer" },
                    { 15, "Engineering", "الهندسة", "مهندس معماري", "Architectural Engineer" },
                    { 16, "Engineering", "الهندسة", "مهندس كهرباء", "Electrical Engineer" },
                    { 17, "Engineering", "الهندسة", "مهندس ميكانيكا", "Mechanical Engineer" },
                    { 18, "Engineering", "الهندسة", "فني هندسي", "Engineering Technician" },
                    { 19, "Medicine & Health", "الطب والصحة", "طبيب", "Doctor" },
                    { 20, "Medicine & Health", "الطب والصحة", "ممرض", "Nurse" },
                    { 21, "Medicine & Health", "الطب والصحة", "صيدلي", "Pharmacist" },
                    { 22, "Medicine & Health", "الطب والصحة", "مساعد طبي", "Medical Assistant" },
                    { 23, "Education", "التعليم", "مدرس", "Teacher" },
                    { 24, "Education", "التعليم", "مدرس خصوصي", "Private Tutor" },
                    { 25, "Education", "التعليم", "محاضر", "Lecturer" },
                    { 26, "Education", "التعليم", "إداري تعليم", "Education Administrator" },
                    { 27, "Crafts & Trades", "الحرف والمهن", "كهربائي", "Electrician" },
                    { 28, "Crafts & Trades", "الحرف والمهن", "سباك", "Plumber" },
                    { 29, "Crafts & Trades", "الحرف والمهن", "نجار", "Carpenter" },
                    { 30, "Crafts & Trades", "الحرف والمهن", "حداد", "Blacksmith" },
                    { 31, "Crafts & Trades", "الحرف والمهن", "فني تكييف", "Air Conditioning Technician" },
                    { 32, "Crafts & Trades", "الحرف والمهن", "سائق", "Driver" },
                    { 33, "Crafts & Trades", "الحرف والمهن", "عامل إنتاج", "Production Worker" },
                    { 34, "Restaurants & Hotels", "المطاعم والفنادق", "شيف", "Chef" },
                    { 35, "Restaurants & Hotels", "المطاعم والفنادق", "مساعد شيف", "Assistant Chef" },
                    { 36, "Restaurants & Hotels", "المطاعم والفنادق", "ويتر", "Waiter" },
                    { 37, "Restaurants & Hotels", "المطاعم والفنادق", "كاشير", "Cashier" },
                    { 38, "Restaurants & Hotels", "المطاعم والفنادق", "عامل مطعم", "Restaurant Worker" },
                    { 39, "Agriculture", "الزراعة", "مهندس زراعي", "Agricultural Engineer" },
                    { 40, "Agriculture", "الزراعة", "عامل مزرعة", "Farm Worker" },
                    { 41, "Agriculture", "الزراعة", "مشرف زراعي", "Agricultural Supervisor" },
                    { 42, "Other", "أخرى", "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ListingTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "بيع" },
                    { 2, "إيجار" },
                    { 3, "بدل" },
                    { 4, "حوادث" }
                });

            migrationBuilder.InsertData(
                table: "LivestockAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "LivestockBreeds",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "بقر بلدي", "Baladi Cow" },
                    { 2, "بقر فريزيان", "Friesian Cow" },
                    { 3, "بقر هولشتاين", "Holstein Cow" },
                    { 4, "بقر سيمنتال", "Simmental Cow" },
                    { 5, "بقر براون سويس", "Brown Swiss Cow" },
                    { 6, "جاموس بلدي", "Baladi Buffalo" },
                    { 7, "جاموس إيطالي", "Italian Buffalo" },
                    { 8, "عجول تسمين", "Fattening Calves" },
                    { 9, "خليط", "Crossbreed" },
                    { 10, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LivestockGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "LivestockHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "LivestockProductions",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ألبان", "Milk" },
                    { 2, "لحوم", "Meat" },
                    { 3, "ألبان ولحوم", "Milk & meat" },
                    { 4, "جلود", "Leather" },
                    { 5, "لا يوجد", "None" }
                });

            migrationBuilder.InsertData(
                table: "LivestockPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تربية", "Breeding" },
                    { 2, "تسمين", "Fattening" },
                    { 3, "إنتاج ألبان", "Milk production" },
                    { 4, "إنتاج لحوم", "Meat production" },
                    { 5, "عمل", "Work" },
                    { 6, "بيع", "Sale" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "LivestockVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "MerchantSaleTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "قطاعي", "Retail" },
                    { 2, "جملة", "Wholesale" },
                    { 3, "قطاعي وجملة", "Both" }
                });

            migrationBuilder.InsertData(
                table: "OtherAnimalAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "OtherAnimalGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "OtherAnimalHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "OtherAnimalPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تربية", "Breeding" },
                    { 2, "عمل", "Work" },
                    { 3, "زينة", "Ornamental" },
                    { 4, "بيع", "Sale" },
                    { 5, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "OtherAnimalTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "حمير", "Donkeys" },
                    { 2, "بغال", "Mules" },
                    { 3, "أرانب", "Rabbits" },
                    { 4, "خنزير غينيا", "Guinea Pig" },
                    { 5, "غزلان", "Deer" },
                    { 6, "زواحف", "Reptiles" },
                    { 7, "قرود", "Monkeys" },
                    { 8, "حشرات مزرعية", "Farm Insects" },
                    { 9, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "OtherAnimalVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "AccountHolderName", "AccountNumber", "ArabicName", "BankName", "CreatedAt", "DisplayOrder", "Iban", "InstaPayIdentifier", "Instructions", "IsActive", "Name", "PhoneNumber", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, null, "فودافون كاش", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, "حوّل المبلغ إلى رقم المحفظة ثم ارفع صورة إيصال التحويل.", true, "Vodafone Cash", "01026568617", 1, null },
                    { 2, null, null, "إنستا باي", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, "01026568617", "حوّل المبلغ عبر إنستا باي إلى المعرّف الموضح ثم ارفع صورة الإيصال.", true, "InstaPay", null, 2, null },
                    { 3, "MarkatPlace", "0000000000000000", "تحويل بنكي", "بنك مصر", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, "حوّل المبلغ إلى الحساب البنكي الموضح ثم ارفع صورة إيصال التحويل.", true, "Bank Transfer", null, 3, null }
                });

            migrationBuilder.InsertData(
                table: "PetAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "PetBreeds",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "كلاب", "Dogs" },
                    { 2, "جيرمن شيبرد", "German Shepherd" },
                    { 3, "هاسكي", "Husky" },
                    { 4, "جولدن ريتريفر", "Golden Retriever" },
                    { 5, "جريفون", "Griffon" },
                    { 6, "قطط", "Cats" },
                    { 7, "قط شيرازي", "Persian Cat" },
                    { 8, "قط سيامي", "Siamese Cat" },
                    { 9, "قط هيمالايا", "Himalayan Cat" },
                    { 10, "أرانب", "Rabbits" },
                    { 11, "هامستر", "Hamster" },
                    { 12, "سلاحف", "Turtles" },
                    { 13, "سنجاب", "Squirrel" },
                    { 14, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "PetGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "PetHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "PetPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تربية منزلية", "Companionship" },
                    { 2, "تربية وإنتاج", "Breeding" },
                    { 3, "حراسة", "Guarding" },
                    { 4, "عروض", "Shows" },
                    { 5, "بيع", "Sale" },
                    { 6, "تبني", "Adoption" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "PetTrainingLevels",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "غير مدرب", "Untrained" },
                    { 2, "تدريب أساسي", "Basic training" },
                    { 3, "تدريب متقدم", "Advanced training" },
                    { 4, "تدريب احترافي", "Professional training" }
                });

            migrationBuilder.InsertData(
                table: "PetVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "ProductionSpecialties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "مواد غذائية" },
                    { 2, "مشروبات" },
                    { 3, "ألبان ومنتجاتها" },
                    { 4, "مخابز وحلويات" },
                    { 5, "تعبئة وتغليف" },
                    { 6, "بلاستيك" },
                    { 7, "ورق وكرتون" },
                    { 8, "طباعة" },
                    { 9, "غزل ونسيج" },
                    { 10, "ملابس جاهزة" },
                    { 11, "أحذية" },
                    { 12, "جلود" },
                    { 13, "أثاث" },
                    { 14, "أخشاب" },
                    { 15, "ألومنيوم" },
                    { 16, "حديد وصلب" },
                    { 17, "تشغيل معادن" },
                    { 18, "ماكينات ومعدات" },
                    { 19, "أجهزة كهربائية" },
                    { 20, "إلكترونيات" },
                    { 21, "كابلات وأسلاك" },
                    { 22, "أدوات صحية" },
                    { 23, "سيراميك" },
                    { 24, "رخام وجرانيت" },
                    { 25, "زجاج" },
                    { 26, "أسمنت ومواد بناء" },
                    { 27, "دهانات" },
                    { 28, "كيماويات" },
                    { 29, "منظفات" },
                    { 30, "مستحضرات تجميل" },
                    { 31, "أدوية" },
                    { 32, "مستلزمات طبية" },
                    { 33, "أسمدة" },
                    { 34, "مبيدات" },
                    { 35, "أعلاف" },
                    { 36, "منتجات زراعية" },
                    { 37, "إعادة تدوير" },
                    { 38, "أخرى" }
                });

            migrationBuilder.InsertData(
                table: "SalaryTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "قابل للتفاوض", "Negotiable" },
                    { 2, "تحديد الراتب", "Specified" }
                });

            migrationBuilder.InsertData(
                table: "SaleTypes",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "Retail", "قطاعي" },
                    { 2, "Wholesale", "جملة" },
                    { 3, "Both", "قطاعي وجملة" }
                });

            migrationBuilder.InsertData(
                table: "SheepGoatAges",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مولود", "Newborn" },
                    { 2, "صغير", "Young" },
                    { 3, "بالغ", "Adult" },
                    { 4, "كبير السن", "Old" }
                });

            migrationBuilder.InsertData(
                table: "SheepGoatBreeds",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "برقي", "Barki" },
                    { 2, "رحماني", "Rahmani" },
                    { 3, "أوسيمي", "Ossimi" },
                    { 4, "عواسي", "Awassi" },
                    { 5, "نعيمي", "Naimi" },
                    { 6, "حري", "Harri" },
                    { 7, "ماعز بلدي", "Baladi Goat" },
                    { 8, "ماعز زرايبي", "Zaraibi Goat" },
                    { 9, "ماعز شامي", "Damascus Goat" },
                    { 10, "ماعز بور", "Boer Goat" },
                    { 11, "خليط", "Crossbreed" },
                    { 12, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "SheepGoatGenders",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ذكر", "Male" },
                    { 2, "أنثى", "Female" },
                    { 3, "مختلط", "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "SheepGoatHealthStatuses",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "ممتازة", "Excellent" },
                    { 2, "جيدة", "Good" },
                    { 3, "مقبولة", "Fair" },
                    { 4, "تحت العلاج", "Under treatment" }
                });

            migrationBuilder.InsertData(
                table: "SheepGoatPurposes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "تربية", "Breeding" },
                    { 2, "تسمين", "Fattening" },
                    { 3, "إنتاج ألبان", "Milk production" },
                    { 4, "إنتاج لحوم", "Meat production" },
                    { 5, "أضاحي", "Sacrifice" },
                    { 6, "بيع", "Sale" },
                    { 7, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "SheepGoatVaccinations",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "محصن بالكامل", "Fully vaccinated" },
                    { 2, "محصن جزئيًا", "Partially vaccinated" },
                    { 3, "غير محصن", "Not vaccinated" },
                    { 4, "غير معروف", "Unknown" }
                });

            migrationBuilder.InsertData(
                table: "SupplierSpecializations",
                columns: new[] { "Id", "Group", "GroupAr", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "Industrial Suppliers", "موردو الصناعة", "خامات بلاستيك", "Plastic Raw Materials" },
                    { 2, "Industrial Suppliers", "موردو الصناعة", "خامات كيماوية", "Chemical Raw Materials" },
                    { 3, "Industrial Suppliers", "موردو الصناعة", "خامات غذائية", "Food Raw Materials" },
                    { 4, "Industrial Suppliers", "موردو الصناعة", "مواد تعبئة وتغليف", "Packaging Materials" },
                    { 5, "Industrial Suppliers", "موردو الصناعة", "منسوجات", "Textiles" },
                    { 6, "Industrial Suppliers", "موردو الصناعة", "جلود", "Leather" },
                    { 7, "Industrial Suppliers", "موردو الصناعة", "أخشاب", "Wood" },
                    { 8, "Industrial Suppliers", "موردو الصناعة", "حديد وصلب", "Steel" },
                    { 9, "Industrial Suppliers", "موردو الصناعة", "ألومنيوم", "Aluminum" },
                    { 10, "Industrial Suppliers", "موردو الصناعة", "زجاج", "Glass" },
                    { 11, "Industrial Suppliers", "موردو الصناعة", "دهانات", "Paint" },
                    { 12, "Industrial Suppliers", "موردو الصناعة", "مواد بناء", "Building Materials" },
                    { 13, "Industrial Suppliers", "موردو الصناعة", "رخام وجرانيت", "Marble & Granite" },
                    { 14, "Industrial Suppliers", "موردو الصناعة", "مستلزمات مصانع", "Factory Supplies" },
                    { 15, "Industrial Suppliers", "موردو الصناعة", "قطع غيار ماكينات", "Machine Spare Parts" },
                    { 16, "Industrial Suppliers", "موردو الصناعة", "معدات صناعية", "Industrial Equipment" },
                    { 17, "Agricultural Suppliers", "موردو الزراعة", "بذور", "Seeds" },
                    { 18, "Agricultural Suppliers", "موردو الزراعة", "شتلات", "Seedlings" },
                    { 19, "Agricultural Suppliers", "موردو الزراعة", "أسمدة", "Fertilizers" },
                    { 20, "Agricultural Suppliers", "موردو الزراعة", "مبيدات", "Pesticides" },
                    { 21, "Agricultural Suppliers", "موردو الزراعة", "أعلاف", "Animal Feed" },
                    { 22, "Agricultural Suppliers", "موردو الزراعة", "معدات زراعية", "Agricultural Equipment" },
                    { 23, "Agricultural Suppliers", "موردو الزراعة", "أنظمة ري", "Irrigation Systems" },
                    { 24, "Agricultural Suppliers", "موردو الزراعة", "صوب زراعية", "Greenhouses" },
                    { 25, "Agricultural Suppliers", "موردو الزراعة", "مستلزمات مزارع", "Farm Supplies" },
                    { 26, "Livestock Suppliers", "موردو الثروة الحيوانية", "أعلاف", "Animal Feed" },
                    { 27, "Livestock Suppliers", "موردو الثروة الحيوانية", "أدوية بيطرية", "Veterinary Medicines" },
                    { 28, "Livestock Suppliers", "موردو الثروة الحيوانية", "مستلزمات دواجن", "Poultry Supplies" },
                    { 29, "Livestock Suppliers", "موردو الثروة الحيوانية", "مستلزمات مزارع", "Farm Supplies" },
                    { 30, "Construction Suppliers", "موردو مواد البناء", "أسمنت", "Cement" },
                    { 31, "Construction Suppliers", "موردو مواد البناء", "حديد تسليح", "Steel" },
                    { 32, "Construction Suppliers", "موردو مواد البناء", "طوب", "Bricks" },
                    { 33, "Construction Suppliers", "موردو مواد البناء", "أدوات صحية", "Sanitary Ware" },
                    { 34, "Construction Suppliers", "موردو مواد البناء", "مواد كهربائية", "Electrical Materials" },
                    { 35, "Construction Suppliers", "موردو مواد البناء", "معدات بناء", "Construction Equipment" },
                    { 36, "Business Supplies", "مستلزمات الأنشطة التجارية", "مستلزمات مطاعم", "Restaurant Supplies" },
                    { 37, "Business Supplies", "مستلزمات الأنشطة التجارية", "مستلزمات كافيهات", "Cafe Supplies" },
                    { 38, "Business Supplies", "مستلزمات الأنشطة التجارية", "مستلزمات سوبر ماركت", "Supermarket Supplies" },
                    { 39, "Business Supplies", "مستلزمات الأنشطة التجارية", "مستلزمات مكتبية", "Office Supplies" },
                    { 40, "Business Supplies", "مستلزمات الأنشطة التجارية", "مستلزمات شركات", "Company Supplies" },
                    { 41, "Other", "أخرى", "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "SupplierTypes",
                columns: new[] { "Id", "Group", "GroupAr", "Name" },
                values: new object[,]
                {
                    { 1, "Industrial Suppliers", "موردو الصناعة", "مواد خام" },
                    { 2, "Industrial Suppliers", "موردو الصناعة", "كيماويات" },
                    { 3, "Industrial Suppliers", "موردو الصناعة", "بلاستيك" },
                    { 4, "Industrial Suppliers", "موردو الصناعة", "ورق وكرتون" },
                    { 5, "Industrial Suppliers", "موردو الصناعة", "مواد تعبئة وتغليف" },
                    { 6, "Industrial Suppliers", "موردو الصناعة", "حديد وصلب" },
                    { 7, "Industrial Suppliers", "موردو الصناعة", "ألومنيوم" },
                    { 8, "Industrial Suppliers", "موردو الصناعة", "ماكينات ومعدات" },
                    { 9, "Industrial Suppliers", "موردو الصناعة", "قطع غيار صناعية" },
                    { 10, "Agriculture Suppliers", "موردو الزراعة", "بذور وشتلات" },
                    { 11, "Agriculture Suppliers", "موردو الزراعة", "أسمدة" },
                    { 12, "Agriculture Suppliers", "موردو الزراعة", "مبيدات" },
                    { 13, "Agriculture Suppliers", "موردو الزراعة", "مستلزمات ري" },
                    { 14, "Agriculture Suppliers", "موردو الزراعة", "معدات زراعية" },
                    { 15, "Agriculture Suppliers", "موردو الزراعة", "صوب زراعية" },
                    { 16, "Livestock Suppliers", "موردو الثروة الحيوانية", "أعلاف" },
                    { 17, "Livestock Suppliers", "موردو الثروة الحيوانية", "أدوية بيطرية" },
                    { 18, "Livestock Suppliers", "موردو الثروة الحيوانية", "مستلزمات دواجن" },
                    { 19, "Livestock Suppliers", "موردو الثروة الحيوانية", "مستلزمات مواشي" },
                    { 20, "Livestock Suppliers", "موردو الثروة الحيوانية", "مستلزمات استزراع سمكي" },
                    { 21, "Construction Suppliers", "موردو مواد البناء", "أسمنت ومواد بناء" },
                    { 22, "Construction Suppliers", "موردو مواد البناء", "طوب ورمل وزلط" },
                    { 23, "Construction Suppliers", "موردو مواد البناء", "حديد تسليح" },
                    { 24, "Construction Suppliers", "موردو مواد البناء", "سيراميك" },
                    { 25, "Construction Suppliers", "موردو مواد البناء", "رخام وجرانيت" },
                    { 26, "Construction Suppliers", "موردو مواد البناء", "زجاج" },
                    { 27, "Construction Suppliers", "موردو مواد البناء", "دهانات" },
                    { 28, "Construction Suppliers", "موردو مواد البناء", "أدوات صحية" },
                    { 29, "Construction Suppliers", "موردو مواد البناء", "كابلات وأسلاك" },
                    { 30, "Construction Suppliers", "موردو مواد البناء", "أخشاب" },
                    { 31, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "مواد غذائية" },
                    { 32, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "مشروبات" },
                    { 33, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "ألبان ومنتجاتها" },
                    { 34, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "مخابز وحلويات" },
                    { 35, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "منظفات" },
                    { 36, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "مستحضرات تجميل" },
                    { 37, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "أدوية" },
                    { 38, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "مستلزمات طبية" },
                    { 39, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "ملابس جاهزة" },
                    { 40, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "أحذية" },
                    { 41, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "جلود" },
                    { 42, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "غزل ونسيج" },
                    { 43, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "أثاث" },
                    { 44, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "أجهزة كهربائية" },
                    { 45, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "إلكترونيات" },
                    { 46, "Business & Retail Suppliers", "موردو التجارة والتجزئة", "مستلزمات مكتبية" },
                    { 47, "Other", "أخرى", "أخرى" }
                });

            migrationBuilder.InsertData(
                table: "TradeTypes",
                columns: new[] { "Id", "Group", "GroupAr", "Name" },
                values: new object[,]
                {
                    { 1, "Food", "المواد الغذائية", "مواد غذائية" },
                    { 2, "Food", "المواد الغذائية", "مشروبات" },
                    { 3, "Food", "المواد الغذائية", "ألبان ومنتجاتها" },
                    { 4, "Food", "المواد الغذائية", "مخابز وحلويات" },
                    { 5, "Food", "المواد الغذائية", "لحوم ودواجن" },
                    { 6, "Food", "المواد الغذائية", "أسماك" },
                    { 7, "Food", "المواد الغذائية", "حبوب وبقوليات" },
                    { 8, "Food", "المواد الغذائية", "توابل وأعشاب" },
                    { 9, "Agricultural Products", "المنتجات الزراعية", "خضروات" },
                    { 10, "Agricultural Products", "المنتجات الزراعية", "فواكه" },
                    { 11, "Agricultural Products", "المنتجات الزراعية", "موالح" },
                    { 12, "Agricultural Products", "المنتجات الزراعية", "تمور" },
                    { 13, "Agricultural Products", "المنتجات الزراعية", "زيتون" },
                    { 14, "Agricultural Products", "المنتجات الزراعية", "أعشاب" },
                    { 15, "Agricultural Products", "المنتجات الزراعية", "زهور" },
                    { 16, "Agricultural Products", "المنتجات الزراعية", "محاصيل حقلية" },
                    { 17, "Fashion", "الأزياء", "ملابس جاهزة" },
                    { 18, "Fashion", "الأزياء", "أحذية" },
                    { 19, "Fashion", "الأزياء", "جلود" },
                    { 20, "Fashion", "الأزياء", "غزل ونسيج" },
                    { 21, "Home Products", "المنتجات المنزلية", "أثاث" },
                    { 22, "Home Products", "المنتجات المنزلية", "أجهزة كهربائية" },
                    { 23, "Home Products", "المنتجات المنزلية", "إلكترونيات" },
                    { 24, "Home Products", "المنتجات المنزلية", "أدوات منزلية" },
                    { 25, "Home Products", "المنتجات المنزلية", "منظفات" },
                    { 26, "Home Products", "المنتجات المنزلية", "مستحضرات تجميل" },
                    { 27, "Building Materials", "مواد البناء", "أسمنت ومواد بناء" },
                    { 28, "Building Materials", "مواد البناء", "حديد وصلب" },
                    { 29, "Building Materials", "مواد البناء", "ألومنيوم" },
                    { 30, "Building Materials", "مواد البناء", "سيراميك" },
                    { 31, "Building Materials", "مواد البناء", "رخام وجرانيت" },
                    { 32, "Building Materials", "مواد البناء", "زجاج" },
                    { 33, "Building Materials", "مواد البناء", "دهانات" },
                    { 34, "Building Materials", "مواد البناء", "أدوات صحية" },
                    { 35, "Building Materials", "مواد البناء", "أخشاب" },
                    { 36, "Automotive", "السيارات", "قطع غيار" },
                    { 37, "Automotive", "السيارات", "إطارات" },
                    { 38, "Automotive", "السيارات", "زيوت وشحوم" },
                    { 39, "Automotive", "السيارات", "بطاريات" },
                    { 40, "Agriculture & Livestock", "الزراعة والثروة الحيوانية", "أعلاف" },
                    { 41, "Agriculture & Livestock", "الزراعة والثروة الحيوانية", "أسمدة" },
                    { 42, "Agriculture & Livestock", "الزراعة والثروة الحيوانية", "مبيدات" },
                    { 43, "Agriculture & Livestock", "الزراعة والثروة الحيوانية", "بذور وشتلات" },
                    { 44, "Agriculture & Livestock", "الزراعة والثروة الحيوانية", "مواشي ودواجن" },
                    { 45, "Other", "أخرى", "أخرى" }
                });

            migrationBuilder.InsertData(
                table: "WholesaleTradeTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "مواد غذائية", "Food" },
                    { 2, "زراعة", "Agriculture" },
                    { 3, "أزياء", "Fashion" },
                    { 4, "مستلزمات منزلية", "Home Supplies" },
                    { 5, "مواد بناء", "Building Materials" },
                    { 6, "سيارات", "Automotive" },
                    { 7, "زراعة وثروة حيوانية", "Agriculture & Livestock" },
                    { 8, "أخرى", "Other" }
                });

            migrationBuilder.InsertData(
                table: "WorkTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "دوام كامل", "Full time" },
                    { 2, "دوام جزئي", "Part time" },
                    { 3, "عمل حر", "Freelance" },
                    { 4, "تدريب", "Internship" }
                });

            migrationBuilder.InsertData(
                table: "WorkshopTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ورشة نجارة" },
                    { 2, "ورشة حدادة" },
                    { 3, "ورشة ألوميتال" },
                    { 4, "ورشة زجاج" },
                    { 5, "ورشة رخام وجرانيت" },
                    { 6, "ورشة موبيليا" },
                    { 7, "ورشة مطابخ" },
                    { 8, "ورشة تنجيد" },
                    { 9, "ورشة خراطة" },
                    { 10, "ورشة لحام" },
                    { 11, "ورشة سيارات" },
                    { 12, "ورشة سمكرة" },
                    { 13, "ورشة دهان سيارات" },
                    { 14, "ورشة كهرباء سيارات" },
                    { 15, "ورشة موتور" },
                    { 16, "ورشة دراجات نارية" },
                    { 17, "ورشة إصلاح أجهزة كهربائية" },
                    { 18, "ورشة تكييف وتبريد" },
                    { 19, "ورشة تصنيع معدني" },
                    { 20, "ورشة تصنيع بلاستيك" },
                    { 21, "ورشة ملابس" },
                    { 22, "ورشة أحذية" },
                    { 23, "ورشة جلود" },
                    { 24, "ورشة تطريز" },
                    { 25, "ورشة طباعة" },
                    { 26, "أخرى" }
                });

            migrationBuilder.InsertData(
                table: "Centers",
                columns: new[] { "Id", "GovernorateId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "الفيوم" },
                    { 2, 1, "سنورس" },
                    { 3, 1, "طامية" },
                    { 4, 1, "يوسف الصديق" },
                    { 5, 1, "اطسا" },
                    { 6, 1, "ابشواي" },
                    { 7, 1, "الفيوم الجديدة" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Name", "NameAr" },
                values: new object[,]
                {
                    { 1, 1, "Private", "ملاكي" },
                    { 2, 1, "Taxi", "أجرة" },
                    { 3, 1, "Motorcycles", "موتوسيكلات" },
                    { 4, 1, "Heavy Equipment", "لوادر ومعدات ثقيلة" },
                    { 5, 2, "Workshops", "الورش" },
                    { 6, 2, "Craftsmen", "الحرفيين" },
                    { 7, 3, "Lost", "ضايع مني" },
                    { 8, 3, "Found", "لقيت" },
                    { 9, 4, "Factories", "المصانع" },
                    { 10, 4, "Farms", "المزارع" },
                    { 11, 4, "Companies", "الشركات" },
                    { 12, 4, "Suppliers", "الموردون" },
                    { 13, 4, "Wholesale Traders", "تجار الجملة" },
                    { 14, 4, "Fruit & Vegetable Traders", "تجار خضر وفاكهة" },
                    { 15, 5, "Job Requests", "طلبات عمل" },
                    { 16, 5, "Job Opportunities", "فرص عمل" },
                    { 17, 6, "Livestock", "المواشي" },
                    { 18, 6, "Sheep & Goats", "الأغنام والماعز" },
                    { 19, 6, "Horses", "الخيول" },
                    { 20, 6, "Camels", "الإبل" },
                    { 21, 6, "Birds", "الطيور" },
                    { 22, 6, "Pets", "الحيوانات الأليفة" },
                    { 23, 6, "Fish", "الأسماك" },
                    { 24, 6, "Bees", "النحل" },
                    { 25, 6, "Other Animals", "حيوانات أخرى" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementFeatures_FeatureId",
                table: "AdvertisementFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_AdvertisementId",
                table: "AdvertisementImages",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CategoryId_SubCategoryId",
                table: "Advertisements",
                columns: new[] { "CategoryId", "SubCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CreatedAt",
                table: "Advertisements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_ExpireAt",
                table: "Advertisements",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_OwnerId",
                table: "Advertisements",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Status",
                table: "Advertisements",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_SubCategoryId",
                table: "Advertisements",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementViews_AdvertisementId",
                table: "AdvertisementViews",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PasswordResetTokenHash",
                table: "AspNetUsers",
                column: "PasswordResetTokenHash",
                filter: "[PasswordResetTokenHash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RefreshToken",
                table: "AspNetUsers",
                column: "RefreshToken",
                filter: "[RefreshToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySeasons_Name",
                table: "AvailabilitySeasons",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeeHealthStatuses_Name",
                table: "BeeHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeeImages_BeeId",
                table: "BeeImages",
                column: "BeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BeeProductions_Name",
                table: "BeeProductions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeePurposes_Name",
                table: "BeePurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bees_AnimalType",
                table: "Bees",
                column: "AnimalType");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_CreatedAt",
                table: "Bees",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_HealthStatus",
                table: "Bees",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_Price",
                table: "Bees",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_Purpose",
                table: "Bees",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_SellerName",
                table: "Bees",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Bees_UserId",
                table: "Bees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BeeTypes_Name",
                table: "BeeTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirdAges_Name",
                table: "BirdAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirdGenders_Name",
                table: "BirdGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirdHealthStatuses_Name",
                table: "BirdHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirdImages_BirdId",
                table: "BirdImages",
                column: "BirdId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdPurposes_Name",
                table: "BirdPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Birds_Age",
                table: "Birds",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_AnimalType",
                table: "Birds",
                column: "AnimalType");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_CreatedAt",
                table: "Birds",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_Gender",
                table: "Birds",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_HealthStatus",
                table: "Birds",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_Price",
                table: "Birds",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_Purpose",
                table: "Birds",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_SellerName",
                table: "Birds",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Birds_UserId",
                table: "Birds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdTypes_Name",
                table: "BirdTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirdVaccinations_Name",
                table: "BirdVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CamelAges_Name",
                table: "CamelAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CamelBreeds_Name",
                table: "CamelBreeds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CamelGenders_Name",
                table: "CamelGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CamelHealthStatuses_Name",
                table: "CamelHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CamelImages_CamelId",
                table: "CamelImages",
                column: "CamelId");

            migrationBuilder.CreateIndex(
                name: "IX_CamelPurposes_Name",
                table: "CamelPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Camels_Age",
                table: "Camels",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_Breed",
                table: "Camels",
                column: "Breed");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_CreatedAt",
                table: "Camels",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_Gender",
                table: "Camels",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_HealthStatus",
                table: "Camels",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_Price",
                table: "Camels",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_Purpose",
                table: "Camels",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_SellerName",
                table: "Camels",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Camels_UserId",
                table: "Camels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CamelVaccinations_Name",
                table: "CamelVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Centers_GovernorateId_Name",
                table: "Centers",
                columns: new[] { "GovernorateId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyField",
                table: "Companies",
                column: "CompanyField");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyName",
                table: "Companies",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CreatedAt",
                table: "Companies",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFields_Code",
                table: "CompanyFields",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyImages_CompanyId",
                table: "CompanyImages",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftsmanImages_CraftsmanId",
                table: "CraftsmanImages",
                column: "CraftsmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_Center",
                table: "Craftsmen",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_CreatedAt",
                table: "Craftsmen",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_ExperienceLevel",
                table: "Craftsmen",
                column: "ExperienceLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_Specialization",
                table: "Craftsmen",
                column: "Specialization");

            migrationBuilder.CreateIndex(
                name: "IX_Craftsmen_UserId",
                table: "Craftsmen",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_Name",
                table: "EducationLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceLevels_Name",
                table: "ExperienceLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factories_CreatedAt",
                table: "Factories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_FactoryName",
                table: "Factories",
                column: "FactoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_ProductionSpecialty",
                table: "Factories",
                column: "ProductionSpecialty");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_UserId",
                table: "Factories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryImages_FactoryId",
                table: "FactoryImages",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmImages_FarmId",
                table: "FarmImages",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmingMethods_Code",
                table: "FarmingMethods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_CreatedAt",
                table: "Farms",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_FarmName",
                table: "Farms",
                column: "FarmName");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_FarmType",
                table: "Farms",
                column: "FarmType");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_UserId",
                table: "Farms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmTypes_Name",
                table: "FarmTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_Name",
                table: "Features",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fish_Age",
                table: "Fish",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_AnimalType",
                table: "Fish",
                column: "AnimalType");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_CreatedAt",
                table: "Fish",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_HealthStatus",
                table: "Fish",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_Price",
                table: "Fish",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_Purpose",
                table: "Fish",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_SellerName",
                table: "Fish",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_UserId",
                table: "Fish",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FishAges_Name",
                table: "FishAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FishHealthStatuses_Name",
                table: "FishHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FishImages_FishId",
                table: "FishImages",
                column: "FishId");

            migrationBuilder.CreateIndex(
                name: "IX_FishPurposes_Name",
                table: "FishPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FishTypes_Name",
                table: "FishTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchantImages_FruitVegetableMerchantId",
                table: "FruitVegetableMerchantImages",
                column: "FruitVegetableMerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_CreatedAt",
                table: "FruitVegetableMerchants",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_MerchantName",
                table: "FruitVegetableMerchants",
                column: "MerchantName");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_ProductName",
                table: "FruitVegetableMerchants",
                column: "ProductName");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_SaleType",
                table: "FruitVegetableMerchants",
                column: "SaleType");

            migrationBuilder.CreateIndex(
                name: "IX_FruitVegetableMerchants_UserId",
                table: "FruitVegetableMerchants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Name",
                table: "Governorates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorseAges_Name",
                table: "HorseAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorseBreeds_Name",
                table: "HorseBreeds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorseGenders_Name",
                table: "HorseGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorseHealthStatuses_Name",
                table: "HorseHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorseImages_HorseId",
                table: "HorseImages",
                column: "HorseId");

            migrationBuilder.CreateIndex(
                name: "IX_HorsePurposes_Name",
                table: "HorsePurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Horses_Age",
                table: "Horses",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_Breed",
                table: "Horses",
                column: "Breed");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_CreatedAt",
                table: "Horses",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_Gender",
                table: "Horses",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_HealthStatus",
                table: "Horses",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_Price",
                table: "Horses",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_Purpose",
                table: "Horses",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_SellerName",
                table: "Horses",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Horses_UserId",
                table: "Horses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HorseTrainingLevels_Name",
                table: "HorseTrainingLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorseVaccinations_Name",
                table: "HorseVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobExperienceLevels_Name",
                table: "JobExperienceLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobFields_Group_Name",
                table: "JobFields",
                columns: new[] { "Group", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_Center",
                table: "JobOpportunities",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_CreatedAt",
                table: "JobOpportunities",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_JobField",
                table: "JobOpportunities",
                column: "JobField");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_JobTitle",
                table: "JobOpportunities",
                column: "JobTitle");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_RequiredExperience",
                table: "JobOpportunities",
                column: "RequiredExperience");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_UserId",
                table: "JobOpportunities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunities_WorkType",
                table: "JobOpportunities",
                column: "WorkType");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpportunityImages_JobOpportunityId",
                table: "JobOpportunityImages",
                column: "JobOpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_Center",
                table: "JobRequests",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_CreatedAt",
                table: "JobRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_Education",
                table: "JobRequests",
                column: "Education");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_Experience",
                table: "JobRequests",
                column: "Experience");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_JobField",
                table: "JobRequests",
                column: "JobField");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_UserId",
                table: "JobRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingTypes_Name",
                table: "ListingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_Age",
                table: "Livestock",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_Breed",
                table: "Livestock",
                column: "Breed");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_CreatedAt",
                table: "Livestock",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_Gender",
                table: "Livestock",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_HealthStatus",
                table: "Livestock",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_Price",
                table: "Livestock",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_Purpose",
                table: "Livestock",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_SellerName",
                table: "Livestock",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Livestock_UserId",
                table: "Livestock",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockAges_Name",
                table: "LivestockAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockBreeds_Name",
                table: "LivestockBreeds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockGenders_Name",
                table: "LivestockGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockHealthStatuses_Name",
                table: "LivestockHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockImages_LivestockId",
                table: "LivestockImages",
                column: "LivestockId");

            migrationBuilder.CreateIndex(
                name: "IX_LivestockProductions_Name",
                table: "LivestockProductions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockPurposes_Name",
                table: "LivestockPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LivestockVaccinations_Name",
                table: "LivestockVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundComments_PostId_CreatedAt",
                table: "LostFoundComments",
                columns: new[] { "PostId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundComments_UserId",
                table: "LostFoundComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundImages_PostId",
                table: "LostFoundImages",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundLikes_PostId_UserId",
                table: "LostFoundLikes",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundLikes_UserId",
                table: "LostFoundLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_CreatedAt",
                table: "LostFoundPosts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_FoundDate",
                table: "LostFoundPosts",
                column: "FoundDate");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_LostDate",
                table: "LostFoundPosts",
                column: "LostDate");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_PostType",
                table: "LostFoundPosts",
                column: "PostType");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_Status",
                table: "LostFoundPosts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LostFoundPosts_UserId",
                table: "LostFoundPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantSaleTypes_Name",
                table: "MerchantSaleTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_Type_ReferenceId",
                table: "Notifications",
                columns: new[] { "UserId", "Type", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalAges_Name",
                table: "OtherAnimalAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalGenders_Name",
                table: "OtherAnimalGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalHealthStatuses_Name",
                table: "OtherAnimalHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalImages_OtherAnimalId",
                table: "OtherAnimalImages",
                column: "OtherAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalPurposes_Name",
                table: "OtherAnimalPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_Age",
                table: "OtherAnimals",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_AnimalType",
                table: "OtherAnimals",
                column: "AnimalType");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_CreatedAt",
                table: "OtherAnimals",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_Gender",
                table: "OtherAnimals",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_HealthStatus",
                table: "OtherAnimals",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_Price",
                table: "OtherAnimals",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_Purpose",
                table: "OtherAnimals",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_SellerName",
                table: "OtherAnimals",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimals_UserId",
                table: "OtherAnimals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalTypes_Name",
                table: "OtherAnimalTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherAnimalVaccinations_Name",
                table: "OtherAnimalVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_IsActive_DisplayOrder",
                table: "PaymentMethods",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Name",
                table: "PaymentMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ApprovedBy",
                table: "Payments",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status_SubmittedAt",
                table: "Payments",
                columns: new[] { "Status", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TargetType_TargetId",
                table: "Payments",
                columns: new[] { "TargetType", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId_SubmittedAt",
                table: "Payments",
                columns: new[] { "UserId", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PetAges_Name",
                table: "PetAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetBreeds_Name",
                table: "PetBreeds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetGenders_Name",
                table: "PetGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetHealthStatuses_Name",
                table: "PetHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetImages_PetId",
                table: "PetImages",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPurposes_Name",
                table: "PetPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Age",
                table: "Pets",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Breed",
                table: "Pets",
                column: "Breed");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_CreatedAt",
                table: "Pets",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Gender",
                table: "Pets",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_HealthStatus",
                table: "Pets",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Price",
                table: "Pets",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Purpose",
                table: "Pets",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_SellerName",
                table: "Pets",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_UserId",
                table: "Pets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PetTrainingLevels_Name",
                table: "PetTrainingLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetVaccinations_Name",
                table: "PetVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionSpecialties_Name",
                table: "ProductionSpecialties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryTypes_Name",
                table: "SalaryTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleTypes_Code",
                table: "SaleTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatAges_Name",
                table: "SheepGoatAges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatBreeds_Name",
                table: "SheepGoatBreeds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatGenders_Name",
                table: "SheepGoatGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatHealthStatuses_Name",
                table: "SheepGoatHealthStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatImages_SheepGoatId",
                table: "SheepGoatImages",
                column: "SheepGoatId");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatPurposes_Name",
                table: "SheepGoatPurposes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_Age",
                table: "SheepGoats",
                column: "Age");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_Breed",
                table: "SheepGoats",
                column: "Breed");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_CreatedAt",
                table: "SheepGoats",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_Gender",
                table: "SheepGoats",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_HealthStatus",
                table: "SheepGoats",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_Price",
                table: "SheepGoats",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_Purpose",
                table: "SheepGoats",
                column: "Purpose");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_SellerName",
                table: "SheepGoats",
                column: "SellerName");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoats_UserId",
                table: "SheepGoats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SheepGoatVaccinations_Name",
                table: "SheepGoatVaccinations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierImages_SupplierId",
                table: "SupplierImages",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatedAt",
                table: "Suppliers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierName",
                table: "Suppliers",
                column: "SupplierName");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierType",
                table: "Suppliers",
                column: "SupplierType");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UserId",
                table: "Suppliers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierSpecializations_Group_Name",
                table: "SupplierSpecializations",
                columns: new[] { "Group", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTypes_Group_Name",
                table: "SupplierTypes",
                columns: new[] { "Group", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeTypes_Group_Name",
                table: "TradeTypes",
                columns: new[] { "Group", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraderImages_WholesaleTraderId",
                table: "WholesaleTraderImages",
                column: "WholesaleTraderId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_CreatedAt",
                table: "WholesaleTraders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_SaleType",
                table: "WholesaleTraders",
                column: "SaleType");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_TraderName",
                table: "WholesaleTraders",
                column: "TraderName");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_TradeType",
                table: "WholesaleTraders",
                column: "TradeType");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTraders_UserId",
                table: "WholesaleTraders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleTradeTypes_Name",
                table: "WholesaleTradeTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopImages_WorkshopId",
                table: "WorkshopImages",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_Center",
                table: "Workshops",
                column: "Center");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_CreatedAt",
                table: "Workshops",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_UserId",
                table: "Workshops",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_WorkshopType",
                table: "Workshops",
                column: "WorkshopType");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopTypes_Name",
                table: "WorkshopTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_Name",
                table: "WorkTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementFeatures");

            migrationBuilder.DropTable(
                name: "AdvertisementImages");

            migrationBuilder.DropTable(
                name: "AdvertisementViews");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AvailabilitySeasons");

            migrationBuilder.DropTable(
                name: "BeeHealthStatuses");

            migrationBuilder.DropTable(
                name: "BeeImages");

            migrationBuilder.DropTable(
                name: "BeeProductions");

            migrationBuilder.DropTable(
                name: "BeePurposes");

            migrationBuilder.DropTable(
                name: "BeeTypes");

            migrationBuilder.DropTable(
                name: "BirdAges");

            migrationBuilder.DropTable(
                name: "BirdGenders");

            migrationBuilder.DropTable(
                name: "BirdHealthStatuses");

            migrationBuilder.DropTable(
                name: "BirdImages");

            migrationBuilder.DropTable(
                name: "BirdPurposes");

            migrationBuilder.DropTable(
                name: "BirdTypes");

            migrationBuilder.DropTable(
                name: "BirdVaccinations");

            migrationBuilder.DropTable(
                name: "CamelAges");

            migrationBuilder.DropTable(
                name: "CamelBreeds");

            migrationBuilder.DropTable(
                name: "CamelGenders");

            migrationBuilder.DropTable(
                name: "CamelHealthStatuses");

            migrationBuilder.DropTable(
                name: "CamelImages");

            migrationBuilder.DropTable(
                name: "CamelPurposes");

            migrationBuilder.DropTable(
                name: "CamelVaccinations");

            migrationBuilder.DropTable(
                name: "Centers");

            migrationBuilder.DropTable(
                name: "CompanyFields");

            migrationBuilder.DropTable(
                name: "CompanyImages");

            migrationBuilder.DropTable(
                name: "CraftsmanImages");

            migrationBuilder.DropTable(
                name: "CraftsmanSpecializations");

            migrationBuilder.DropTable(
                name: "EducationLevels");

            migrationBuilder.DropTable(
                name: "ExperienceLevels");

            migrationBuilder.DropTable(
                name: "FactoryImages");

            migrationBuilder.DropTable(
                name: "FarmImages");

            migrationBuilder.DropTable(
                name: "FarmingMethods");

            migrationBuilder.DropTable(
                name: "FarmTypes");

            migrationBuilder.DropTable(
                name: "FishAges");

            migrationBuilder.DropTable(
                name: "FishHealthStatuses");

            migrationBuilder.DropTable(
                name: "FishImages");

            migrationBuilder.DropTable(
                name: "FishPurposes");

            migrationBuilder.DropTable(
                name: "FishTypes");

            migrationBuilder.DropTable(
                name: "FruitVegetableMerchantImages");

            migrationBuilder.DropTable(
                name: "HorseAges");

            migrationBuilder.DropTable(
                name: "HorseBreeds");

            migrationBuilder.DropTable(
                name: "HorseGenders");

            migrationBuilder.DropTable(
                name: "HorseHealthStatuses");

            migrationBuilder.DropTable(
                name: "HorseImages");

            migrationBuilder.DropTable(
                name: "HorsePurposes");

            migrationBuilder.DropTable(
                name: "HorseTrainingLevels");

            migrationBuilder.DropTable(
                name: "HorseVaccinations");

            migrationBuilder.DropTable(
                name: "JobExperienceLevels");

            migrationBuilder.DropTable(
                name: "JobFields");

            migrationBuilder.DropTable(
                name: "JobOpportunityImages");

            migrationBuilder.DropTable(
                name: "JobRequests");

            migrationBuilder.DropTable(
                name: "ListingTypes");

            migrationBuilder.DropTable(
                name: "LivestockAges");

            migrationBuilder.DropTable(
                name: "LivestockBreeds");

            migrationBuilder.DropTable(
                name: "LivestockGenders");

            migrationBuilder.DropTable(
                name: "LivestockHealthStatuses");

            migrationBuilder.DropTable(
                name: "LivestockImages");

            migrationBuilder.DropTable(
                name: "LivestockProductions");

            migrationBuilder.DropTable(
                name: "LivestockPurposes");

            migrationBuilder.DropTable(
                name: "LivestockVaccinations");

            migrationBuilder.DropTable(
                name: "LostFoundComments");

            migrationBuilder.DropTable(
                name: "LostFoundImages");

            migrationBuilder.DropTable(
                name: "LostFoundLikes");

            migrationBuilder.DropTable(
                name: "MerchantSaleTypes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OtherAnimalAges");

            migrationBuilder.DropTable(
                name: "OtherAnimalGenders");

            migrationBuilder.DropTable(
                name: "OtherAnimalHealthStatuses");

            migrationBuilder.DropTable(
                name: "OtherAnimalImages");

            migrationBuilder.DropTable(
                name: "OtherAnimalPurposes");

            migrationBuilder.DropTable(
                name: "OtherAnimalTypes");

            migrationBuilder.DropTable(
                name: "OtherAnimalVaccinations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PetAges");

            migrationBuilder.DropTable(
                name: "PetBreeds");

            migrationBuilder.DropTable(
                name: "PetGenders");

            migrationBuilder.DropTable(
                name: "PetHealthStatuses");

            migrationBuilder.DropTable(
                name: "PetImages");

            migrationBuilder.DropTable(
                name: "PetPurposes");

            migrationBuilder.DropTable(
                name: "PetTrainingLevels");

            migrationBuilder.DropTable(
                name: "PetVaccinations");

            migrationBuilder.DropTable(
                name: "ProductionSpecialties");

            migrationBuilder.DropTable(
                name: "SalaryTypes");

            migrationBuilder.DropTable(
                name: "SaleTypes");

            migrationBuilder.DropTable(
                name: "SheepGoatAges");

            migrationBuilder.DropTable(
                name: "SheepGoatBreeds");

            migrationBuilder.DropTable(
                name: "SheepGoatGenders");

            migrationBuilder.DropTable(
                name: "SheepGoatHealthStatuses");

            migrationBuilder.DropTable(
                name: "SheepGoatImages");

            migrationBuilder.DropTable(
                name: "SheepGoatPurposes");

            migrationBuilder.DropTable(
                name: "SheepGoatVaccinations");

            migrationBuilder.DropTable(
                name: "SupplierImages");

            migrationBuilder.DropTable(
                name: "SupplierSpecializations");

            migrationBuilder.DropTable(
                name: "SupplierTypes");

            migrationBuilder.DropTable(
                name: "TradeTypes");

            migrationBuilder.DropTable(
                name: "WholesaleTraderImages");

            migrationBuilder.DropTable(
                name: "WholesaleTradeTypes");

            migrationBuilder.DropTable(
                name: "WorkshopImages");

            migrationBuilder.DropTable(
                name: "WorkshopTypes");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Bees");

            migrationBuilder.DropTable(
                name: "Birds");

            migrationBuilder.DropTable(
                name: "Camels");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Craftsmen");

            migrationBuilder.DropTable(
                name: "Factories");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Fish");

            migrationBuilder.DropTable(
                name: "FruitVegetableMerchants");

            migrationBuilder.DropTable(
                name: "Horses");

            migrationBuilder.DropTable(
                name: "JobOpportunities");

            migrationBuilder.DropTable(
                name: "Livestock");

            migrationBuilder.DropTable(
                name: "LostFoundPosts");

            migrationBuilder.DropTable(
                name: "OtherAnimals");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "SheepGoats");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "WholesaleTraders");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
