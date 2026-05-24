# Product Management Backend

## Introduction
Product Management Web API is an ASP.NET Core API designed to serve the frontend application, handling JWT authentication, validation, and database operations for products and categories.

## API Endpoints
| HTTP Verbs | Endpoints | Action |
| --- | --- | --- |
| POST | `/api/Auth/login` | Login with username and password to receive a JWT Token. |
| GET | `/api/Category` | Retrieve all categories. |
| POST | `/api/Category` | Create a new category. |
| GET | `/api/Category/{id}` | Retrieve details of a specific category. |
| PUT | `/api/Category/{id}` | Update an existing category. |
| DELETE | `/api/Category/{id}` | Delete a category. |
| GET | `/api/Product` | Retrieve products (supports pagination `PageNumber`/`PageSize`, sorting `OrderBy`/`OrderDirection`, and filtering by `Search` and `Categories`). |
| POST | `/api/Product` | Create a new product. |
| GET | `/api/Product/{id}` | Retrieve details of a specific product. |
| PUT | `/api/Product/{id}` | Update an existing product. |
| DELETE | `/api/Product/{id}` | Delete a product. |

## Management Web API Features
* JWT Bearer Authentication ensures all Product and Category endpoints are secure.
* Implements basic Layered Architecture (Controller, Service, Repository) and separates DTOs from Entities.
* Returns appropriate HTTP status codes (200, 201, 400, 401, 404, etc.) for all operations.
* Server-side validation handles required fields and logic (Category Name is required, Product Name is required, Price > 0, Stock >= 0).

## Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)
* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or VS Code
* [dotnet ef tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (configure ConnectionString in `appsettings.json`)
* [Docker](https://www.docker.com/) (Optional)

## Installation Guide
* Clone this repository to your local machine.
* Open the solution file (.sln) or project folder using Visual Studio or VS Code.
* Run `dotnet restore` to install NuGet packages and other dependencies.
* Configure your database connection string in `appsettings.json`.
* Run `dotnet ef database update` to apply Entity Framework Core Code-First migrations and build the database schema.

## Usage
* Run `dotnet run` to start the application.
* The API will start on `http://localhost:5072/`.
* Copy openapi JSON format file from `http://localhost:5072/openapi/v1.json` and Connect to the API using Postman, Swagger UI, or the Angular Frontend.
* **Mock User:** The system includes seed scripts that run automatically to generate initial Mock Users.

## Docker (Optional)
* Run `docker build -t product-management-backend .` to build image
* Run this below command to start container:
```
  docker run -p 8081:8080 -d \
  -e 'ConnectionStrings__DefaultConnection=[YOUR_CONNECTION_STRING]' \
  -e 'Cors__AllowedOrigins=[YOUR_FRONTEND_URL]' \
  --name product-management-backend product-management-backend
```
* The API will start on `http://localhost:8081/`.

## Testing
* Run `dotnet test` to execute all unit tests (xUnit) for the backend service layer.

## Technologies Used
* [C# 14 / .NET 10](https://learn.microsoft.com/en-us/dotnet/csharp/)
* [ASP.NET Core Web API](https://dotnet.microsoft.com/en-us/apps/aspnet/apis)
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
* [xUnit](https://xunit.net/)
* [SQL Server](https://www.microsoft.com/en-us/sql-server)
* [Docker](https://www.docker.com/)

## Authors
* [AlabicaCoff](https://github.com/AlabicaCoff)
