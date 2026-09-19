# .NET Clean Architecture Template

A production-ready, flexible .NET backend template following Clean Architecture principles. Clone this repository to jumpstart your .NET projects with best practices built-in.

## 🏗️ Architecture Overview

This template implements Clean Architecture with the following layers:

```
┌─────────────────────────────────────────────┐
│                API Layer                    │  ← Controllers, Middleware, Configuration
├─────────────────────────────────────────────┤
│             Application Layer               │  ← Use Cases, Services, DTOs, Interfaces
├─────────────────────────────────────────────┤
│               Domain Layer                  │  ← Entities, Enums, Business Logic
├─────────────────────────────────────────────┤
│            Infrastructure Layer             │  ← Data Access, External Services, Repos
└─────────────────────────────────────────────┘
```

### 🎯 Key Features

- ✅ **Clean Architecture** - Proper dependency flow and separation of concerns
- ✅ **Repository Pattern** - Generic and specific repository implementations
- ✅ **Unit of Work** - Transaction management across repositories
- ✅ **AutoMapper** - Object-to-object mapping
- ✅ **Custom Exception Handling** - Centralized error handling middleware
- ✅ **Dependency Injection** - Built-in DI container configuration
- ✅ **Entity Framework Core** - Data access with migrations support
- ✅ **Unit Testing** - Test project with NUnit and Moq
- ✅ **Swagger/OpenAPI** - API documentation (ready to configure)
- ✅ **Localization** - Multi-language support infrastructure

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (or modify for your preferred database)
- Your favorite IDE (Visual Studio, VS Code, Rider)

### 1. Clone and Setup

```bash
# Clone the template
git clone https://github.com/your-org/dotnet-clean-architecture.git MyNewProject
cd MyNewProject

# Remove git history and initialize your own
rm -rf .git
git init
git add .
git commit -m "Initial commit from Clean Architecture template"

# Restore packages
dotnet restore
```

### 2. Customize for Your Project

1. **Rename the solution and projects** (optional):
   ```bash
   # Rename solution file
   mv CA.sln YourProject.sln
   
   # Update project references in .csproj files
   # Update namespaces throughout the codebase
   ```

2. **Update connection string** in `API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=YourDatabase;Trusted_Connection=true;"
     }
   }
   ```

3. **Customize entities** in `Domain/Entities/` for your business domain

4. **Update company information** in `Directory.Build.props`

### 3. Run the Application

```bash
# Run the API project
dotnet run --project API

# Or using watch mode for development
dotnet watch run --project API
```

The API will be available at `https://localhost:7000` (or check the console output).

## 📁 Project Structure

```
├── 📁 API/                     # Presentation Layer
│   ├── Controllers/            # API Controllers
│   ├── Filters/               # Action filters, middleware
│   ├── Program.cs             # Application entry point
│   └── appsettings.json       # Configuration
│
├── 📁 Application/            # Application Layer
│   ├── Abstraction/           # Interfaces and contracts
│   │   ├── Repository/        # Repository interfaces
│   │   ├── Services/          # Service interfaces
│   │   └── UnitOfWork/        # UoW interface
│   ├── Implementation/        # Service implementations
│   ├── Request/               # Request DTOs
│   ├── Response/              # Response DTOs
│   └── Mapping Profiles/      # AutoMapper profiles
│
├── 📁 Domain/                 # Domain Layer
│   ├── Entities/              # Domain entities
│   ├── Enums/                 # Domain enumerations
│   ├── Models/                # Domain models
│   └── Types/                 # Custom types and exceptions
│
├── 📁 Infrastructure/         # Infrastructure Layer
│   ├── DbContext/             # EF Core database context
│   ├── Implementation/        # Repository implementations
│   ├── Localization/          # Localization resources
│   ├── Middleware/            # Custom middleware
│   └── Dependency/            # DI configuration
│
└── 📁 Application.UnitTest/   # Test Project
    └── UserService_Tests.cs   # Example unit tests
```

## 🛠️ Customization Guide

### Adding New Entities

1. **Create entity** in `Domain/Entities/`:
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    // Add business logic methods here
}
```

2. **Add to DbContext** in `Infrastructure/DbContext/ApplicationDbContext.cs`:
```csharp
public virtual DbSet<Product> Products { get; set; }
```

3. **Create repository interface** in `Application/Abstraction/Repository/`:
```csharp
public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    // Add custom methods
}
```

4. **Implement repository** in `Infrastructure/Implementation/Repository/`:
```csharp
public class ProductRepository : IProductRepository
{
    // Implementation
}
```

5. **Register in DI** in `Infrastructure/Dependency/ServicesContainer.cs`:
```csharp
services.AddTransient<IProductRepository, ProductRepository>();
```

### Removing Existing Features

The template includes User and DoctorJobTitle as examples. To remove them:

1. Delete entity files from `Domain/Entities/`
2. Remove from `ApplicationDbContext`
3. Delete repository interfaces and implementations
4. Remove from DI registration
5. Delete related DTOs, services, and controllers

### Adding External Services

1. **Create interface** in `Application/Abstraction/Services/`
2. **Implement in Infrastructure** in `Infrastructure/Implementation/Services/`
3. **Register in DI** in `Infrastructure/Dependency/ServicesContainer.cs`

## 🔧 Development Tools

### Database Migrations

```bash
# Add a new migration
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API

# Update database
dotnet ef database update --project Infrastructure --startup-project API
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Code Analysis

The template includes .NET analyzers. Configure rules in `.editorconfig` or project files.

## 📚 Patterns Included

- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management
- **Dependency Injection** - IoC container
- **DTO Pattern** - Data transfer objects
- **Service Layer** - Business logic organization
- **Middleware Pattern** - Cross-cutting concerns
- **Result Pattern** - Consistent API responses
- **Exception Handling** - Centralized error management

## 🎨 Extending the Template

### Adding Authentication

Consider these options:
- **JWT Bearer Authentication**
- **ASP.NET Core Identity**
- **Auth0 / Azure AD B2C**
- **Custom authentication**

### Adding Features

Common additions:
- **CQRS with MediatR**
- **FluentValidation**
- **Caching (Redis/In-Memory)**
- **Background Jobs (Hangfire/Quartz)**
- **Message Queues (RabbitMQ/Azure Service Bus)**
- **Health Checks**
- **Rate Limiting**
- **API Versioning**

### Production Considerations

Before deploying:
- **Security**: Add authentication, authorization, input validation
- **Logging**: Configure structured logging (Serilog)
- **Monitoring**: Add health checks, metrics
- **Performance**: Implement caching, optimization
- **Configuration**: Environment-specific settings
- **Database**: Production database setup
- **Docker**: Container deployment

## 📖 Additional Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET Application Architecture Guides](https://docs.microsoft.com/en-us/dotnet/architecture/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This template is open source. See LICENSE file for details.

---

**Happy coding! 🚀**

> This template provides a solid foundation. Customize it according to your project needs and remove what you don't require.



