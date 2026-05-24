# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

CMIS (Church Management Information System) is a Blazor Web App targeting .NET 10 with MySQL via Entity Framework Core. It manages church membership, finances, events, appointments, and role-based access across a church district hierarchy.

## Commands

```bash
# Restore packages
dotnet restore

# Run the main Blazor app
dotnet run --project CMIS/CMIS.csproj

# Apply EF Core migrations
dotnet ef database update --project CMIS/CMIS.csproj

# Run the appointment microservice
dotnet run --project API/appointment-service/appointment-service.csproj

# Build solution
dotnet build CMIS.slnx
```

No test project exists in the repository.

## Architecture

### Two-Project Structure

- **`CMIS/`** — Main Blazor Web App (server-side interactive rendering). All business logic, UI, and data access live here.
- **`API/appointment-service/`** — Standalone ASP.NET Core REST API for appointment/service scheduling. It duplicates some models from the main project and is not yet fully integrated.

### Data Model Hierarchy

The organizational structure flows: **District → Church → Ministry → Profile**. The `LeadershipAssignment` model connects a `Profile` to a `Role` within that hierarchy, and is the source of truth for access control.

Account (login credentials) and Profile (person data) are separate entities linked by `ProfileId`. Never conflate them.

### Role-Based Access

Five roles with distinct views: District Head, Head Pastor, Leadership Council, Ministry Head, Board of Directors. Pages are organized under `Components/Pages/{RoleName}/`. Route prefixes match role slugs (e.g., `/ministry-head/events`). All pages use `@rendermode InteractiveServer`.

### Service Layer

- `AuthService` — BCrypt verification, login lookup by email, password update
- `CustomAuthStateProvider` — Blazor `AuthenticationStateProvider` backed by the service. Use `@inject CustomAuthStateProvider` to get the current user in components.
- `EventService` — Loads `EventModel` with all child collections (program schedule, guests, personnel, equipment, transportation, expenses) via explicit `Include()` chains

### Database

MySQL via `Pomelo.EntityFrameworkCore.MySql` with auto-detected server version. `ApplicationDbContext` is in `CMIS/Data/`. Migrations live in `CMIS/Migrations/`. `DatabaseSeeder.cs` seeds initial roles and an admin account on startup.

Connection strings are in `appsettings.json` pointing to an Aiven-hosted MySQL instance. Local development requires either access to that instance or a local MySQL override in `appsettings.Development.json`.

### Financial Models

`BudgetProposal` → `BudgetAllocation` → `Transaction` is the flow. Transactions reference an allocation; proposals go through Pending → Approved/Disapproved status.

### Event Model

`EventModel` has six child collection navigation properties (`ProgramScheduleItems`, `Guests`, `Personnel`, `Equipment`, `Transportation`, `Expenses`). Always load them explicitly — EF lazy loading is not configured. See `EventService.cs` for the correct Include pattern.

## Key Conventions

- MudBlazor is the component library — use `Mud*` components for all UI elements.
- Status fields use C# `enum` types (`ActiveStatus`, `BudgetStatus`, `TransactionType`, etc.) defined in the relevant model files.
- `CreatedAt`/`UpdatedAt` timestamps and `CreatedBy`/`RecordedBy` string fields are the audit trail — populate them on create/update operations.
- The `DashboardLayout.razor` (currently modified on `develop`) wraps all authenticated pages and owns the sidebar/nav state.
