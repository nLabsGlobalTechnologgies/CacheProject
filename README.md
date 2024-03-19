## EF Core In-Memory Repository

This repository showcases an example of using Entity Framework Core (EF Core) with an in-memory database to manage products in a web application. The repository provides CRUD (Create, Read, Update, Delete) operations for products and implements pagination for retrieving product data.

### Setup

1. **Clone the Repository:**
   Clone this repository to your local machine using the following command:
```bash
   git clone <repository-url>
   dotnet restore
   dotnet run
```
## Features

- **In-Memory Database:**
  The application uses EF Core's in-memory database provider for storing and managing product data. This is useful for testing and development purposes.

- **Swagger Integration:**
  Swagger UI is integrated into the application to provide a convenient interface for testing API endpoints.

- **Pagination:**
  Pagination functionality is implemented to retrieve products in paginated chunks, improving performance and user experience.

## Endpoints

- **GET /GetAllProducts**
  - Retrieves all products with pagination support.
  - **Parameters:**
    - `pageNumber`: The page number to retrieve.
    - `pageSize`: The number of products per page.
  - **Returns:**
    - Paginated list of products.

- **GET /SeedData**
  - Seeds the database with sample product data.
  - **No parameters required.**
  - **Returns:**
    - Success message indicating that the product seed data was created successfully.

## Usage

- **Retrieve Products:**
  Send a GET request to `/GetAllProducts` with appropriate pagination parameters (`pageNumber` and `pageSize`) to retrieve a paginated list of products.

- **Seed Data:**
  Send a GET request to `/SeedData` to populate the database with sample product data.

## Development

- **Environment:** Development
- **Dependencies:**
  - .NET 8.0
  - Entity Framework Core
  - Microsoft.AspNetCore
  - Microsoft.EntityFrameworkCore.InMemory
  - EntityFrameworkCorePagination.Nuget.Pagination

## Contributions

Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to open an issue or create a pull request.

## License

This project is licensed under the MIT License. Feel free to use and modify the code as per your requirements.
