# Northwind Customer Microservice

A .NET 8 microservice providing a RESTful API to manage and retrieve customer data and order history from the Northwind database.

## Technical Stack

- **Framework:** .NET 8 Web API
- **ORM:** Entity Framework Core 8.0
- **Database:** Microsoft SQL Server (Northwind)
- **API Documentation:** Swagger / OpenAPI
- **Testing:** xUnit
- **Health Checks:** ASP.NET Core Health Checks Middleware

## Solution Structure

The solution follows a multi-project architecture with clear separation of concerns:

### NorthwindWs.Api (Web Project)
- **Role:** Entry point and host
- **Responsibilities:** 
  - API Controllers
  - Dependency Injection configuration
  - Middleware setup (Health Checks & Swagger)
  - HTTP pipeline configuration

### NorthwindWs.Services (Service Layer)
- **Role:** Business logic and data processing
- **Responsibilities:** 
  - Order value calculations
  - Product counts aggregation
  - Customer filtering logic
  - Data transformation and DTOs

### NorthwindWs.Data (Data Access Layer)
- **Role:** Database integration
- **Responsibilities:** 
  - EF Core DbContext
  - Entity models
  - Database configuration
  - Data retrieval operations

### NorthwindWs.Tests (Test Project)
- **Role:** Quality assurance
- **Responsibilities:** 
  - Unit tests for Service Layer
  - 100% logic coverage
  - In-memory database testing

## API Endpoints

### Customer Overview
- **GET** `/api/customers`
- Returns a list of all customers with their names and total order counts

### Customer Detail
- **GET** `/api/customers/{customerId}`
- Returns detailed customer information including:
  - Customer profile
  - Order history summary
  - Total value per order
  - Product count per order

### Health Check
- **GET** `/actuator`
- Returns system health status including database connectivity

### API Documentation
- **GET** `/swagger`
- Interactive Swagger UI for API exploration and testing

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server with Northwind database
- Visual Studio 2022 or later (optional)

### Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
	"NorthwindConnection": "Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Running the Application

1. Restore NuGet packages:
   ```
   dotnet restore
   ```

2. Build the solution:
   ```
   dotnet build
   ```

3. Run the API:
   ```
   dotnet run --project NorthwindWs.Api
   ```

4. Navigate to `https://localhost:{port}/swagger` to explore the API

### Running Tests

Execute all unit tests:
```
dotnet test
```

Run tests with coverage:
```
dotnet test /p:CollectCoverage=true
```

## Future Improvements

### 1. Traceability Middleware
Implement custom middleware to capture and propagate request tracing information:
- **Input-Request-Id:** Unique identifier for each incoming request
- **Input-Timestamp:** Request arrival timestamp
- **Purpose:** Enable end-to-end request tracking across microservices
- **Implementation:** Custom ASP.NET Core middleware component

### 2. Centralized Configuration Server
Migrate from local `appsettings.json` to a centralized configuration management system:
- **Format:** YAML-based configuration files
- **Storage:** External configuration server (e.g., Azure App Configuration, Spring Cloud Config)
- **Benefits:** 
  - Centralized configuration management
  - Dynamic configuration updates without redeployment
  - Configuration versioning and audit trails

### 3. Environment-Aware Configuration Loading
Implement dynamic configuration fetching based on the active environment:
- **Environments:** Development, Staging, Production
- **Mechanism:** Runtime detection of active environment
- **Behavior:** Automatic loading of environment-specific configurations
- **Implementation:** 
  - Use `ASPNETCORE_ENVIRONMENT` variable
  - Fetch configurations from centralized server based on environment
  - Support for configuration overrides and fallbacks

## Project Status

✅ Multi-project solution architecture  
✅ EF Core data layer with Northwind entities  
✅ Business logic in Services layer  
✅ RESTful API controllers  
✅ Swagger documentation at `/swagger`  
✅ Health checks at `/actuator`  
✅ Comprehensive unit tests with 100% coverage  

## License

This is a sample project for educational purposes.
