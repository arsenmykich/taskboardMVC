# Документація збережених процедур SQL Server

## Опис
Система оголошень була мігрована з PostgreSQL на **Microsoft SQL Server** з повним набором збережених процедур для всіх CRUD операцій відповідно до вимог тестового завдання.

## Список збережених процедур

### 1. sp_CreateAnnouncement
**Призначення:** Створення нового оголошення

**Параметри:**
- `@Title` NVARCHAR(200) - Заголовок оголошення
- `@Description` NVARCHAR(2000) - Опис оголошення
- `@CategoryId` INT - ID категорії
- `@SubCategoryId` INT - ID підкатегорії
- `@UserId` NVARCHAR(450) - ID користувача
- `@Status` BIT - Статус оголошення (1 - активне, 0 - неактивне)
- `@CreatedDate` DATETIME2 - Дата створення
- `@UpdatedDate` DATETIME2 - Дата оновлення

**Повертає:** ID створеного оголошення (SCOPE_IDENTITY)

```sql
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
```

### 2. sp_UpdateAnnouncement
**Призначення:** Оновлення існуючого оголошення

**Параметри:**
- `@Id` INT - ID оголошення для оновлення
- `@Title` NVARCHAR(200) - Новий заголовок
- `@Description` NVARCHAR(2000) - Новий опис
- `@CategoryId` INT - Новий ID категорії
- `@SubCategoryId` INT - Новий ID підкатегорії
- `@Status` BIT - Новий статус
- `@UpdatedDate` DATETIME2 - Дата оновлення

```sql
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
```

### 3. sp_DeleteAnnouncement
**Призначення:** Видалення оголошення

**Параметри:**
- `@Id` INT - ID оголошення для видалення

```sql
CREATE PROCEDURE sp_DeleteAnnouncement
    @Id INT
AS
BEGIN
    DELETE FROM Announcements WHERE Id = @Id
END
```

### 4. sp_GetAnnouncementById
**Призначення:** Отримання оголошення за ID

**Параметри:**
- `@Id` INT - ID оголошення

**Повертає:** Дані оголошення

```sql
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
```

### 5. sp_GetAllAnnouncements
**Призначення:** Отримання всіх оголошень

**Повертає:** Список всіх оголошень упорядкований за датою створення

```sql
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
```

### 6. sp_GetUserAnnouncements
**Призначення:** Отримання оголошень конкретного користувача

**Параметри:**
- `@UserId` NVARCHAR(450) - ID користувача

**Повертає:** Список оголошень користувача упорядкований за датою створення

```sql
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
```

## Використання в коді

### Репозиторій (AnnouncementRepository.cs)

```csharp
public async Task<Announcement> CreateAnnouncementStoredProcAsync(Announcement announcement)
{
    var parameters = new[]
    {
        new SqlParameter("@Title", announcement.Title),
        new SqlParameter("@Description", announcement.Description),
        new SqlParameter("@CategoryId", announcement.CategoryId),
        new SqlParameter("@SubCategoryId", announcement.SubCategoryId),
        new SqlParameter("@UserId", announcement.UserId),
        new SqlParameter("@Status", announcement.Status),
        new SqlParameter("@CreatedDate", announcement.CreatedDate),
        new SqlParameter("@UpdatedDate", announcement.UpdatedDate)
    };

    var result = await _context.Database.ExecuteSqlRawAsync(
        "EXEC sp_CreateAnnouncement @Title, @Description, @CategoryId, @SubCategoryId, @UserId, @Status, @CreatedDate, @UpdatedDate",
        parameters);

    return announcement;
}
```

### Сервіс (AnnouncementService.cs)

```csharp
public async Task<Announcement> CreateAnnouncementStoredProcAsync(Announcement announcement)
{
    // Business logic validation
    ValidateAnnouncement(announcement);
    
    // Set timestamps
    announcement.CreatedDate = DateTime.UtcNow;
    announcement.UpdatedDate = DateTime.UtcNow;
    announcement.Status = true;

    return await _unitOfWork.Announcements.CreateAnnouncementStoredProcAsync(announcement);
}
```

### Контролер (AnnouncementsController.cs)

```csharp
[HttpPost]
public async Task<IActionResult> CreateWithStoredProc(AnnouncementViewModel model)
{
    if (ModelState.IsValid)
    {
        var announcement = new Announcement
        {
            Title = model.Title,
            Description = model.Description,
            CategoryId = model.CategoryId,
            SubCategoryId = model.SubCategoryId,
            UserId = await GetCurrentUserIdAsync(),
            Status = model.Status
        };

        await _announcementService.CreateAnnouncementStoredProcAsync(announcement);
        return RedirectToAction(nameof(MyAnnouncements));
    }
    return View(model);
}
```

## Архітектура системи

### Міграція
Збережені процедури створюються автоматично через міграцію Entity Framework:
- **Файл міграції:** `20250703142341_InitialCreateWithStoredProcedures.cs`
- **Команда створення:** `dotnet ef migrations add InitialCreateWithStoredProcedures`
- **Команда застосування:** `dotnet ef database update`

### Підключення до бази даних
```json
{
  "ConnectionStrings": {
    "TaskboardDB": "Server=(localdb)\\mssqllocaldb;Database=AnnouncementBoard;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Налаштування Entity Framework
```csharp
builder.Services.AddDbContext<AnnouncementBoardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskboardDB")));
```

## Переваги збережених процедур

1. **Продуктивність:** Планування виконання на рівні СУБД
2. **Безпека:** Захист від SQL Injection
3. **Централізована логіка:** Бізнес-логіка на рівні бази даних
4. **Кешування:** SQL Server кешує плани виконання
5. **Аудит:** Простше відстежувати виклики процедур

## Порівняння з Entity Framework

| Аспект | Збережені процедури | Entity Framework |
|--------|-------------------|------------------|
| Продуктивність | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| Безпека | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| Підтримка | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| Портабельність | ⭐⭐ | ⭐⭐⭐⭐⭐ |
| Компактність коду | ⭐⭐⭐ | ⭐⭐⭐⭐ |

## Статус міграції
✅ **ЗАСТОСОВАНО** - Всі збережені процедури успішно створені в базі даних SQL Server

## Тестування
Для перевірки роботи збережених процедур:
1. Запустіть додаток: `dotnet run --project AnnouncementBoard.Web`
2. Використовуйте методи з суфіксом `StoredProc` у сервісах
3. Перевірте логи виконання SQL команд

## Розгортання на Azure
Для розгортання на Azure App Service з SQL Server:
1. Створіть Azure SQL Database
2. Оновіть connection string в appsettings.json
3. Застосуйте міграції: `dotnet ef database update`
4. Розгорніть додаток на Azure App Service 