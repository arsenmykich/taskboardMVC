# Документація збереженої функції sp_GetUserAnnouncements

## Опис
Збережена функція `sp_GetUserAnnouncements` реалізована для оптимізації отримання оголошень конкретного користувача з бази даних PostgreSQL. Функція замінює стандартний LINQ запит Entity Framework на більш ефективний SQL запит на рівні бази даних.

## Місце в архітектурі
- **Контроллер**: `AnnouncementsController.MyAnnouncementsStoredProc()`
- **Сервіс**: `AnnouncementService.GetAnnouncementsByUserStoredProcAsync()`
- **Репозиторій**: `AnnouncementRepository.GetAnnouncementsByUserStoredProcAsync()`
- **Збережена функція**: `sp_GetUserAnnouncements`

## PostgreSQL код функції
```sql
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
        a."Id",
        a."Title",
        a."Description",
        a."CategoryId",
        a."SubCategoryId",
        a."UserId",
        a."Status",
        a."CreatedDate",
        a."UpdatedDate",
        c."Name" AS CategoryName,
        c."Description" AS CategoryDescription,
        c."CreatedAt" AS CategoryCreatedAt,
        sc."Name" AS SubCategoryName,
        sc."Description" AS SubCategoryDescription,
        sc."CategoryId" AS SubCategoryCategoryId,
        sc."CreatedAt" AS SubCategoryCreatedAt
    FROM "Announcements" a
    INNER JOIN "Categories" c ON a."CategoryId" = c."Id"
    INNER JOIN "SubCategories" sc ON a."SubCategoryId" = sc."Id"
    WHERE a."UserId" = p_userid
    ORDER BY a."CreatedDate" DESC;
END;
$$ LANGUAGE plpgsql;
```

## Параметри
- `p_userid` (VARCHAR(450)) - Ідентифікатор користувача для фільтрації оголошень

## Повертає
Результуючий набір містить:
- Всі поля оголошень (Announcements)
- Інформацію про категорії (Categories)
- Інформацію про підкатегорії (SubCategories)

## Використання

### В контроллері
```csharp
// GET: /Announcements/MyAnnouncementsStoredProc
public async Task<IActionResult> MyAnnouncementsStoredProc()
{
    var userId = await GetCurrentUserIdAsync();
    var announcements = await _announcementService.GetAnnouncementsByUserStoredProcAsync(userId);
    return View("MyAnnouncements", announcements);
}
```

### В сервісі
```csharp
public async Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId)
{
    return await _unitOfWork.Announcements.GetAnnouncementsByUserStoredProcAsync(userId);
}
```

### В репозиторії
```csharp
public async Task<IEnumerable<Announcement>> GetAnnouncementsByUserStoredProcAsync(string userId)
{
    return await _context.Set<Announcement>()
        .FromSqlRaw("SELECT * FROM sp_GetUserAnnouncements({0})", userId)
        .Include(a => a.Category)
        .Include(a => a.SubCategory)
        .AsNoTracking()
        .ToListAsync();
}
```

## Переваги збереженої функції
1. **Продуктивність**: Швидше виконання через компіляцію на рівні БД
2. **Менше мережевого трафіку**: Один виклик замість множинних запитів
3. **Кешування планів виконання**: PostgreSQL кешує план виконання
4. **Централізована логіка**: Бізнес-логіка на рівні бази даних

## Міграція
Збережена функція додається до бази даних через міграцію Entity Framework:
- **Файл**: `20250702153037_AddGetUserAnnouncementsProcedure.cs`
- **Команда створення**: `dotnet ef migrations add AddGetUserAnnouncementsProcedure`
- **Команда застосування**: `dotnet ef database update`

## Статус міграції
✅ **ЗАСТОСОВАНО** - Міграція успішно застосована до бази даних PostgreSQL

## Тестування
Для перевірки роботи збереженої функції відвідайте:
```
GET /Announcements/MyAnnouncementsStoredProc
```

Цей endpoint використовує збережену функцію PostgreSQL замість стандартного LINQ запиту. 