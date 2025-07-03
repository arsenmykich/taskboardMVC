
---

# AnnouncementBoard – Дошка оголошень

## 📋 Опис проєкту

**Дошка оголошень** – це веб-додаток для створення, перегляду, редагування та видалення оголошень з категоріями та підкатегоріями. Проєкт реалізовано на ASP.NET Core MVC з використанням POSTGRES (збережені функції) та Google OAuth для автентифікації. Додаток розгорнуто на Azure App Service.

---
**Весрія з ms sql і процедурами на гілці feature/ms-sql--version**
---

---

## ✅ Відповідність тестовому завданню

- [x] CRUD для оголошень
- [x] Категорії та підкатегорії (повний перелік)
- [x] Postgres + збережені функції для всіх CRUD-операцій
- [x] Google OAuth (автентифікація)
- [x] Розгортання на Azure

---

## 🖼️ Як це виглядає

### Головна сторінка
![image](https://github.com/user-attachments/assets/9cc6404b-9b30-4203-9154-e66644fa2261)


### Створення оголошення
![image](https://github.com/user-attachments/assets/7d7807dd-f474-47d1-b387-6e9d02a911aa)


### Оголошення користуваяа
![image](https://github.com/user-attachments/assets/d36f9e9f-711c-4881-abb6-b4d0be6b8cd3)


---

## 🏗️ Архітектура проєкту

```
taskboardMVC/
├── AnnouncementBoard.Core/        # Моделі та інтерфейси
│   ├── Models/                   # Announcement, Category, SubCategory, User
│   └── Interfaces/               # Репозиторії та сервіси
├── AnnouncementBoard.Data/        # Доступ до даних, міграції, репозиторії
│   ├── Repositories/
│   ├── Migrations/
│   └── AnnouncementBoardDbContext.cs
├── AnnouncementBoard.Business/    # Бізнес-логіка, сервіси, валідація
│   └── Services/
└── AnnouncementBoard.Web/         # ASP.NET Core MVC фронтенд
    ├── Controllers/
    ├── Views/
    └── Models/ViewModels/
```

---

## 🗄️ Категорії та підкатегорії

- **Побутова техніка:** Холодильники, Пральні машини, Бойлери, Печі, Витяжки, Мікрохвильові печі
- **Комп'ютерна техніка:** ПК, Ноутбуки, Монітори, Принтери, Сканери
- **Смартфони:** Android смартфони, iOS/Apple смартфони
- **Інше:** Одяг, Взуття, Аксесуари, Спортивне обладнання, Іграшки

---

## 🚀 Як запустити локально

### 1. Клонування та відновлення залежностей

```bash
git clone https://github.com/arsenmykich/taskboardMVC.git
cd taskboardMVC
dotnet restore
```

### 2. Налаштування бази даних

```bash
dotnet ef database update --project AnnouncementBoard.Data --startup-project AnnouncementBoard.Web
```

### 3. Налаштування Google OAuth

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

### 4. Запуск

```bash
dotnet run --project AnnouncementBoard.Web
```

---

## 🧪 Тестування

1. Увійдіть через Google OAuth.
2. Створіть оголошення.
3. Перевірте фільтрацію за категоріями/підкатегоріями.
4. Оновіть або видаліть оголошення.
5. Перевірте роботу на мобільному пристрої.

---
## Історія деплоїв
![image](https://github.com/user-attachments/assets/fa72bdf4-b564-4f29-a538-586961dbf451)



## 📞 Контакти

**Автор:** Arsen Mykich  
**GitHub:** [github.com/arsenmykich/taskboardMVC](https://github.com/arsenmykich/taskboardMVC)

---

## 📄 Ліцензія

Цей проєкт створено як тестове завдання для DevCom.

