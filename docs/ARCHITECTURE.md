# Architecture

## Overview

The application follows a **layered architecture** with clear separation of concerns. The backend and frontend are fully decoupled — they run as independent processes and communicate exclusively through a RESTful API.

```
┌─────────────────────────────┐      HTTPS       ┌─────────────────────────────┐
│   Angular SPA (Frontend)    │ ◄──────────────► │   ASP.NET Core (Backend)    │
│   localhost:52867           │    /api/*         │   localhost:7206            │
└─────────────────────────────┘                   └─────────────────────────────┘
                                                           │
                                                           ▼
                                                  ┌─────────────────┐
                                                  │   SQL Server     │
                                                  │   (LocalDB)      │
                                                  └─────────────────┘
```

## SOLID Principles

This project follows all five SOLID principles:

### S — Single Responsibility Principle

Each class has exactly one reason to change:

| Layer | Responsibility | Example |
|-------|---------------|---------|
| **Controllers** | Handle HTTP requests/responses | `BooksController` — routes, status codes, content negotiation |
| **Services** | Business logic, validation, DTO mapping | `BookService` — ISBN uniqueness check, copy count validation |
| **Repositories** | Data access (EF Core queries) | `BookRepository` — LINQ queries, persistence |
| **Models** | Define the data shape | `Book` — entity properties and relationships |
| **DTOs** | Define API contracts | `BookDto` / `CreateBookDto` — what the client sends and receives |

### O — Open/Closed Principle

The system is open for extension but closed for modification:

- Need caching? Create `CachedBookRepository` implementing `IBookRepository` — wrap the existing repository. No changes to `BookService`.
- Need audit logging? Create a decorator around `IBookService`. No changes to `BooksController`.
- Need a different database? Create a new `IBookRepository` implementation. No changes to services or controllers.

### L — Liskov Substitution Principle

Any implementation can be substituted for its interface without breaking behavior:

- `BookRepository` can be replaced with `InMemoryBookRepository` (for testing) — `BookService` works identically.
- `BookService` can be replaced with `MockBookService` — `BooksController` works identically.

### I — Interface Segregation Principle

Interfaces are small and focused. Each service interface only exposes operations relevant to its domain:

- `IBookService` — book CRUD operations only
- `IMemberService` — member CRUD operations only
- `ILoanService` — checkout, return, dashboard only

Controllers depend only on the single interface they need.

### D — Dependency Inversion Principle

High-level modules depend on abstractions, not concrete implementations:

```
BooksController  →  IBookService  ←  BookService  →  IBookRepository  ←  BookRepository
     (HTTP)          (interface)     (business)        (interface)          (EF Core)
```

All wiring happens in `Program.cs` via the built-in .NET DI container:

```csharp
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
```

## Backend Layers

### 1. Controllers (`Controllers/`)

Thin controllers that only handle HTTP concerns:

- Parse route/query parameters
- Call the appropriate service method
- Map `ServiceResult` to HTTP status codes via `ApiControllerBase`
- No business logic, no database access

### 2. Services (`Services/`)

Contains all business rules and orchestration:

- Validate business constraints (e.g., "can't delete a book with active loans")
- Map between domain models and DTOs
- Coordinate across multiple repositories when needed (e.g., `LoanService` uses book, member, and loan repositories)
- Return `ServiceResult<T>` to communicate success/failure without throwing exceptions

### 3. Repositories (`Repositories/`)

Pure data access layer:

- Execute EF Core queries
- Handle includes/eager loading
- Expose focused query methods (e.g., `ExistsByIsbnAsync`, `GetByIdWithActiveLoansAsync`)
- No business logic — just CRUD and query composition

### 4. ServiceResult Pattern

Instead of throwing exceptions for expected business errors, services return a `ServiceResult`:

```csharp
public class ServiceResult<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public ServiceErrorType ErrorType { get; }  // NotFound, Conflict, BadRequest
}
```

The `ApiControllerBase` maps these to HTTP responses:

| ServiceErrorType | HTTP Status |
|-----------------|-------------|
| Success | 200 OK / 201 Created / 204 No Content |
| NotFound | 404 Not Found |
| Conflict | 409 Conflict |
| BadRequest | 400 Bad Request |

## Frontend Architecture

### Component Structure

```
src/app/
├── components/
│   ├── shared/navbar/          # Navigation bar (always visible)
│   ├── dashboard/              # Home page with stats
│   ├── books/
│   │   ├── book-list/          # Searchable book table with CRUD actions
│   │   └── book-form/          # Add/edit book form (shared component)
│   ├── members/
│   │   ├── member-list/        # Searchable member table
│   │   └── member-form/        # Add/edit member form
│   └── loans/
│       ├── loan-list/          # Loan table with filter and return action
│       └── loan-checkout/      # Book checkout form
├── services/                   # HTTP services (one per domain entity)
├── models/                     # TypeScript interfaces matching backend DTOs
└── app-routing.module.ts       # Route definitions
```

### Data Flow

```
User Action → Component → Service (HttpClient) → Angular Proxy → ASP.NET API
                                                                       │
                                                                  Controller
                                                                       │
                                                                    Service
                                                                       │
                                                                  Repository
                                                                       │
                                                                   Database
```

### API Proxy

In development, Angular's dev server proxies `/api/*` requests to the ASP.NET backend. This avoids CORS issues during development while keeping the projects independent. The proxy is configured in `src/proxy.conf.js`.

## Database

- **Provider**: SQL Server LocalDB
- **ORM**: Entity Framework Core 10
- **Strategy**: Code-First with `EnsureCreated()` (auto-creates on first run)
- **Seed Data**: 8 sample books and 4 sample members are seeded via `OnModelCreating`

### Entity Relationships

```
Book (1) ──────── (many) Loan (many) ──────── (1) Member
  │                        │                        │
  ├── Id (PK)              ├── Id (PK)              ├── Id (PK)
  ├── Title                ├── BookId (FK)           ├── FullName
  ├── Author               ├── MemberId (FK)         ├── Email (unique)
  ├── ISBN (unique)        ├── BorrowDate            ├── Phone
  ├── Genre                ├── DueDate               ├── MembershipDate
  ├── PublishedYear        ├── ReturnDate             └── IsActive
  ├── TotalCopies          └── (IsReturned, IsOverdue
  └── AvailableCopies           computed properties)
```
