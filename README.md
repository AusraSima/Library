# Library Management System

This project implements a basic library management system where authors and books can be managed through an API and a web interface.

## Table of Contents

- [Introduction](#introduction)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Setup and Installation](#setup-and-installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction

The project consists of several components:

- **DataAccess**: Contains entity classes representing authors and books, as well as repositories for interacting with the database.
- **LibraryAPI**: Implements the RESTful API for managing authors and books.
- **LibraryConsoleApp**: Provides a console application for interacting with the database directly.
- **LibraryWeb**: Contains the web interface for managing authors and books.

## Technologies Used

- C#
- ASP.NET Core
- Entity Framework Core
- Razor Pages
- Blazor
- MySQL

## Project Structure

- **DataAccess**: Contains entity classes and repositories.
- **LibraryAPI**: Implements the API controllers.
- **LibraryConsoleApp**: Console application for direct database interaction.
- **LibraryWeb**: Web interface for managing authors and books.
- 

## Setup and Installation

**Important:** The `.appsettings.json` file is not included in this repository due to security reasons, as it might contain sensitive configuration details.
This project provides an example configuration file named `appsettings.example.json` to help you set up your own `.appsettings.json` file.

1. Clone the repository.
2. Ensure you have MySQL installed and running.
3. Copy `appsettings.example.json` to `appsettings.json` and rename it to `.appsettings.json`.
4. Edit the values in the `.appsettings.json` file according to your configuration needs. Refer to the example values and comments for guidance.
5. Run the migrations to create the database schema.
6. Build and run the solution.



## Usage

- **API**: Access the API endpoints for managing authors and books.
- **Console App**: Interact with the database directly using the provided console application.
- **Web Interface**: Use the web interface to manage authors and books through a user-friendly UI.

## Contributing

Contributions are welcome! Please follow the standard GitHub workflow:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes and commit them (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Create a new Pull Request.

## License

This project is licensed under the [MIT License](LICENSE).
