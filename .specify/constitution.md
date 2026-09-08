# ContosoDashboard Constitution

## Core Principles

### I. Offline-First with Cloud Migration Path
This application follows an offline-first architecture with abstraction layers that enable seamless migration to Azure services. For training, all dependencies must work without internet access. Future migration to Azure must require only configuration and implementation swaps, no business logic changes.

### II. Infrastructure Abstraction
All infrastructure dependencies must use interface abstractions to enable switching between local and cloud implementations. Business logic must rely on these interfaces (e.g., `IFileStorageService`) rather than concrete implementations.

### III. Training Purpose Constraints
The application is for training purposes. It must avoid external service dependencies to maximize training availability. It utilizes mock authentication and simplified security mechanisms (e.g., no password required) intentionally.

### IV. Defense in Depth
The application must employ defense in depth for security, despite its training nature:
- Authorization enforcement on all protected pages (`[Authorize]`)
- Role-based access control (RBAC) with hierarchical permissions
- Service-level security to prevent Insecure Direct Object Reference (IDOR)
- User isolation (each user sees only their authorized data)

### V. Code Quality Standards
- Async/await pattern must be used throughout for non-blocking operations.
- Entity Framework Core must use eager loading (`.Include()`) to prevent N+1 query problems.
- Clean separation of concerns (Models, Services, Data, Pages).
- Dependency injection for loose coupling and testability.
- Database indexes on frequently queried fields.

## Security Requirements

- **IDOR Protection**: Service methods must verify the current user has permission to access the requested entity.
- **File Upload Security**: Uploaded files must be stored securely outside the `wwwroot` directory. File extensions must be validated.
- **Safe File Paths**: Unique file paths (e.g., using GUIDs) must be generated BEFORE database insertion to prevent duplicate key violations and path traversal attacks. User-supplied filenames must never be used directly in file paths.

## Development Workflow

- All features must align with the mock authentication system.
- Database schemas should use integer IDs for primary keys for consistency, unless otherwise specified.

**Version**: 1.0.0 | **Ratified**: 2026-09-08 | **Last Amended**: 2026-09-08
