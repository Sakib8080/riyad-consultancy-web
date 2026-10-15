# Reyad Business Solution and Consultancy – Website + Admin Panel

ASP.NET Core MVC, Entity Framework Core 6, SQL Server, Clean Architecture.

```
src/
  ReyadConsultancy.Domain          entities, enums (no dependencies)
  ReyadConsultancy.Application     interfaces, services (CRUD, website content, media, dashboard)
  ReyadConsultancy.Infrastructure  EF Core DbContext, ASP.NET Identity, SQL Server, seeding
  ReyadConsultancy.Web             public website (Website.zip design) + /admin area (AdminLTE design)
```

## Run

1. Install the .NET 6 SDK (or change `TargetFramework`/`EfCoreVersion` in `Directory.Build.props`, e.g. `net8.0` / `8.0.*`) and SQL Server (Express / LocalDB / full).
2. Edit `src/ReyadConsultancy.Web/appsettings.json` -> `ConnectionStrings:DefaultConnection`.
   SQL login example: `Server=localhost;Database=ReyadConsultancyDb;User Id=sa;Password=...;TrustServerCertificate=True`
3. `dotnet run --project src/ReyadConsultancy.Web`
   The database is created on first start and filled with starter content.

Website: `https://localhost:7101/`  Admin: `https://localhost:7101/admin`

## Admin login

* Seeded administrator (appsettings `SeedAdmin`): `admin@example.com` / `Admin@12345` – **change it** (Admin > Change Password) and edit/remove `SeedAdmin` before going live.
* Register page: `/admin/Account/Register`. New accounts cannot sign in until an administrator approves them in Admin > Users.
  (If the database has no users at all, the first registered account becomes the administrator.)

## What the admin manages (everything is stored in SQL Server)

Site settings (logo, contact details, social links, footer), home slider, feature boxes, about items/highlights, page text blocks,
services, FAQs, counters, team, testimonials, client logos, blog posts / categories / comments (moderated), contact messages,
appointments (status), newsletter subscribers, admin users. Uploaded images are stored in the `MediaFiles` table and served from `/media/{id}`.

## EF Core migrations (recommended for production)

Without migrations the app uses `EnsureCreated()`. To use migrations instead:

```
dotnet tool install --global dotnet-ef --version 6.*
dotnet ef migrations add InitialCreate -p src/ReyadConsultancy.Infrastructure -s src/ReyadConsultancy.Web
dotnet ef database update            -p src/ReyadConsultancy.Infrastructure -s src/ReyadConsultancy.Web
```
(The app applies migrations automatically at startup once they exist.)
