using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Presitance.Migrations
{
    /// <inheritdoc />
    public partial class AddCharityModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AskConsults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    OtherCategory = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AskerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Question = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    IsResponsibilityAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ResponsibilityAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstPublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepublishCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModeratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModeratedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RejectionReason = table.Column<int>(type: "int", nullable: true),
                    ModerationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskConsults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AskConsults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BloodRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Center = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HospitalName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    IsResponsibilityAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ResponsibilityAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstPublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepublishCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModeratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModeratedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RejectionReason = table.Column<int>(type: "int", nullable: true),
                    ModerationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rescues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RescuerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    IsResponsibilityAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ResponsibilityAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstPublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepublishCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModeratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModeratedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RejectionReason = table.Column<int>(type: "int", nullable: true),
                    ModerationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rescues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rescues_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AskConsultComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AskConsultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskConsultComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AskConsultComments_AskConsults_AskConsultId",
                        column: x => x.AskConsultId,
                        principalTable: "AskConsults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AskConsultComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AskConsultImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AskConsultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskConsultImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AskConsultImages_AskConsults_AskConsultId",
                        column: x => x.AskConsultId,
                        principalTable: "AskConsults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AskConsultLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AskConsultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskConsultLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AskConsultLikes_AskConsults_AskConsultId",
                        column: x => x.AskConsultId,
                        principalTable: "AskConsults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AskConsultLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BloodRequestImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodRequestImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodRequestImages_BloodRequests_BloodRequestId",
                        column: x => x.BloodRequestId,
                        principalTable: "BloodRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RescueImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RescueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RescueImages_Rescues_RescueId",
                        column: x => x.RescueId,
                        principalTable: "Rescues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Icon", "IsActive", "Name", "NameAr" },
                values: new object[] { 12, null, true, "Charity", "أعمال الخير" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Icon", "IsActive", "Name", "NameAr" },
                values: new object[,]
                {
                    { 50, 12, null, true, "Rescues", "الاستغاثة" },
                    { 51, 12, null, true, "Blood Requests", "فصائل الدم" },
                    { 52, 12, null, true, "Ask & Consult", "اسأل واستشير" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsultComments_AskConsultId_CreatedAt",
                table: "AskConsultComments",
                columns: new[] { "AskConsultId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsultComments_UserId",
                table: "AskConsultComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AskConsultImages_AskConsultId_SortOrder",
                table: "AskConsultImages",
                columns: new[] { "AskConsultId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsultLikes_AskConsult_User",
                table: "AskConsultLikes",
                columns: new[] { "AskConsultId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AskConsultLikes_UserId",
                table: "AskConsultLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_Category_CreatedAt",
                table: "AskConsults",
                columns: new[] { "Category", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ExpireAt",
                table: "AskConsults",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_IsDeleted",
                table: "AskConsults",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus",
                table: "AskConsults",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_ModerationStatus_CreatedAt",
                table: "AskConsults",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AskConsults_UserId_CreatedAt",
                table: "AskConsults",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequestImages_BloodRequestId_SortOrder",
                table: "BloodRequestImages",
                columns: new[] { "BloodRequestId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_BloodGroup_Center_CreatedAt",
                table: "BloodRequests",
                columns: new[] { "BloodGroup", "Center", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ExpireAt",
                table: "BloodRequests",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_IsDeleted",
                table: "BloodRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus",
                table: "BloodRequests",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_ModerationStatus_CreatedAt",
                table: "BloodRequests",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_UserId_CreatedAt",
                table: "BloodRequests",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RescueImages_RescueId_SortOrder",
                table: "RescueImages",
                columns: new[] { "RescueId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ExpireAt",
                table: "Rescues",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_IsDeleted",
                table: "Rescues",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus",
                table: "Rescues",
                column: "ModerationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_ModerationStatus_CreatedAt",
                table: "Rescues",
                columns: new[] { "ModerationStatus", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Rescues_UserId_CreatedAt",
                table: "Rescues",
                columns: new[] { "UserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AskConsultComments");

            migrationBuilder.DropTable(
                name: "AskConsultImages");

            migrationBuilder.DropTable(
                name: "AskConsultLikes");

            migrationBuilder.DropTable(
                name: "BloodRequestImages");

            migrationBuilder.DropTable(
                name: "RescueImages");

            migrationBuilder.DropTable(
                name: "AskConsults");

            migrationBuilder.DropTable(
                name: "BloodRequests");

            migrationBuilder.DropTable(
                name: "Rescues");

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
