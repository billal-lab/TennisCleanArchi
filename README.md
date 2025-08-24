# TennisCleanArchi

## Introduction

TennisCleanArchi is an api designed to manage tennis-related data, such as players, countries, and statistics. It leverages Clean Architecture principles to separate concerns and ensure optimal maintainability.

## Detailed Features

### Player Management
- **Add Players**: Create new players with detailed information such as name, country, gender, etc.
- **Paginated List**: Retrieve a paginated list of players, sorted by rank or other criteria.
- **Retrieve by ID**: Access the details of a specific player using their unique identifier.

### Country Management
- **List of Countries**: Retrieve a paginated list of available countries, including their codes and flags.

### Statistics
- **Global Statistics**: Obtain aggregated statistics about players and countries.

### Validation
- **Validated Requests**: All requests are validated to ensure data integrity before processing.

### Caching
- **Caching Service**: Reduce response times with a configurable caching system.

### Security
- **API Key**: Protect API access with API key authentication.

### Rate Limiting
- **Request Limiting**: Prevents API abuse by limiting requests to **100 per minute per IP address**. Exceeding this limit returns a `429 Too Many Requests` response.

### Documentation
- **Swagger**: Easily explore and test endpoints with interactive documentation.

## Technologies used

- **ASP.NET Core**: Main framework for building the API.
- **Entity Framework Core**: Data persistence management with an InMemory database for testing.
- **Swagger/OpenAPI**: Interactive documentation.
- **GitHub Actions**: Continuous integration and deployment.
- **Azure Web App**: Application hosting.
- **MediatR**: Implements the mediator pattern for handling requests.
- **FluentValidation**: Provides a fluent interface for validating objects.
- **Mapster**: Simplifies object mapping and projections.
- **xUnit**: A testing framework for writing unit tests.

---

Feel free ask questions if you need help!

## Project Structure

- **TennisCleanArchi.Application**  
  Business logic, Handlers, Validators, DTOs for players, countries, and statistics.
- **TennisCleanArchi.Domain**  
  Main entities: `Player`, `Country`, `PlayerStats`.
- **TennisCleanArchi.Infrastructure**  
  Persistence (EF Core InMemory), cross-cutting concerns (caching, exceptions, rate limiting, auth).
- **TennisCleanArchi.Host**  
  API entry point, configuration, REST controllers (`PlayersController`, `CountriesController`, `StatsController`).
- **TennisCleanArchi.Shared**  
  Shared types (`Sex`...).
- **TennisCleanArchi.Application.Tests**  
  Unit tests for Handlers, Validators.

## Main Features

- **Player Management**: Adding, paginated listing, retrieval by ID.
- **Country Management**: List of countries.
- **Statistics**: Global stats retrieval.
- **Validation**: Validators for requests.
- **Caching**: Injectable caching service.
- **Security**: API key authentication.
- **Rate Limiting**: Request number limitation.
- **Documentation**: Integrated Swagger.

## Quick Start

1. **Clone the repo**
2. **Restore packages**
   ```sh
   dotnet restore
   ```
3. **Configure API Key**
   
   Create a `secrets.json` file for local development:
   ```sh
   dotnet user-secrets init --project TennisCleanArchi.Host
   dotnet user-secrets set "ApiKey:Value" "your-api-key-value" --project TennisCleanArchi.Host
   ```
   
   This sets up the API key that will be required for authentication.
   
4. **Launch the API**
   ```sh
   dotnet run --project TennisCleanArchi.Host
   ```
   
   The application will automatically seed the database with sample players and countries on startup.
   
5. **Access Swagger**
   - http://localhost:5045/swagger

## Data Seeding

The application uses an in-memory database that is seeded with sample data on startup.

The seeding process is handled by the `ApplicationSeeder` class in the Infrastructure layer and is automatically triggered when the application starts. This ensures you always have sample data to work with when developing or testing the API.

## Tests

Run all tests:
```sh
dotnet test
```

## Deployment

### CI/CD Pipelines

The project uses two GitHub Actions workflows:

1. **Development Pipeline** (`.github/workflows/dev-api-build-deploy.yml`)
   - Triggered on changes to `develop` branch
   - Builds and tests the application

2. **Production Pipeline** (`.github/workflows/prod-api-build-deploy.yml`)
   - Triggered on changes to the `master` branch
   - Builds and deploys to production environment

### Environment

- **Hosting**: Azure Web App (Free tier plan with 60 minutes of compute per day)
- **Region**: France Central
- **Scaling**: The application may be in a sleep state and require a brief warm-up period on first access

### Live API

- Access the live API and Swagger documentation: [Tennis API](https://tennis-api-c4azfefcdkdrfye0.canadacentral-01.azurewebsites.net/swagger)

## How to Use the API

The TennisCleanArchi API provides a RESTful interface to access tennis-related data. All endpoints require an API key that must be passed in the header of each request.

### Authentication

Add your API key to the request header:
```
X-Api-Key: your-api-key-here
```