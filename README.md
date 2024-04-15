# Project Name

This project is an exemplary representation of a RESTful API developed using ASP.NET Core, highlighting the capabilities and features of the framework as of the latest iteration.

## Features

- **ASP.NET Core Framework**: Utilized the ASP.NET Core framework to establish a robust and scalable API.

- **C# 10 Exposure**: Explored the latest features and enhancements introduced in C# 10.

- **Restful API Design**: Leveraged out-of-the-box features provided by the ASP.NET Core project template to implement RESTful APIs.

- **Swashbuckle Swagger Integration**: Integrated Swagger for API documentation and testing, facilitating a better developer experience.

- **NuGet Dependencies**: Enriched the project by incorporating third-party libraries such as Serilog for logging and Entity Framework for database operations, managed via NuGet.

- **Database Seeding**: Implemented database seeding strategies for populating default data.

- **Scaffolding and Customization**: Utilized scaffolding tools to generate controllers and further customized them by introducing repository patterns.

- **Repository Pattern**: Enhanced data access layers with repositories to abstract data queries.

- **CRUD Operations**: Implemented CRUD functionalities with corresponding HTTP verbs: GET, POST, PUT, DELETE.

- **Data Transfer Objects (DTOs)**: Utilized DTOs to enforce data integrity and standards.

- **Security with Identity Core**: Integrated Identity Core for user management and JWT authentication to secure endpoints.

- **API Versioning**: Introduced API versioning, allowing for multiple versions of the same controller to coexist.

- **Caching Mechanisms**: Implemented caching strategies to optimize performance.

- **Exception Handling**: Created custom exceptions and middleware for graceful error handling.

- **Project Structure Refactoring**: Refined the architecture into three distinct projects separating data, core application logic, and API layers.

- **Logging and Monitoring**: Incorporated extensive logging to monitor API performance and behavior.

## Development Framework

The project is built on the ASP.NET Core framework, taking full advantage of its features to provide a solid foundation for building high-quality web APIs.

## Getting Started

To get a local copy up and running, follow these simple steps:

1. Clone the repository:git clone https://github.com/EmadERagheb/HotelListing.git
2. Navigate to the project directory:cd HotelListing
3. Restore NuGet packages:dotnet restore
4. Set up your database and connection string in the `appsettings.json`.
5. Apply the migrations to create the database schema:dotnet ef database update
6. Start the application:dotnet run

## Usage

Refer to the Swagger documentation at `http://localhost:yourport/swagger` to interact with the API and test the various endpoints.

## Contributions

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

Emad Eid Ragheb –  emaderagheb@gmail.com

Project Link: https://github.com/EmadERagheb/HotelListing



