# Silk Road E-Commerce

Silk Road E-Commerce is a full-stack shopping application built with an Angular front-end and a .NET 8 back-end.

## Project Overview

- `client/` - Angular 18 application with server-side rendering support and UI for browsing products, managing a basket, and placing orders.
- `src/SilkRoad.API/` - ASP.NET Core Web API that exposes product, category, basket, order, payment, and authentication endpoints.
- `src/SilkRoad.Infrastructure/` - Infrastructure services, repositories, and database context for the API.
- `src/SilkRoad.Core/` - Core DTOs, entities, interfaces, and shared logic.
- `Database-Scripts/` - SQL scripts for generating database tables, soft delete patches, and seed data for countries, cities, and states.

## Key Technologies

- Angular 18
- ASP.NET Core (.NET 8)
- Entity Framework Core
- SQL Server
- Redis (via StackExchange.Redis)
- Swagger/OpenAPI
- Bootstrap and Angular Material

## Prerequisites

- .NET SDK 8.0
- Node.js 18 or newer
- npm
- SQL Server or a compatible database instance

## Setup

1. Clone the repository.
2. Open the solution `SilkRoad.slnx` in Visual Studio or VS Code.
3. Configure database settings in `src/SilkRoad.API/appsettings.json` or `appsettings.Development.json`.
4. Run the SQL scripts in `Database-Scripts/` to create schema and seed initial data.

## Running the API

From the `src/SilkRoad.API/` folder:

```bash
cd src/SilkRoad.API
dotnet run
```

By default, the API serves Swagger in development mode.

## Running the Client

From the `client/` folder:

```bash
cd client
npm install
npm start
```

Open `http://localhost:4200/` in the browser.

## Notes

- The API Cors policy allows requests from `http://localhost:4200`.
- The Angular client includes server-side rendering support via `serve:ssr:client`.
- Swagger is enabled during development for API exploration and testing.

## Folder Structure

- `client/` - Angular application and front-end assets
- `src/SilkRoad.API/` - API implementation and startup configuration
- `src/SilkRoad.Core/` - DTOs, entities, services, and interfaces
- `src/SilkRoad.Infrastructure/` - database access, repository layer, and infrastructure registrations
- `Database-Scripts/` - database initialization and migration scripts

## Additional Resources

- Angular CLI: https://angular.io/cli
- ASP.NET Core docs: https://learn.microsoft.com/aspnet/core
- Entity Framework Core: https://learn.microsoft.com/ef/core
