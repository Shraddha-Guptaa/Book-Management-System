Project Description – Book Management System
Overview

The Book Management System is a web-based application developed using ASP.NET Core MVC. The project is designed to manage book-related information efficiently and follows the Repository Pattern to maintain a clean separation between data access logic and application logic. This project was developed as part of hands-on learning to understand real-world application architecture using modern .NET technologies.

Objectives

To develop a structured web application using ASP.NET Core MVC

To implement the Repository Pattern for clean and maintainable code

To perform database operations using Entity Framework Core

To manage books through secure and organized CRUD operations

To understand separation of concerns in enterprise-level applications

Key Features

Add new books with proper validation

View list of available books

Update existing book details

Delete books from the system

Clean and user-friendly interface

Secure access using cookie-based authentication and session management

Architecture & Design

The application follows the MVC architecture combined with the Repository Pattern:

Controller Layer: Handles user requests and responses

Repository Layer: Manages all database-related operations

Model Layer: Represents book entities and database tables

Database Layer: SQL Server accessed through Entity Framework Core

The Repository Pattern ensures that controllers do not directly interact with the database, improving maintainability, testability, and scalability.

Technologies Used

Frontend: HTML, CSS, Bootstrap

Backend: ASP.NET Core MVC

ORM: Entity Framework Core

Database: SQL Server

Design Pattern: Repository Pattern

Authentication: Cookies and Sessions

Development Tool: Visual Studio

Conclusion

The Book Management System demonstrates the practical implementation of ASP.NET Core MVC, Entity Framework Core, and the Repository Pattern. The project focuses on clean architecture and best practices by separating data access from business logic, making the application easier to maintain and extend in the future.
