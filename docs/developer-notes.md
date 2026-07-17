# Developer Notes

## Design Decisions

The project was intentionally kept small and focused on the internal order-management scope.

Key decisions:

- Use a single `Order` entity model to match the case study exactly.
- Use ASP.NET Core Web API with EF Core InMemory for quick local development and testing.
- Use the Repository Pattern and Service Layer to separate persistence concerns from business rules.
- Use Angular standalone components to keep the frontend lightweight and modern.
- Use Bootstrap for consistent responsive UI without introducing unnecessary complexity.

## Architecture

### Backend

- `Controllers` expose only the required REST endpoints.
- `Services` contain the business validation and orchestration logic.
- `Repositories` isolate EF Core query and persistence behavior.
- `Middleware` centralizes global exception handling.
- `Data` contains the EF Core `DbContext` and seeding logic.

### Frontend

- `components` handle dashboard, list, and form views.
- `services` encapsulate typed API access.
- `models` and `enums` drive type safety.
- `environments` hold the API base URL for different build modes.

## Validation Strategy

Validation is applied across the request lifecycle:

- API model validation through annotations and `ModelState`
- Service-level business validation for mandatory order fields and status rules
- Angular reactive form validation for user inputs and field-level feedback

The validation model is intentionally conservative and aligned with the required *internal order* use case.

## Error Handling

### Backend

A global exception middleware standardizes API errors into a consistent JSON shape:

- StatusCode
- Message
- Timestamp
- Path

This handles:

- `ValidationException`
- `NotFoundException`
- unhandled runtime exceptions

### Frontend

The Angular service handles HTTP failures centrally, and the components surface loading/error/success messages to keep the UX predictable.

## Known Limitations

- The database is intentionally in-memory and is not suitable for production persistence.
- Search and list operations are lightweight and appropriate for the current internal scope, but not optimized for massive datasets.
- There is no authentication or role-based access, by design.
- Swagger examples and response examples are present, but the solution remains intentionally small in scope.

## Future Enhancements

Potential future improvements, without changing the current scope:

- Add pagination and filtering support at the API level for larger queries.
- Replace the in-memory database with a relational database such as SQL Server or PostgreSQL.
- Introduce request DTOs for cleaner API contracts.
- Add structured `ProblemDetails` responses for more standardized API interoperability.
- Add automated integration tests for API and UI regression checks.

## Testing Notes

The implementation was validated through:

- backend build verification
- runtime API endpoint verification
- Angular build verification
- application bundle generation confirmation

Recommended future testing improvements:

- unit tests for service validation rules
- integration tests for controller endpoints
- Angular component tests for forms and dashboard data loading
- end-to-end smoke tests for the order lifecycle
