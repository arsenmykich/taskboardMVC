using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnnouncementBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUserAnnouncementsProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                CREATE OR REPLACE FUNCTION sp_GetUserAnnouncements(p_userid VARCHAR(450))
                RETURNS TABLE(
                    Id INTEGER,
                    Title VARCHAR(200),
                    Description VARCHAR(2000),
                    CategoryId INTEGER,
                    SubCategoryId INTEGER,
                    UserId VARCHAR(450),
                    Status BOOLEAN,
                    CreatedDate TIMESTAMP,
                    UpdatedDate TIMESTAMP,
                    CategoryName VARCHAR(100),
                    CategoryDescription VARCHAR(500),
                    CategoryCreatedAt TIMESTAMP,
                    SubCategoryName VARCHAR(100),
                    SubCategoryDescription VARCHAR(500),
                    SubCategoryCategoryId INTEGER,
                    SubCategoryCreatedAt TIMESTAMP
                ) AS $$
                BEGIN
                    RETURN QUERY
                    SELECT 
                        a.""Id"",
                        a.""Title"",
                        a.""Description"",
                        a.""CategoryId"",
                        a.""SubCategoryId"",
                        a.""UserId"",
                        a.""Status"",
                        a.""CreatedDate"",
                        a.""UpdatedDate"",
                        c.""Name"" AS CategoryName,
                        c.""Description"" AS CategoryDescription,
                        c.""CreatedAt"" AS CategoryCreatedAt,
                        sc.""Name"" AS SubCategoryName,
                        sc.""Description"" AS SubCategoryDescription,
                        sc.""CategoryId"" AS SubCategoryCategoryId,
                        sc.""CreatedAt"" AS SubCategoryCreatedAt
                    FROM ""Announcements"" a
                    INNER JOIN ""Categories"" c ON a.""CategoryId"" = c.""Id""
                    INNER JOIN ""SubCategories"" sc ON a.""SubCategoryId"" = sc.""Id""
                    WHERE a.""UserId"" = p_userid
                    ORDER BY a.""CreatedDate"" DESC;
                END;
                $$ LANGUAGE plpgsql;";

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS sp_GetUserAnnouncements(VARCHAR(450));");
        }
    }
}
