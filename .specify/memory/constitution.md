# ContosoDashboard Constitution
<!--
Sync Impact Report
Version change: 1.0.0 -> 1.1.0
Modified principles:
- I. Offline-First with Cloud Migration Path -> I. Offline-First with Cloud Migration Path
- II. Infrastructure Abstraction -> II. Infrastructure Abstraction
- III. Training Purpose Constraints -> III. Training Purpose Constraints
- IV. Defense in Depth -> IV. Defense in Depth
- V. Code Quality Standards -> V. Code Quality Standards
Added sections: None
Removed sections: None
Follow-up TODOs: None
-->

## Core Principles

### I. Offline-First with Cloud Migration Path
This application must remain usable in offline training environments. All dependencies and operational workflows must work without internet access or external cloud services. Any future migration to Azure or other cloud services must be achievable by swapping infrastructure implementations, not by rewriting business logic.

### II. Infrastructure Abstraction
All infrastructure dependencies must use interface abstractions such as `IFileStorageService` and injectable service boundaries. Business logic must depend on abstractions rather than concrete platform-specific implementations, keeping the application portable and testable.

### III. Training Purpose Constraints
This repository is intentionally for training and demonstration. The application must avoid external service dependencies whenever possible, use mock authentication and simplified security scaffolding, and remain easy to run locally without production-grade infrastructure.

### IV. Defense in Depth
The application must implement layered security controls even in a training context:
- Authorization must be enforced on protected pages and routes via `[Authorize]`.
- Role-based access control must be hierarchical and explicit.
- Service-level validation must prevent unauthorized access and IDOR-based data exposure.
- User isolation must ensure each user only sees their own authorized data and actions.

### V. Code Quality Standards
- Async and await must be used for non-blocking I/O and service operations.
- EF Core queries must minimize N+1 issues through explicit eager loading and careful query design.
- Clean separation of concerns must remain across Models, Services, Data, and Pages.
- Dependency injection must be used for loose coupling and maintainability.
- Frequently queried fields require indexes to support predictable performance.

## Security Requirements
- IDOR Protection: service methods must verify that the current user has permission before accessing or mutating any entity.
- File Upload Security: uploaded files must be stored outside the `wwwroot` directory and must be validated before saving.
- Safe File Paths: unique file paths must be generated before database insertion to avoid collisions, path traversal, and orphaned records.
- Authentication boundaries: the mock authentication flow must stay aligned with the application’s training-only design and must not be treated as production-grade identity.

## Development Workflow
- Features must align with the mock authentication and authorization model unless explicitly modified by governance.
- Database schema decisions must favor simple, consistent integer identifiers for core entities within the current training architecture.
- Changes must preserve offline-first operation and must not introduce hidden cloud dependencies.
- Any new feature, refactor, or infrastructure change must be reviewed for security, abstraction, and training-scope compliance before approval.

## Governance
This constitution governs how the ContosoDashboard project is implemented and reviewed. All changes that affect security, persistence, authentication, or architecture must remain consistent with these principles. Any amendment requires a documented rationale, a version bump, and confirmation that the change preserves the project’s training-only scope and offline-first constraints.

All reviews must verify compliance with this constitution before merge approval. Complexity, security exceptions, and new platform dependencies must be justified in writing and must not bypass the core principles.

**Version**: 1.1.0 | **Ratified**: 2026-09-08 | **Last Amended**: 2026-09-09
