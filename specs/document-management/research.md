# Research: Document Upload and Management

## Summary

This feature is implemented inside the existing ContosoDashboard single-project architecture using Blazor Server, SQLite, and a storage abstraction. The design decision is to keep document metadata in the relational database and physical files in a secure local folder outside `wwwroot`.

## Decisions

### Decision: Use SQLite for metadata persistence
- Chosen because the project is running in a local training environment and SQLite works cross-platform on Linux ARM64 without LocalDB dependencies.
- Rationale: minimizes setup friction and matches the repository’s offline-first training objective.
- Alternatives considered: SQL Server LocalDB and Azure SQL. LocalDB was rejected because it is Windows-specific and incompatible with the current ARM64 environment; Azure SQL was rejected because the project must remain offline-first and dependency-light during training.

### Decision: Store files in a local `AppData/uploads` area, not in `wwwroot`
- Chosen because the requirement explicitly calls for a secure file store outside the web root.
- Rationale: prevents direct browser access to stored files and makes authorization checks enforceable via controller logic.
- Alternatives considered: storing directly under `wwwroot` or in a blob-like cloud service. Direct static files were rejected because they bypass access control; cloud storage is intentionally deferred for the training scenario.

### Decision: Use a storage abstraction (`IFileStorageService`) and service-layer authorization
- Chosen because the project constitution requires infrastructure abstraction and defense-in-depth.
- Rationale: the document service performs ownership and project-membership checks before upload, access, or deletion operations.
- Alternatives considered: letting pages or controllers directly manipulate files. This was rejected because it would weaken security and violate the established architecture.

### Decision: Keep the file download contract behind an authenticated controller
- Chosen because the requirement requires secure serving and IDOR protection.
- Rationale: a dedicated controller can validate claims and call the document service before streaming the file to the user.
- Alternatives considered: serving files directly from disk or giving open access URLs. These were rejected because they fail the authorization requirements.

## Constraints and consequences

- Document metadata and file access control must remain consistent with the existing mock auth model.
- The implementation should remain simple and understandable for students rather than enterprise-grade.
- The training app cannot rely on cloud services or external malware scanners, so the feature must document that limitation explicitly.
