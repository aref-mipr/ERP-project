# ERP System

A modular Enterprise Resource Planning (ERP) system built with ASP.NET Core and Entity Framework Core.

This project is under active development and continuously improved by adding new modules, features, and business logic. The goal is to build a practical ERP application that demonstrates real-world software architecture, domain modeling, and business process implementation.

---

## 🚀 Project Status

🟡 In Development

This repository contains the initial version of the ERP system.

New features and improvements will be released periodically as the project evolves.

---

## 📌 Overview

The purpose of this project is to create a small but extensible ERP system capable of managing different business operations such as:

- Financial management
- Budget management
- Employee management
- Product management
- Customer and order management
- Expense tracking

This project focuses not only on implementing CRUD operations but also on applying proper software architecture principles, business rules, and maintainable code structure.

---

## 🛠 Technologies

### Backend

- ASP.NET Core Razor Pages (.NET 9)
- C#
- Entity Framework Core 9
- SQL Server
- LINQ

### Frontend

- Razor Pages
- HTML5
- CSS3
- JavaScript
- Bootstrap 5

### Additional Libraries and Tools

- FluentValidation
- Dependency Injection
- Entity Framework Core Migrations
- Fluent API Configurations

---

## 🏗 Architecture

The project follows the Onion Architecture pattern:

```text
                 ┌─────────────────────────┐
                 │           Web           │
                 │     UI, Razor Pages     │
                 └────────────┬────────────┘
                              │
                 ┌────────────▼────────────┐
                 │     Infrastructure      │
                 │ Database, EF Core, APIs │
                 └────────────┬────────────┘
                              │
                 ┌────────────▼────────────┐
                 │      Application        │
                 │ Services, Use Cases,    │
                 │ Interfaces              │
                 └────────────┬────────────┘
                              │
                 ┌────────────▼────────────┐
                 │        Domain           │
                 │ Entities, Enums,        │
                 │ Business Rules          │
                 └─────────────────────────┘
```

The architecture is designed to separate business logic from UI and infrastructure concerns, making the system easier to maintain, test, and extend.

---

# ✨ Implemented Features

## 💰 Financial Management

Features:

- Initial capital setup
- Financial transaction management
- Budget tracking
- Expense recording

The financial module is designed to keep track of business financial activities and provide a foundation for future reporting features.

---

## 👨‍💼 Employee Management

Features:

- Employee registration
- Employee information management
- Employee status tracking
- Salary information management
- Salary payment logic

---

## 📦 Product Management

Features:

- Product management
- Product item management
- Product item pricing
- Product item status tracking

---

## 🛒 Order Management

Features:

- Customer orders
- Order items
- Product selection
- Discount calculation
- Final price calculation
- Order status management

---

# 🗄 Database

The project uses Entity Framework Core with SQL Server.

Database features:

- Code First approach
- EF Core Migrations
- Fluent API configurations
- Entity relationships
- Decimal precision configuration

---

# 🔮 Planned Features

Future improvements include:

- Authentication and authorization
- User and role management
- Inventory management
- Advanced financial reports
- Dashboard improvements
- More ERP modules
- Improved UI/UX
- Additional business workflows

---

# 📷 Screenshots

Screenshots will be added as the user interface becomes more complete.

---

# ▶️ Running the Project

## Requirements

- .NET 9 SDK
- SQL Server
- Visual Studio 2022/2026 or another compatible IDE

---

## Setup

### 1. Clone the repository

git clone <repository-url>

### 2. Configure database connection

Update the connection string in:

appsettings.json

### 3. Apply database migrations

dotnet ef database update

### 4. Run the project

dotnet run

---

# 📈 Development Roadmap

The project is continuously updated.

Each release may include:

- New modules
- New business features
- UI improvements
- Bug fixes
- Architecture improvements

---

# 👤 Author
Developed as a personal software engineering project to explore real-world ERP design, backend development, domain-driven thinking, and scalable application architecture.
