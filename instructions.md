# Backend Specification: Northwind Customer Microservice

## 1. Project Overview
[cite_start]A .NET 8 microservice providing a RESTful API to manage and retrieve customer data and order history from the Northwind database[cite: 8, 12].

---

## 2. Technical Stack
- **Framework:** .NET 8 Web API.
- **ORM:** Entity Framework Core (version compatible with .NET 8).
- [cite_start]**Database:** Microsoft Northwind (SQL Server)[cite: 8].
- **API Documentation:** Swagger / OpenAPI.
  - **Endpoint:** `https://localhost:{port}/swagger`
- **Monitoring:** Integrated Health Checks Middleware.
  - **Endpoint:** `https://localhost:{port}/actuator`
- **Testing:** xUnit.

---

## 3. Solution Structure (Multi-Project Architecture)
[cite_start]The solution must be organized into four distinct projects to ensure separation of concerns[cite: 27]:

### 3.1 NorthwindWs.Api (Web Project)
- [cite_start]**Role:** Entry point and host[cite: 12].
- [cite_start]**Responsibilities:** Controllers, Dependency Injection, Middleware (Health Checks & Swagger)[cite: 25].

### 3.2 NorthwindWs.Services (Service Layer)
- [cite_start]**Role:** Business logic and data processing[cite: 40].
- [cite_start]**Responsibilities:** Order value calculations, product counts, and customer filtering logic[cite: 17, 21, 22].

### 3.3 NorthwindWs.Data (Data Access Layer)
- [cite_start]**Role:** Database integration[cite: 26].
- **Responsibilities:** EF Core DbContext, Entities, and Data Retrieval.

### 3.4 NorthwindWs.Tests (Test Project)
- [cite_start]**Role:** Quality assurance[cite: 30, 31].
- **Responsibilities:** Isolated Unit Tests for the Service Layer ensuring 100% logic coverage.

---

## 4. Functional Requirements
- [cite_start]**Customer Overview:** List names and total order counts[cite: 15, 16].
- [cite_start]**Customer Detail:** Summary of order history including total value and product count per order[cite: 19, 21, 22].
- **Health Check:** System status exposed at `/actuator`.

---

## 5. README Requirements (Future Improvements)
[cite_start]The `README.md` must describe the following structured improvements[cite: 33, 36]:
1. **Traceability Middleware:** Custom middleware for `Input-Request-Id` and `Input-Timestamp`.
2. **Centralized Configuration Server:** Environment-specific `.yaml` files.
3. **Environment-Aware Loading:** Dynamic configuration fetching based on the active environment.

---

## 6. AI Agent Execution Steps
1. Create a solution with four projects: `Api`, `Services`, `Data`, and `Tests`.
2. Configure **Swagger (`/swagger`)** and **Health Checks (`/actuator`)** in the Api project.
3. Scaffold EF Core in the Data project.
4. Implement core business logic in the Services project.
5. Achieve **100% unit test coverage** within the Tests project.