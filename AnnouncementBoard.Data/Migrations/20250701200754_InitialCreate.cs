using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnnouncementBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcements_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(1888), "Техніка для дому", "Побутова техніка" },
                    { 2, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(2029), "Комп'ютери та комплектуючі", "Комп'ютерна техніка" },
                    { 3, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(2030), "Мобільні телефони", "Смартфони" },
                    { 4, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(2032), "Різне", "Інше" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6334), "", "Холодильники" },
                    { 2, 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6469), "", "Пральні машини" },
                    { 3, 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6470), "", "Бойлери" },
                    { 4, 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6471), "", "Печі" },
                    { 5, 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6473), "", "Витяжки" },
                    { 6, 1, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6474), "", "Мікрохвильові печі" },
                    { 7, 2, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6475), "", "ПК" },
                    { 8, 2, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6476), "", "Ноутбуки" },
                    { 9, 2, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6477), "", "Монітори" },
                    { 10, 2, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6478), "", "Принтери" },
                    { 11, 2, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6479), "", "Сканери" },
                    { 12, 3, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6480), "", "Android смартфони" },
                    { 13, 3, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6481), "", "iOS/Apple смартфони" },
                    { 14, 4, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6482), "", "Одяг" },
                    { 15, 4, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6483), "", "Взуття" },
                    { 16, 4, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6552), "", "Аксесуари" },
                    { 17, 4, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6553), "", "Спортивне обладнання" },
                    { 18, 4, new DateTime(2025, 7, 1, 20, 7, 53, 753, DateTimeKind.Utc).AddTicks(6554), "", "Іграшки" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CategoryId",
                table: "Announcements",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_SubCategoryId",
                table: "Announcements",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_UserId",
                table: "Announcements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
