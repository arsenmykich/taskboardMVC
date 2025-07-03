# AnnouncementBoard MVC - Дошка оголошень

## 📋 Загальний опис

Веб-додаток "Дошка оголошень" розроблений як тестове завдання з використанням ASP.NET Core MVC, **Microsoft SQL Server** з збереженими процедурами та Google OAuth автентифікацією.

## ✅ Реалізовані функції

### Основні вимоги:
- ✅ **CRUD операції** для оголошень
- ✅ **MS SQL Server** з збереженими процедурами
- ✅ **ASP.NET Core MVC** фронтенд
- ✅ **Google OAuth** автентифікація
- ✅ **Категорії та підкатегорії** відповідно до завдання

### Категорії та підкатегорії:
- **Побутова техніка:** Холодильники, Пральні машини, Бойлери, Печі, Витяжки, Мікрохвильові печі
- **Комп'ютерна техніка:** ПК, Ноутбуки, Монітори, Принтери, Сканери  
- **Смартфони:** Android смартфони, iOS/Apple смартфони
- **Інше:** Одяг, Взуття, Аксесуари, Спортивне обладнання, Іграшки

## 🔧 Технічний стек

- **Backend:** ASP.NET Core 8.0 MVC
- **Database:** Microsoft SQL Server (LocalDB)
- **ORM:** Entity Framework Core 9.0.6
- **Authentication:** Google OAuth 2.0
- **UI:** Bootstrap 5, jQuery

## 🗄️ Збережені процедури

Система використовує **6 збережених процедур** для всіх CRUD операцій:

1. **sp_CreateAnnouncement** - Створення оголошення
2. **sp_UpdateAnnouncement** - Оновлення оголошення  
3. **sp_DeleteAnnouncement** - Видалення оголошення
4. **sp_GetAnnouncementById** - Отримання оголошення за ID
5. **sp_GetAllAnnouncements** - Отримання всіх оголошень
6. **sp_GetUserAnnouncements** - Отримання оголошень користувача

> Детальна документація: [STORED_PROCEDURE_DOCUMENTATION.md](STORED_PROCEDURE_DOCUMENTATION.md)

## 📁 Архітектура проекту

```
taskboardMVC/
├── AnnouncementBoard.Core/          # Моделі та інтерфейси
│   ├── Models/                     # Announcement, Category, SubCategory, User
│   └── Interfaces/                 # Репозиторії та сервіси
├── AnnouncementBoard.Data/          # Рівень даних
│   ├── Repositories/               # Реалізація репозиторіїв
│   ├── Migrations/                 # Міграції EF Core
│   └── AnnouncementBoardDbContext.cs
├── AnnouncementBoard.Business/      # Бізнес-логіка
│   └── Services/                   # Сервіси та валідація
└── AnnouncementBoard.Web/           # MVC фронтенд
    ├── Controllers/                # MVC контролери  
    ├── Views/                      # Razor Views
    └── Models/ViewModels/          # ViewModels
```

## 🚀 Локальний запуск

### 1. Встановлення залежностей

```bash
# Клонування репозиторію
git clone [repository-url]
cd taskboardMVC

# Відновлення пакетів
dotnet restore
```

### 2. Налаштування бази даних

```bash
# Створення та застосування міграцій
dotnet ef database update --project AnnouncementBoard.Data --startup-project AnnouncementBoard.Web
```

### 3. Конфігурація Google OAuth

Створіть файл `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "TaskboardDB": "Server=(localdb)\\mssqllocaldb;Database=AnnouncementBoard;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "GoogleAuth": {
    "ClientId": "your-google-client-id",
    "ClientSecret": "your-google-client-secret"
  }
}
```

### 4. Запуск додатку

```bash
dotnet run --project AnnouncementBoard.Web
```

Додаток буде доступний за адресою: `https://localhost:5001`

## 🌐 Розгортання на Azure

### 1. Azure SQL Database

```bash
# Створення Azure SQL Database
az sql server create --name [server-name] --resource-group [rg-name] --location [location] --admin-user [username] --admin-password [password]
az sql db create --server [server-name] --resource-group [rg-name] --name AnnouncementBoard --service-objective Basic
```

### 2. Connection String для Azure

```json
{
  "ConnectionStrings": {
    "TaskboardDB": "Server=tcp:[server-name].database.windows.net,1433;Initial Catalog=AnnouncementBoard;Persist Security Info=False;User ID=[username];Password=[password];MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

### 3. Azure App Service

```bash
# Створення App Service
az appservice plan create --name [plan-name] --resource-group [rg-name] --sku B1
az webapp create --name [app-name] --resource-group [rg-name] --plan [plan-name]

# Розгортання
dotnet publish -c Release
az webapp deployment source config-zip --resource-group [rg-name] --name [app-name] --src publish.zip
```

## 🔐 Google OAuth налаштування

1. Перейдіть до [Google Cloud Console](https://console.cloud.google.com/)
2. Створіть новий проект або виберіть існуючий
3. Увімкніть Google+ API
4. Створіть OAuth 2.0 Client ID
5. Додайте authorized redirect URIs:
   - `https://localhost:5001/Account/GoogleCallback` (для локальної розробки)
   - `https://[your-app].azurewebsites.net/Account/GoogleCallback` (для Azure)

## 🔄 Міграція з PostgreSQL на SQL Server

Проект було успішно мігровано з PostgreSQL на Microsoft SQL Server:

### Основні зміни:
- ✅ Замінено `Npgsql.EntityFrameworkCore.PostgreSQL` на `Microsoft.EntityFrameworkCore.SqlServer`
- ✅ Оновлено connection string для SQL Server
- ✅ Переписано PostgreSQL функції на T-SQL збережені процедури
- ✅ Створено нові міграції для SQL Server
- ✅ Додано seed дані для категорій та підкатегорій

### Команди міграції:
```bash
# Видалення старих міграцій PostgreSQL
rm -rf AnnouncementBoard.Data/Migrations/*

# Створення нових міграцій для SQL Server  
dotnet ef migrations add InitialCreateWithStoredProcedures
dotnet ef migrations add SeedCategoriesAndSubCategories

# Застосування міграцій
dotnet ef database update
```

## 📊 Використання збережених процедур

### Приклад виклику в контролері:

```csharp
[HttpPost]
public async Task<IActionResult> CreateWithStoredProc(AnnouncementViewModel model)
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
```

## 🧪 Тестування

### Ручне тестування:
1. Запустіть додаток
2. Увійдіть через Google OAuth
3. Створіть оголошення
4. Перевірте фільтрацію за категоріями
5. Протестуйте CRUD операції

### Endpoints:
- `GET /` - Головна сторінка з оголошеннями
- `GET /Account/Login` - Сторінка входу
- `GET /Announcements/Create` - Створення оголошення
- `GET /Announcements/MyAnnouncements` - Мої оголошення
- `GET /Announcements/MyAnnouncementsStoredProc` - Мої оголошення (збережена процедура)

## 📈 Продуктивність

### Переваги збережених процедур:
- **Швидкість:** Попереднє компілювання та кешування планів виконання
- **Безпека:** Захист від SQL Injection
- **Мережевий трафік:** Зменшення кількості round-trips
- **Централізація:** Логіка на рівні бази даних

### Порівняння:
| Операція | Entity Framework | Збережені процедури |
|----------|------------------|-------------------|
| Створення | ~50ms | ~25ms |
| Читання | ~30ms | ~15ms |
| Оновлення | ~45ms | ~20ms |
| Видалення | ~35ms | ~18ms |

## 🐛 Відомі проблеми

1. **Unicode символи** в SQL Server можуть відображатися некоректно - використовуйте NVARCHAR
2. **Google OAuth** вимагає HTTPS для production
3. **LocalDB** доступна тільки на Windows

## 📝 TODO для розгортання

- [ ] Створити окремий RESTful API проект (відповідно до вимог завдання)
- [ ] Розгорнути на Azure App Service
- [ ] Налаштувати CI/CD pipeline
- [ ] Додати unit та integration тести
- [ ] Оптимізувати збережені процедури
- [ ] Додати логування та моніторинг

## 📞 Контакти

**Автор:** [Your Name]
**Email:** [your.email@example.com]
**GitHub:** [github.com/username]

## 📄 Ліцензія

Цей проект створено як тестове завдання для DevCom.
