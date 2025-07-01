# Database Diagram for eraser.io

```
// Entity Relationship Diagram for Announcements Board

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

## Categories and SubCategories Data:

**Побутова техніка:**
- Холодильники
- Пральні машини  
- Бойлери
- Печі
- Витяжки
- Мікрохвильові печі

**Комп'ютерна техніка:**
- ПК
- Ноутбуки
- Монітори
- Принтери
- Сканери

**Смартфони:**
- Android смартфони
- iOS/Apple смартфони

**Інше:**
- Одяг
- Взуття
- Аксесуари
- Спортивне обладнання
- Іграшки 