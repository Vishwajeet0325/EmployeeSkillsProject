# Employee & Skills Management Application

A web application built using ASP.NET Core MVC for managing employees and their skills.

## Features
- Create Employee
- View Employee List
- Edit Employee
- Delete Employee with confirmation
- Multiple skills for each employee
- Many-to-many Employee and Skill relationship
- Server-side validation
- Date of Birth validation
- 10-digit Indian mobile number validation
- Pagination
- Column-wise search
- Async database operations
- Dependency Injection

## Technology Stack
- .NET 8
- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server LocalDB
- EF Core Code-First Approach
- EF Core Migrations
- Bootstrap

## Database
This project uses SQL Server LocalDB.

SQL Server LocalDB was selected because it is easy to configure and works well with Visual Studio and Entity Framework Core for local development.

## Prerequisites

Before running the application, install:

- Visual Studio 2026
- .NET 8 SDK
- SQL Server LocalDB
- Git

Verify the .NET SDK:

```bash
dotnet --version
```

## Clone the Repository

Clone the repository:

```bash
git clone https://github.com/YOUR-GITHUB-USERNAME/EmployeeSkillsManagement.git
```

Move to the project directory:

```bash
cd EmployeeSkillsManagement
```

## Database Configuration

Open `appsettings.json` and configure the connection string.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeSkillsManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```


## Restore Packages

Run:

```bash
dotnet restore
```

## Apply Database Migrations

project uses Entity Framework Core Code-First migrations.

Run:

```bash
dotnet ef database update
```

Or in Visual Studio:

1. Go to **Tools**
2. Select **NuGet Package Manager**
3. Open **Package Manager Console**
4. Run:

```powershell
Update-Database
```

This will create the database and apply the migrations.

The predefined skills include:

- C#
- ASP.NET
- C++
- Java
- JavaScript
- SQL

## Build the Application

Run:

```bash
dotnet build
```

Or in Visual Studio:

**Build → Build Solution**

## Run the Application

Run:

```bash
dotnet run
```

Open the URL displayed by Visual Studio or the terminal.

Example:

```text
https://localhost:xxxx
```

The port number may be different on each machine.

## Employee-Skill Relationship

The application uses a many-to-many relationship between Employee and Skill.

An employee can have multiple skills, and a skill can belong to multiple employees.

The relationship is implemented using an `EmployeeSkill` join entity containing:

- EmployeeId
- SkillId

## Validation

### Employee Name

- First Name is required.
- Last Name is required.

### Date of Birth

- Date of Birth is required.
- Date must be after 01-01-1900.
- Date must be in the past.

### Phone Number

- Phone number is required.
- Phone number must contain exactly 10 digits.

## Pagination

The employee list supports pagination using Entity Framework Core `Skip()` and `Take()`.

The current implementation displays 5 employees per page.

## Search

Column-wise search is available for:

- First Name
- Last Name
- Date of Birth
- Phone Number
- Skills

Search filters are preserved while navigating between pages.

## Project Structure

```text
EmployeeSkillsManagement
│
├── Controllers
│   └── EmployeesController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Migrations
│
├── Models
│   ├── Employee.cs
│   ├── Skill.cs
│   └── EmployeeSkill.cs
│
├── Validation
│   └── PastDateAttribute.cs
│
├── ViewModels
│   ├── EmployeeViewModel.cs
│   └── EmployeeListViewModel.cs
│
├── Views
│   └── Employees
│
├── wwwroot
├── Program.cs
├── appsettings.json
├── EmployeeSkillsManagement.csproj
└── README.md
```

## Key Design Decisions
- ASP.NET Core MVC is used to separate Models, Views, and Controllers.
- Entity Framework Core Code-First approach is used for database design.
- EF Core migrations are included for database creation and updates.
- A join entity is used to model the Employee-Skill many-to-many relationship.
- ViewModels are used for Create/Edit and Employee List operations.
- Async EF Core methods are used for database operations.
- Dependency Injection is used to inject the DbContext.

## Future Improvements

With additional time, the following could be added:

- Unit tests
- Integration tests
- Column sorting
- Authentication and authorization
- Logging and global exception handling
- REST API endpoints
- Improved responsive UI
