# Food Delivery Order Management System

## Project Overview

This project is a focused internal food delivery order management application built with:

- ASP.NET Core Web API on .NET 8
- Entity Framework Core InMemory database
- Angular frontend with standalone components

The solution supports only the required internal-order workflow:

- Add Order
- View Orders
- Search Orders
- Update Status
- Delete Order
- Dashboard Summary

## Architecture

The application follows a clean layered architecture:

- `FoodDelivery.Api` handles the backend API, business logic, EF Core data access, middleware, and Swagger documentation.
- `food-delivery-ui` contains the Angular UI, components, models, services, and routing configuration.

### Backend Layers

- Controllers
- Services
- Repository Pattern
- Models / DTOs
- Data Context
- Middleware
- Extensions

### Frontend Layers

- Components
- Models
- Services
- Environments
- Shared UI helpers

## Technology Stack

### Backend

- ASP.NET Core 8 Web API
- Entity Framework Core InMemory
- Swagger / Swashbuckle
- CORS configuration for Angular
- Repository Pattern + Service Layer

### Frontend

- Angular 18
- TypeScript
- Bootstrap 5
- Reactive Forms
- Angular HttpClient
- Standalone Components

## Setup Instructions

### 1. Backend

From the root folder:

```bash
dotnet restore
cd FoodDelivery.Api
dotnet run
```

The API is expected to run on:

```text
https://localhost:5001
```

Swagger is available at:

```text
https://localhost:5001/swagger
```

### 2. Frontend

```bash
cd food-delivery-ui
npm install
npm start
```

The Angular app is expected to run on:

```text
http://localhost:4200
```

## API Endpoints

### Orders

- `GET /api/orders`
- `GET /api/orders/{id}`
- `GET /api/orders/search?searchTerm=value`
- `POST /api/orders`
- `PUT /api/orders/{id}`
- `PATCH /api/orders/{id}/status`
- `DELETE /api/orders/{id}`
- `GET /api/orders/summary`

## Angular Setup

The Angular frontend is configured to call:

```text
https://localhost:5001/api
```

The implementation uses:

- `OrderService` for typed HTTP calls
- routing for dashboard and order workflows
- reactive forms for order creation and update
- Bootstrap-based layouts and alerts

## Database

The backend uses Entity Framework Core InMemory Database with seeded sample orders.

Database characteristics:

- lightweight local persistence for development
- no external database dependency
- seeded sample records for dashboard and list demonstration

## Features

- Create new orders
- View all orders
- Search orders by customer value
- Update order status
- Delete orders
- View dashboard summary metrics and revenue

## Copilot Usage

GitHub Copilot was used to support the development workflow across:

- requirement analysis
- architecture consolidation
- backend scaffolding and DI setup
- API endpoint implementation
- Angular model and service generation
- component implementation
- integration fixes
- documentation generation

## Screenshots Placeholder

Add screenshots here for:

- Dashboard view
- Order list view
- Create/Edit order form
- Swagger UI

## Folder Structure

```text
FoodDelivery.OrderManagementSystem/
├── FoodDelivery.Api/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   ├── Repositories/
│   ├── Interfaces/
│   ├── Services/
│   ├── Middleware/
│   └── Extensions/
├── food-delivery-ui/
│   ├── src/app/components/
│   ├── src/app/models/
│   ├── src/app/services/
│   ├── src/app/enums/
│   ├── src/app/interfaces/
│   ├── src/app/shared/
│   └── src/environments/
└── docs/
```
