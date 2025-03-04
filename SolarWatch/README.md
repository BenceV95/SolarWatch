# SolarWatch

SolarWatch is a .NET 9.0 web application that provides solar data and geocoding information. The application includes authentication and authorization features using JWT tokens.

## Features

- User registration and login
- Fetch geocoding data for a given location
- Fetch solar data for a given location and date
- Integration tests for authentication and data fetching

## Technologies Used

- .NET 9.0
- ASP.NET Core
- Entity Framework Core
- In-memory database for testing
- JWT Authentication
- FluentAssertions for testing
- xUnit for unit testing

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022


## Project Structure

- `SolarWatch/`: The main application project.
- `SolarWatchTests/`: The test project containing integration tests.

## Configuration

The application uses an in-memory database for testing purposes. You can configure the database and other settings in the `appsettings.json` file.
