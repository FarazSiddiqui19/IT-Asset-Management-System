# IT Asset Management System

A backend REST API for managing IT assets within an organization. The system allows administrators to track assets, assign them to employees, monitor lifecycle status, and maintain an auditable inventory.

This project focuses on **clean backend architecture, role-based authorization, and asset lifecycle management** using modern backend development practices.

---

# Features

### Asset Management

* Create, update, and delete IT assets
* Track asset lifecycle states:

  * Available
  * Assigned
  * Under Maintenance
  * Retired
* Maintain asset metadata such as model, category, and serial number

### Asset Assignment

* Assign assets to employees
* Track assignment history
* Prevent invalid status transitions

### User Management

* User registration and authentication
* Role-based access control (Admin / Employee)
* Employees can view assets assigned to them

### Security

* JWT based authentication
* Role-based authorization
* Input validation and error handling

### API Architecture

* Layered architecture
* Repository pattern
* Service layer validations
* DTO based request/response contracts

---

# Tech Stack

| Layer             | Technology            |
| ----------------- | --------------------- |
| Backend           | ASP.NET Core Web API  |
| Language          | C#                    |
| ORM               | Entity Framework Core |
| Database          | Postgres            |
| Authentication    | JWT                   |
| API Documentation | Swagger / OpenAPI     |

---

# Architecture

The project follows a **clean layered architecture**:

```
Controller Layer
        ↓
Service Layer
        ↓
Repository Layer
        ↓
Database (Entity Framework)
```

### Controller

Handles HTTP requests and responses.

### Service Layer

Contains business logic, validations, and domain rules.

### Repository Layer

Handles database operations and data access.

### Domain Models

Entities representing the system data.

---


# API Endpoints

## Authentication

| Method | Endpoint             | Description                            |
| ------ | -------------------- | -------------------------------------- |
| POST   | `/api/User/login`    | Authenticate user and return JWT token |
| POST   | `/api/User/register` | Register a new user                    |

---

# Users

| Method | Endpoint         | Description         |
| ------ | ---------------- | ------------------- |
| GET    | `/api/User/{id}` | Get user by ID      |
| PUT    | `/api/User/{id}` | Update user details |
| DELETE | `/api/User/{id}` | Delete a user       |

---

# Assets

| Method | Endpoint          | Description                                   |
| ------ | ----------------- | --------------------------------------------- |
| POST   | `/api/Asset/List` | Retrieve filtered or paginated list of assets |
| GET    | `/api/Asset/{id}` | Get asset details by ID                       |
| POST   | `/api/Asset`      | Create a new asset                            |
| PUT    | `/api/Asset/{id}` | Update asset details                          |
| DELETE | `/api/Asset/{id}` | Delete an asset                               |

---

# Assignments

| Method | Endpoint               | Description                        |
| ------ | ---------------------- | ---------------------------------- |
| POST   | `/api/Assignment/List` | Retrieve list of asset assignments |
| GET    | `/api/Assignment/{id}` | Get assignment details             |
| POST   | `/api/Assignment`      | Assign asset to a user             |
| PUT    | `/api/Assignment/{id}` | Update assignment                  |
| DELETE | `/api/Assignment/{id}` | Remove assignment                  |

---

# Assignment Requests

| Method | Endpoint                                    | Description                    |
| ------ | ------------------------------------------- | ------------------------------ |
| POST   | `/api/AssignmentRequest/List`               | Retrieve assignment requests   |
| GET    | `/api/AssignmentRequest/{id}`               | Get assignment request details |
| POST   | `/api/AssignmentRequest`                    | Create assignment request      |
| PATCH  | `/api/AssignmentRequest/UpdateContent/{id}` | Update request content         |
| PATCH  | `/api/AssignmentRequest/UpdateStatus/{id}`  | Approve or reject request      |
| DELETE | `/api/AssignmentRequest/{id}`               | Delete assignment request      |

---

# Categories

| Method | Endpoint             | Description         |
| ------ | -------------------- | ------------------- |
| POST   | `/api/Category/List` | Retrieve categories |
| GET    | `/api/Category/{id}` | Get category by ID  |
| POST   | `/api/Category`      | Create category     |
| PUT    | `/api/Category/{id}` | Update category     |
| DELETE | `/api/Category/{id}` | Delete category     |

---

# Products

| Method | Endpoint            | Description           |
| ------ | ------------------- | --------------------- |
| POST   | `/api/Product/List` | Retrieve product list |
| GET    | `/api/Product/{id}` | Get product details   |
| POST   | `/api/Product`      | Create product        |
| PUT    | `/api/Product/{id}` | Update product        |
| DELETE | `/api/Product/{id}` | Delete product        |

---

# Tickets

| Method | Endpoint                         | Description              |
| ------ | -------------------------------- | ------------------------ |
| POST   | `/api/Ticket/List`               | Retrieve support tickets |
| GET    | `/api/Ticket/{id}`               | Get ticket details       |
| POST   | `/api/Ticket`                    | Create ticket            |
| PATCH  | `/api/Ticket/UpdateStatus/{id}`  | Update ticket status     |
| PATCH  | `/api/Ticket/UpdateContent/{id}` | Update ticket content    |
| DELETE | `/api/Ticket/{id}`               | Delete ticket            |

---

# Comments

| Method | Endpoint             | Description       |
| ------ | -------------------- | ----------------- |
| POST   | `/api/comments/List` | Retrieve comments |
| GET    | `/api/comments/{id}` | Get comment by ID |
| POST   | `/api/comments`      | Create comment    |
| PUT    | `/api/comments/{id}` | Update comment    |
| DELETE | `/api/comments/{id}` | Delete comment    |

---

## Notes

* All endpoints require **JWT authentication** except login and register.
* Role-based authorization is enforced for sensitive operations.
* Filtering and pagination for lists are handled via the **List endpoints**.

---




# Installation

## 1. Clone the Repository

```bash
git clone https://github.com/FarazSiddiqui19/IT-Asset-Management-System.git
cd IT-Asset-Management-System
```

---

## 2. Configure Database

Update the connection string inside:

```
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ITAssets;Trusted_Connection=True;"
}
```

---

## 3. Apply Migrations

```bash
dotnet ef database update
```

---

## 4. Run the Application

```bash
dotnet run
```

Swagger will be available at:

```
https://localhost:5001/swagger
```

---

# Example Workflow

1. Admin logs in and receives a JWT token.
2. Admin creates IT assets.
3. Admin assigns assets to employees.
4. Employees can view assets assigned to them.
5. Asset lifecycle status can be updated as needed.

---

# Validation Rules

Examples of enforced business rules:

* Assets cannot be assigned if they are **Under Maintenance**
* Invalid status transitions are blocked
* Only admins can create or assign assets

---

# Future Improvements
* Notification system
* File attachments for assets

---




