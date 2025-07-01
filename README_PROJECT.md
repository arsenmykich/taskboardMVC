# Дошка оголошень - ASP.NET Core MVC

Повноцінна 3-шарова архітектура веб-додатку для управління оголошеннями з Google Authentication.

## 🏗️ Архітектура проекту

### 3-шарова архітектура:
- **AnnouncementBoard.Core** - Моделі, інтерфейси, контракти
- **AnnouncementBoard.Data** - Data Access Layer (Repository + Unit of Work)
- **AnnouncementBoard.Business** - Business Logic Layer (Services)
- **AnnouncementBoard.Web** - Presentation Layer (MVC Controllers + Views)

## 📊 Діаграма бази даних

```
// Entity Relationship Diagram for eraser.io

// Users table
Users {
  Id: string [pk]
  Email: string [unique]
  Name: string
  CreatedAt: datetime
  UpdatedAt: datetime
}

// Categories table
Categories {
  Id: int [pk]
  Name: string [unique]
  Description: string
  CreatedAt: datetime
}

// SubCategories table  
SubCategories {
  Id: int [pk]
  Name: string
  Description: string
  CategoryId: int [fk]
  CreatedAt: datetime
}

// Announcements table
Announcements {
  Id: int [pk]
  Title: string
  Description: text
  Status: boolean
  CreatedDate: datetime
  UpdatedDate: datetime
  UserId: string [fk]
  CategoryId: int [fk]
  SubCategoryId: int [fk]
}

// Relationships
Users ||--o{ Announcements
Categories ||--o{ SubCategories
Categories ||--o{ Announcements
SubCategories ||--o{ Announcements
```

## 🚀 Функціональність

### ✅ Реалізовано:
- **CRUD операції** для оголошень
- **Google Authentication** з автоматичним створенням користувачів
- **Категорії та підкатегорії** (4 основні категорії, 18 підкатегорій)
- **Пошук та фільтрація** оголошень
- **Авторизація** - тільки автентифіковані користувачі можуть створювати/редагувати
- **3-шарова архітектура** з Repository pattern і Unit of Work
- **PostgreSQL** база даних з Entity Framework Core
- **Seed data** для категорій та підкатегорій

### 📂 Категорії:
1. **Побутова техніка** (Холодильники, Пральні машини, Бойлери, Печі, Витяжки, Мікрохвильові печі)
2. **Комп'ютерна техніка** (ПК, Ноутбуки, Монітори, Принтери, Сканери)
3. **Смартфони** (Android, iOS/Apple)
4. **Інше** (Одяг, Взуття, Аксесуари, Спортивне обладнання, Іграшки)

## ⚙️ Налаштування

### 1. Передумови
- .NET 8.0 SDK
- PostgreSQL сервер
- Google OAuth2 credentials

### 2. Налаштування бази даних
```bash
# База даних вже створена з connection string:
Server=localhost;Port=5432;User Id=postgres;Password=1102;Database=taskboardDb;

# Міграції вже застосовані
dotnet ef database update --project AnnouncementBoard.Data --startup-project AnnouncementBoard.Web
```

### 3. Google OAuth налаштування
1. Створіть проект в [Google Cloud Console](https://console.cloud.google.com/)
2. Увімкніть Google+ API
3. Створіть OAuth 2.0 Client ID
4. Додайте redirect URIs:
   - `https://localhost:7001/Account/GoogleCallback`
   - `http://localhost:5000/Account/GoogleCallback`
5. Замініть в `appsettings.json`:
```json
{
  "GoogleAuth": {
    "ClientId": "YOUR_GOOGLE_CLIENT_ID",
    "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
  }
}
```

### 4. Запуск проекту
```bash
dotnet run --project AnnouncementBoard.Web
```

## 🎯 Основні маршрути

- `/` - Головна сторінка з оголошеннями
- `/Home/Details/{id}` - Деталі оголошення
- `/Account/Login` - Вхід через Google
- `/Account/Dashboard` - Dashboard користувача
- `/Announcements/MyAnnouncements` - Мої оголошення
- `/Announcements/Create` - Створити оголошення
- `/Announcements/Edit/{id}` - Редагувати оголошення

## 🏛️ Архітектурні рішення

### Patterns використані:
- **Repository Pattern** - абстракція доступу до даних
- **Unit of Work** - управління транзакціями
- **Dependency Injection** - IoC контейнер
- **Service Layer** - бізнес-логіка
- **MVC Pattern** - розділення відповідальностей

### Технології:
- **ASP.NET Core 8.0 MVC**
- **Entity Framework Core** з PostgreSQL
- **Google Authentication**
- **Bootstrap** для UI
- **Repository + Unit of Work patterns**

## 📝 TODO для завершення

### Views (потрібно створити):
1. `Home/Index.cshtml` - головна сторінка з списком оголошень
2. `Home/Details.cshtml` - деталі оголошення
3. `Account/Login.cshtml` - сторінка входу
4. `Account/Dashboard.cshtml` - dashboard користувача
5. `Announcements/MyAnnouncements.cshtml` - список оголошень користувача
6. `Announcements/Create.cshtml` - форма створення
7. `Announcements/Edit.cshtml` - форма редагування
8. `Announcements/Delete.cshtml` - підтвердження видалення
9. `Shared/_Layout.cshtml` - оновити layout з навігацією

### JavaScript:
- Динамічне завантаження підкатегорій при виборі категорії
- AJAX для пошуку та фільтрації

## 🔧 Команди для розробки

```bash
# Компіляція
dotnet build

# Запуск
dotnet run --project AnnouncementBoard.Web

# Нова міграція
dotnet ef migrations add [MigrationName] --project AnnouncementBoard.Data --startup-project AnnouncementBoard.Web

# Застосування міграцій
dotnet ef database update --project AnnouncementBoard.Data --startup-project AnnouncementBoard.Web
```

## 📊 Стан проекту

✅ **Завершено:**
- 3-шарова архітектура
- Repository pattern + Unit of Work
- Business Logic Layer
- Controllers з автентифікацією
- Models та ViewModels
- База даних з seed data
- Google Authentication налаштування
- Міграції Entity Framework

⏳ **Потребує доробки:**
- Views (Razor Pages)
- JavaScript для UI
- CSS стилізація
- Валідація форм
- Error handling pages

**Проект готовий на 80%** - основна архітектура та бізнес-логіка повністю реалізовані! 