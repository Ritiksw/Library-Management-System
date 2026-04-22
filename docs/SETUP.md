# Development Setup Guide

## Prerequisites

Before you begin, make sure you have the following installed:

| Tool | Version | Purpose |
|------|---------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ | Backend runtime and build tools |
| [Node.js](https://nodejs.org/) | 18+ | Frontend runtime |
| npm | 9+ (comes with Node.js) | Frontend package manager |
| SQL Server LocalDB | (included with Visual Studio) | Development database |

### Verify installations

```bash
dotnet --version    # Should show 10.x.x
node --version      # Should show v18.x.x or higher
npm --version       # Should show 9.x.x or higher
```

### Verify LocalDB

```bash
sqllocaldb info
```

If `MSSQLLocalDB` is listed, you're good. If not, install it via Visual Studio Installer (under "Data storage and processing" workload) or the [standalone installer](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb).

---

## Backend Setup

### 1. Navigate to the server project

```bash
cd LibraryManagementSystem.Server
```

### 2. Restore NuGet packages

```bash
dotnet restore
```

### 3. Trust the HTTPS dev certificate (first time only)

```bash
dotnet dev-certs https --trust
```

### 4. Run the backend

```bash
dotnet run
```

The API will be available at:
- **https://localhost:7206** (HTTPS)
- **http://localhost:5096** (HTTP)

On first run, the database `LibraryManagementDb` is automatically created in LocalDB with seed data (8 books, 4 members).

### 5. Verify the API is working

Open a browser or use curl:

```bash
curl -k https://localhost:7206/api/books
```

You should see a JSON array of books.

### Using Visual Studio 2022/2026

1. Open `LibraryManagementSystem.slnx`
2. Make sure the launch profile is set to **https** (not Docker)
3. Press **F5** to build and run with debugging

> If Visual Studio shows a Docker error, check that `Properties/launchSettings.json` does not have a Docker profile, and that `.csproj.user` has `<ActiveDebugProfile>https</ActiveDebugProfile>`.

---

## Frontend Setup

### 1. Navigate to the client project

```bash
cd librarymanagementsystem.client
```

### 2. Install npm dependencies

```bash
npm install
```

### 3. Start the Angular dev server

```bash
npm start
```

The Angular app will be available at **https://localhost:52867**.

> The first time you run this, Angular generates an HTTPS certificate via `aspnetcore-https.js`. You may see a browser warning — accept the self-signed certificate to proceed.

### How the proxy works

In development, the Angular dev server proxies all `/api/*` requests to the ASP.NET backend at `https://localhost:7206`. This is configured in `src/proxy.conf.js`:

```javascript
const PROXY_CONFIG = [
  {
    context: ["/api"],
    target: "https://localhost:7206",
    secure: false
  }
]
```

This means:
- Angular serves the UI on port 52867
- API calls like `GET /api/books` are forwarded to `https://localhost:7206/api/books`
- No CORS issues in development

---

## Running Both Together

You need **two terminals** running simultaneously:

**Terminal 1 — Backend:**

```bash
cd LibraryManagementSystem.Server
dotnet run
```

**Terminal 2 — Frontend:**

```bash
cd librarymanagementsystem.client
npm start
```

Then open **https://localhost:52867** in your browser.

> Always start the backend first so the API is available when the frontend starts making requests.

---

## Database

### Connection string

Defined in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Reset the database

To drop and recreate the database with fresh seed data, delete the database and restart the backend:

```bash
sqllocaldb stop MSSQLLocalDB
sqllocaldb delete MSSQLLocalDB
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
dotnet run
```

Or connect to LocalDB via SQL Server Management Studio / Azure Data Studio and drop `LibraryManagementDb` manually.

---

## Common Issues

### "Visual Studio container tools require Docker Desktop"

The launch profile is set to Docker. Fix: ensure `Properties/launchSettings.json` has the `https` profile (not `Container (Dockerfile)`) and that `.csproj.user` contains:

```xml
<ActiveDebugProfile>https</ActiveDebugProfile>
```

### Angular shows "Loading... Please refresh"

The backend isn't running yet. Start the backend first, then refresh the Angular app.

### SSL certificate errors in the browser

Run `dotnet dev-certs https --trust` to trust the ASP.NET dev certificate. For the Angular certificate, accept the browser's warning for `localhost:52867`.

### Port already in use

Kill the existing process:

```bash
# Windows
netstat -ano | findstr :7206
taskkill /PID <pid> /F
```

Or change the port in `Properties/launchSettings.json`.
