Prompt 1 — Requirement Analysis
You are a Senior .NET Full Stack Architect.

Analyze the attached Food Delivery Order Management System case study.

Do not generate code.

Understand every functional and non-functional requirement.

Create a detailed implementation plan.

Provide:

1. Project architecture
2. Folder structure
3. Backend design
4. Frontend design
5. Entity model
6. API endpoints
7. Validation rules
8. Angular pages
9. Development sequence
10. Potential risks

The solution must exactly satisfy the case study without adding unnecessary features.

Prompt 2 - Correct the Architecture
The previous implementation plan includes features that are outside the scope of the case study.

Review the case study again and strictly follow only the given requirements.

Remove:
- JWT Authentication
- Login/Register
- Customer module
- Restaurant module
- Delivery Agent module
- Payment
- Role-based authorization
- Menu management
- Complex workflows

The application is only an Internal Food Delivery Order Management System.

The solution must contain only:

Backend:
- ASP.NET Core Web API (.NET 8)
- InMemory Database
- Entity Framework Core

Frontend:
- Angular

Single entity:
Order

Required features only:

• Add Order
• View Orders
• Search Orders
• Update Status
• Delete Order
• Dashboard Summary

Use the Repository Pattern and clean layered architecture.

Do not generate code yet.

Update the architecture and folder structure accordingly.

Prompt 3 — Generate Solution Structure
Now generate the complete solution structure.

Create two projects only:

FoodDelivery.Api
food-delivery-ui

For FoodDelivery.Api generate the folder structure:

Controllers
Models
DTOs
Data
Repositories
Interfaces
Services
Mappings
Validators
Middleware
Extensions

For Angular generate:

components
models
services
environments
shared

Also generate the required documentation folders:

.github
docs

Do not generate implementation code.

Only generate the complete folder hierarchy and explain the purpose of each folder.


Prompt 4 — Generate Backend Project
Now let's implement the backend.

Generate an ASP.NET Core 8 Web API project named FoodDelivery.Api.

Requirements:

• Entity Framework Core InMemory Database
• Swagger enabled
• CORS enabled for Angular localhost:4200
• Repository Pattern
• Dependency Injection
• Global Exception Middleware
• Clean architecture

Generate only:

Program.cs

Project file

Package references

Dependency Injection

Folder creation

Do not generate Models or Controllers yet.

The project must compile successfully.

Prompt 5  — Generate Order Entity and Generate DbContext

Generate the Order model.

Properties:

Id
CustomerName
CustomerPhone
FoodItem
Quantity
Price
DeliveryAddress
Status
OrderDate

Requirements:

Use DataAnnotations.

Validation:

CustomerName Required

CustomerPhone Required

FoodItem Required

DeliveryAddress Required

Quantity > 0

Price > 0

Status must be an Enum with values:

Placed
Preparing
OutForDelivery
Delivered
Cancelled

Generate production-quality code.

Generate FoodDeliveryDbContext.

Requirements:

Use Entity Framework Core InMemory Database.

Create DbSet<Order>.

Seed two sample orders.

Configure OnModelCreating.

The DbContext must be compatible with the previously generated Order model.

Explain every configuration.

Prompt 6 — Generate Repository Layer and Generate Service Layer

Generate the Repository Pattern.

Create:

IOrderRepository

OrderRepository

Methods:

GetAllAsync

GetByIdAsync

SearchAsync

CreateAsync

UpdateAsync

DeleteAsync

UpdateStatusAsync

GetSummaryAsync

Use async/await.

Return meaningful values.

Handle null checks.

Use Entity Framework Core best practices.

Generate the Service Layer.

Business Rules:

Customer Name is mandatory.

Phone is mandatory.

Food Item is mandatory.

Quantity must be greater than zero.

Price must be greater than zero.

Status must be one of:

Placed
Preparing
OutForDelivery
Delivered
Cancelled

The Service layer should contain all business logic.

Repository should only access the database.

Generate interfaces and implementations.

Prompt 7 — Generate Summary DTO and Generate OrdersController

Generate DashboardSummaryDto.

Properties:

TotalOrders

PlacedOrders

PreparingOrders

OutForDeliveryOrders

DeliveredOrders

CancelledOrders

TotalRevenue

Also generate the LINQ logic required to calculate these values efficiently.

Generate OrdersController.

Implement only the following endpoints exactly as specified in the case study.

GET /api/orders

GET /api/orders/{id}

GET /api/orders/search

POST /api/orders

PUT /api/orders/{id}

PATCH /api/orders/{id}/status

DELETE /api/orders/{id}

GET /api/orders/summary

Requirements:

REST API

Use IActionResult

Return proper HTTP status codes

Use dependency injection

Validate input

Return meaningful error messages

Do not generate unnecessary endpoints.

Prompt 8 — Build and Fix Backend

Review the entire backend project.

Verify:

Project builds successfully.

No compile errors.

No dependency injection issues.

No namespace issues.

No nullable warnings causing runtime problems.

Swagger works.

All endpoints execute successfully.

If any issue exists, fix only that issue.

Do not rewrite working code.

Prompt 9 — CORS, Exception Handling and Swagger

Configure CORS for Angular.

Allow:

http://localhost:4200

Do not use AllowAnyOrigin.

Explain why.

Update Program.cs accordingly.

Implement Global Exception Handling Middleware.

Return consistent JSON responses.

Include:

StatusCode

Message

Timestamp

Path

Handle:

ValidationException

NotFoundException

Unhandled Exception

Register middleware in Program.cs.

Configure Swagger.

Enable:

XML Comments

Endpoint descriptions

Request examples

Response examples

Swagger UI

Ensure all endpoints are visible.

Prompt 10 — Angular Architecture Model and Service

Generate Angular architecture.

Include:

Models

Services

Components

Interfaces

Shared folder

Environment configuration

Routing

Reactive Forms

Use standalone components.

Do not generate code yet.

Generate the Angular Order model.

Match exactly with backend DTO.

Use TypeScript interface.

Generate OrderStatus enum.

Generate Angular HttpClient service.

Methods:

getOrders()

getOrderById()

createOrder()

updateOrder()

updateStatus()

deleteOrder()

searchOrders()

getSummary()

Use Observable.

Handle HTTP errors.

Return strongly typed responses.

Prompt 11 — Dashboard Component, Order List Component, Order Form

Generate Dashboard component.

Display cards:

Total Orders

Placed

Preparing

Out For Delivery

Delivered

Cancelled

Revenue

Load data from API.

Use Bootstrap cards.

Generate Order List component.

Features:

Bootstrap table

Pagination

Sorting

Search by Customer Name

Filter by Status

Update Status dropdown

Delete button

Edit button

Responsive layout

Generate Reactive Form.

Fields:

Customer Name

Phone

Food Item

Quantity

Price

Address

Status

Date

Include:

Validators

Error Messages

Submit

Reset

Bootstrap styling

Prompt 12 — API Integration

Integrate Angular with ASP.NET Core API.

Configure environment.ts.

Base URL:

https://localhost:5001/api

Connect every component.

Handle loading.

Handle API failures.

Show success messages.

Review the complete application.

Find:

Compile errors

Runtime errors

Null reference issues

Dependency Injection issues

Angular binding issues

HTTP issues

Validation issues

Provide fixes.

Do not rewrite working code.

Only fix problems.

Prompt 13 — Code Review

Act as a Senior Software Architect.

Review the entire application.

Check:

SOLID

Naming

Performance

Security

Validation

Exception handling

Repository Pattern

REST API standards

Angular Best Practices

Suggest improvements.

Do not change functionality.

Prompt 14 — README, Copilot Documentation and Developer Notes

Generate README-AI-FULLSTACK-SUBMISSION.md

Include:

Project Overview

Architecture

Technology Stack

Setup Instructions

API Endpoints

Angular Setup

Database

Features

Copilot Usage

Screenshots Placeholder

Folder Structure

Generate docs/copilot-prompts-used.md

Include every prompt used during development.

For each prompt include:

Objective

Prompt

Copilot Response Summary

Changes made after review

Final outcome

Use Markdown.

Generate docs/developer-notes.md

Include:

Design Decisions

Architecture

Validation Strategy

Error Handling

Known Limitations

Future Enhancements

Testing Notes