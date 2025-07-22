# 📺 Talabat - E-Commerce Backend API

A scalable and modular E-Commerce backend system built with **ASP.NET Core Web API**, following **Clean Architecture** and industry best practices.

---

## 🚀 Technologies Used

* ASP.NET Core 5 / 6
* Entity Framework Core
* SQL Server
* Redis (for Caching)
* Stripe (Payment Integration)
* AutoMapper
* JWT Authentication
* .NET Core Identity

---

## 🧱 Architecture Patterns

* Clean Architecture
* Repository Pattern
* Unit of Work
* Specification Pattern

---

## ✨ Features

* 🔐 JWT Authentication & Identity Management
* 📋 Stripe Payment Gateway Integration
* 🥚 Shopping Basket with Redis Caching
* 🚸 Order Management & Delivery Method
* 🔍 Advanced Product Filtering & Pagination
* ⚙️ Custom Middleware for Error Handling
* 🧹 Modular & Maintainable Code Structure

---

## 📁 Project Structure

```
/Talabat.APIs         → API Layer (Controllers, Middleware, Startup)
|__ appsettings.json  → Configuration files
/Talabat.Repository   → EF Core DbContexts, Repositories, Data Seeding
/TalabatCore          → Core Entities, Interfaces, Specifications
/Talabate.Sevice      → Business Logic & Services
```

---

## 🔧 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/mohamedabuelmgd/talabat.git
```

### 2. Setup database connection

* Configure `appsettings.json` with your SQL Server string

### 3. Apply Migrations (if needed)

```bash
dotnet ef database update
```

### 4. Run the project

```bash
dotnet run
```

---

## 📳 API Testing (Postman)

You can test all endpoints using the official Postman collection:

👉 [Download Talabat Postman Collection](./postman/Talabat.postman_collection.json)

###  Import Instructions:

1. Open **Postman**
2. Click **Import**
3. Select the file `Talabat.postman_collection.json`
4. Start sending requests 🌟

---

## 👨‍💻 Developed By

**Mohamed Saeed Abuelmgd**
[LinkedIn](https://www.linkedin.com/in/mohamed-abuelmgd-519528237/)
[GitHub](https://github.com/mohamedabuelmgd)
