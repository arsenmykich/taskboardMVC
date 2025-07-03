using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnnouncementBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
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

            // Create stored procedures
            CreateStoredProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop stored procedures
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CreateAnnouncement");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_UpdateAnnouncement");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_DeleteAnnouncement");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetAnnouncementById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetAllAnnouncements");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetUserAnnouncements");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }

        private void CreateStoredProcedures(MigrationBuilder migrationBuilder)
        {
            // sp_CreateAnnouncement
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_CreateAnnouncement
                    @Title NVARCHAR(200),
                    @Description NVARCHAR(2000),
                    @CategoryId INT,
                    @SubCategoryId INT,
                    @UserId NVARCHAR(450),
                    @Status BIT,
                    @CreatedDate DATETIME2,
                    @UpdatedDate DATETIME2
                AS
                BEGIN
                    INSERT INTO Announcements (Title, Description, CategoryId, SubCategoryId, UserId, Status, CreatedDate, UpdatedDate)
                    VALUES (@Title, @Description, @CategoryId, @SubCategoryId, @UserId, @Status, @CreatedDate, @UpdatedDate)
                    
                    SELECT SCOPE_IDENTITY() AS Id
                END
            ");

            // sp_UpdateAnnouncement
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_UpdateAnnouncement
                    @Id INT,
                    @Title NVARCHAR(200),
                    @Description NVARCHAR(2000),
                    @CategoryId INT,
                    @SubCategoryId INT,
                    @Status BIT,
                    @UpdatedDate DATETIME2
                AS
                BEGIN
                    UPDATE Announcements 
                    SET Title = @Title,
                        Description = @Description,
                        CategoryId = @CategoryId,
                        SubCategoryId = @SubCategoryId,
                        Status = @Status,
                        UpdatedDate = @UpdatedDate
                    WHERE Id = @Id
                END
            ");

            // sp_DeleteAnnouncement
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DeleteAnnouncement
                    @Id INT
                AS
                BEGIN
                    DELETE FROM Announcements WHERE Id = @Id
                END
            ");

            // sp_GetAnnouncementById
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetAnnouncementById
                    @Id INT
                AS
                BEGIN
                    SELECT 
                        a.Id,
                        a.Title,
                        a.Description,
                        a.Status,
                        a.CreatedDate,
                        a.UpdatedDate,
                        a.UserId,
                        a.CategoryId,
                        a.SubCategoryId
                    FROM Announcements a
                    WHERE a.Id = @Id
                END
            ");

            // sp_GetAllAnnouncements
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetAllAnnouncements
                AS
                BEGIN
                    SELECT 
                        a.Id,
                        a.Title,
                        a.Description,
                        a.Status,
                        a.CreatedDate,
                        a.UpdatedDate,
                        a.UserId,
                        a.CategoryId,
                        a.SubCategoryId
                    FROM Announcements a
                    ORDER BY a.CreatedDate DESC
                END
            ");

            // sp_GetUserAnnouncements
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetUserAnnouncements
                    @UserId NVARCHAR(450)
                AS
                BEGIN
                    SELECT 
                        a.Id,
                        a.Title,
                        a.Description,
                        a.Status,
                        a.CreatedDate,
                        a.UpdatedDate,
                        a.UserId,
                        a.CategoryId,
                        a.SubCategoryId
                    FROM Announcements a
                    WHERE a.UserId = @UserId
                    ORDER BY a.CreatedDate DESC
                END
            ");
        }
    }
}
