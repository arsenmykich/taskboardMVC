# 🛠️ Налаштування проекту AnnouncementBoard

## 📋 Перед запуском

### 1. **Налаштування Google OAuth**

1. Перейдіть до [Google Cloud Console](https://console.cloud.google.com/)
2. Створіть новий проект або оберіть існуючий
3. Увімкніть Google+ API
4. Створіть OAuth 2.0 Client ID:
   - Application type: Web application
   - Authorized redirect URIs: `https://localhost:7149/signin-google`
5. Скопіюйте Client ID та Client Secret

### 2. **Налаштування конфігурації**

1. Скопіюйте `appsettings.Development.template.json` в `appsettings.Development.json`
2. Замініть placeholder-и на справжні значення:

```json
{
  "GoogleAuth": {
    "ClientId": "ваш_справжній_client_id",
    "ClientSecret": "ваш_справжній_client_secret"
  },
  "ConnectionStrings": {
    "TaskboardDB": "Host=localhost;Database=AnnouncementBoardDB;Username=postgres;Password=ваш_пароль"
  }
}
```

### 3. **Налаштування бази даних**

1. Встановіть PostgreSQL
2. Створіть базу даних `AnnouncementBoardDB`
3. Запустіть міграції:

```bash
cd AnnouncementBoard.Web
dotnet ef database update
```

## 🚀 **Запуск проекту**

```bash
cd AnnouncementBoard.Web
dotnet run
```

## ⚠️ **ВАЖЛИВО!**

- **НЕ комітьте файл `appsettings.Development.json`** - він містить секрети
- Файл уже додано до `.gitignore`
- Використовуйте тільки template файл для референсу

## 🔧 **Структура проекту**

- `AnnouncementBoard.Core` - Моделі та інтерфейси
- `AnnouncementBoard.Data` - Репозиторії та DbContext  
- `AnnouncementBoard.Business` - Бізнес логіка
- `AnnouncementBoard.Web` - MVC веб-додаток

## 📱 **Функціональність**

- ✅ Google OAuth автентифікація
- ✅ CRUD операції з оголошеннями
- ✅ Фільтрація по категоріях та підкатегоріях
- ✅ Пошук по тексту
- ✅ Repository Pattern + Unit of Work
- ✅ Responsive Bootstrap UI
- ✅ AJAX операції 