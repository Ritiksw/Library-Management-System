# Library Management System

A full-stack library management system built with **ASP.NET Core 10** (backend API) and **Angular 16** (frontend SPA). The backend and frontend are fully decoupled — each runs independently and communicates via a RESTful API.

## Tech Stack

| Layer    | Technology                              |
|----------|-----------------------------------------|
| Backend  | ASP.NET Core 10, C# 13                 |
| Frontend | Angular 16, TypeScript 5.1              |
| Database | SQL Server (LocalDB)                    |
| ORM      | Entity Framework Core 10                |
| API Docs | OpenAPI / Swagger                       |

## Features

- **Books** — Add, edit, delete, and search books by title, author, ISBN, or genre. Tracks total and available copies.
- **Members** — Register members, manage their profiles, and track active status.
- **Loans** — Checkout books to members, return books, and track due dates. Prevents duplicate loans and enforces availability.
- **Dashboard** — Overview with total books, active members, active loans, overdue count, and recent loan activity.
- **Search** — All list views support real-time search/filter.
- **Validation** — Server-side validation with meaningful error messages surfaced in the UI.

## Project Structure

```
LibraryManagementSystem/
├── LibraryManagementSystem.Server/    # ASP.NET Core Web API
│   ├── Controllers/                   # Thin API controllers (HTTP only)
│   ├── Services/                      # Business logic and DTO mapping
│   ├── Repositories/                  # Data access layer (EF Core)
│   ├── Models/                        # Entity models
│   ├── DTOs/                          # Data transfer objects
│   ├── Data/                          # DbContext and seed data
│   └── Program.cs                     # App entry point and DI configuration
├── librarymanagementsystem.client/    # Angular SPA
│   └── src/app/
│       ├── components/                # UI components (dashboard, books, members, loans)
│       ├── services/                  # HTTP services for API communication
│       └── models/                    # TypeScript interfaces
├── docs/                              # Documentation
│   ├── ARCHITECTURE.md                # Design principles and patterns
│   ├── API.md                         # API endpoint reference
│   └── SETUP.md                       # Development environment setup
└── README.md
```

## Quick Start

> Detailed setup instructions are in [docs/SETUP.md](docs/SETUP.md).

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) with npm
- SQL Server LocalDB (included with Visual Studio)

### Run the Backend

```bash
cd LibraryManagementSystem.Server
dotnet run
```

The API starts on **https://localhost:7206**. The database is auto-created with seed data on first run.

### Run the Frontend

```bash
cd librarymanagementsystem.client
npm install
npm start
```

The Angular app starts on **https://localhost:52867** and proxies API calls to the backend.

## Documentation

| Document | Description |
|----------|-------------|
| [Architecture](docs/ARCHITECTURE.md) | SOLID principles, layered design, dependency flow |
| [API Reference](docs/API.md) | All REST endpoints with request/response examples |
| [Setup Guide](docs/SETUP.md) | Step-by-step dev environment setup |


