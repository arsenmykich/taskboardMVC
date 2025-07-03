using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnnouncementBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriesAndSubCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            // Seed Categories with Unicode support
            migrationBuilder.Sql($@"
                INSERT INTO Categories (Name, Description, CreatedAt) VALUES
                (N'Побутова техніка', N'Техніка для дому', '{seedDate}'),
                (N'Комп''ютерна техніка', N'Комп''ютери та комплектуючі', '{seedDate}'),
                (N'Смартфони', N'Мобільні телефони', '{seedDate}'),
                (N'Інше', N'Різне', '{seedDate}')
            ");

            // Seed SubCategories for Побутова техніка (CategoryId = 1)
            migrationBuilder.Sql($@"
                INSERT INTO SubCategories (Name, Description, CategoryId, CreatedAt) VALUES
                (N'Холодильники', N'', 1, '{seedDate}'),
                (N'Пральні машини', N'', 1, '{seedDate}'),
                (N'Бойлери', N'', 1, '{seedDate}'),
                (N'Печі', N'', 1, '{seedDate}'),
                (N'Витяжки', N'', 1, '{seedDate}'),
                (N'Мікрохвильові печі', N'', 1, '{seedDate}')
            ");

            // Seed SubCategories for Комп'ютерна техніка (CategoryId = 2)
            migrationBuilder.Sql($@"
                INSERT INTO SubCategories (Name, Description, CategoryId, CreatedAt) VALUES
                (N'ПК', N'', 2, '{seedDate}'),
                (N'Ноутбуки', N'', 2, '{seedDate}'),
                (N'Монітори', N'', 2, '{seedDate}'),
                (N'Принтери', N'', 2, '{seedDate}'),
                (N'Сканери', N'', 2, '{seedDate}')
            ");

            // Seed SubCategories for Смартфони (CategoryId = 3)
            migrationBuilder.Sql($@"
                INSERT INTO SubCategories (Name, Description, CategoryId, CreatedAt) VALUES
                (N'Android смартфони', N'', 3, '{seedDate}'),
                (N'iOS/Apple смартфони', N'', 3, '{seedDate}')
            ");

            // Seed SubCategories for Інше (CategoryId = 4)
            migrationBuilder.Sql($@"
                INSERT INTO SubCategories (Name, Description, CategoryId, CreatedAt) VALUES
                (N'Одяг', N'', 4, '{seedDate}'),
                (N'Взуття', N'', 4, '{seedDate}'),
                (N'Аксесуари', N'', 4, '{seedDate}'),
                (N'Спортивне обладнання', N'', 4, '{seedDate}'),
                (N'Іграшки', N'', 4, '{seedDate}')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seeded data
            migrationBuilder.Sql("DELETE FROM SubCategories");
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}
