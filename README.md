# Church Management Information System

## Project Description

The Church Management Information System is a web-based platform designed to improve and automate church operations such as financial management, membership administration, service scheduling, event coordination, and donation tracking.

The system solves problems related to manual record-keeping, inefficient financial monitoring, scheduling conflicts, and lack of centralized member management. It provides church leaders and members with a secure and organized platform for managing church activities efficiently and transparently.

### Main Features
- Financial Management
- Service & Appointment Management
- Event Management
- Membership Management
- Collection & Donation Management
- Audit Logging and Reporting

---

# Tech Stack

## Core Framework
- Blazor Web App

## UI Framework
- MudBlazor

## Backend API Layer
- ASP.NET Core

## Database
- MySQL

## ORM (Database Access)
- Entity Framework Core

## State Management
- Fluxor

## Hosting / Deployment
- Vercel

## Development Tools
- Visual Studio Community
- GitHub

---

# Installation Guide

## 1. Clone the Repository

```bash
git clone https://github.com/anjamesmanuel/CMIS
```

---

## 2. Open the Project

Open the solution file (`.sln`) using Visual Studio Community.

---

## 3. Configure the Database

- Install MySQL Server
- Create a database named:

```sql
church_management_db
```

- Update the connection string in:

```plaintext
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=church_management_db;user=root;password=yourpassword;"
}
```

---

## 4. Install Required Dependencies

Open the terminal in Visual Studio and run:

```bash
dotnet restore
```

---

## 5. Apply Entity Framework Core Migrations

```bash
dotnet ef database update
```

---

## 6. Run the Application

```bash
dotnet run
```

---

## 7. Open the Application in Browser

```plaintext
https://localhost:5001
```

---

# Contributors

| Name | Role | Assigned Module |
|------|------|------|
| James Manuel V. An | Full Stack Developer / Project Lead | Financial Management |
| Michael Harvey C. Saturius | Full Stack Developer | Collection & Donation |
| Roejon Kayne Paul P. Hernandez | Full Stack Developer | Event Management |
| Jethro Emmanuel F. Abunda | Full Stack Developer | Service & Appointment Management |
| Jhov Joshua E. Galvan | Full Stack Developer | Membership Management |

---

# Branching Strategy

## Main Branch
- Production-ready code only
- Protected branch
- No direct push allowed

## Develop Branch
- Main integration branch for ongoing development

## Feature Branches
Feature branches follow the naming convention:

```plaintext
feature/financial-management
feature/event-management
```

All feature branches must be merged into the `develop` branch through Pull Requests before deployment to `main`.

---

---

# License

This project is licensed under the MIT License.
