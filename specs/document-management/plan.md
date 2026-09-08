# Implementation Plan: Document Upload and Management

**Branch**: `feature-document-management` | **Date**: 2026-09-08 | **Spec**: `/specs/document-management/spec.md`
**Input**: Feature specification from `/specs/document-management/spec.md`

## Summary

Implement document upload and management for ContosoDashboard using Blazor Server and EF Core. The implementation uses a local file storage service abstraction (`IFileStorageService`) to store files outside `wwwroot` securely, and tracks document metadata in a new `Document` database entity.

## Technical Context

**Language/Version**: C# 12 / .NET 8.0  
**Primary Dependencies**: ASP.NET Core Blazor Server, Entity Framework Core  
**Storage**: SQL Server LocalDB, Local File System (`AppData/uploads`)  
**Target Platform**: Web (Offline Training Environment)  
**Constraints**: Must use interface abstractions for storage, no external cloud dependencies, IDOR protection required.  

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] Offline-First with Cloud Migration Path (Uses IFileStorageService)
- [x] Defense in Depth (IDOR protection on downloads and viewing)
- [x] Safe File Paths (GUID-based names generated before database insertion)

## Project Structure

### Source Code

```text
ContosoDashboard/
├── Models/
│   ├── Document.cs             # New entity
│   └── DocumentShare.cs        # New entity
├── Services/
│   ├── IFileStorageService.cs  # Storage abstraction
│   ├── LocalFileStorageService.cs
│   ├── IDocumentService.cs
│   └── DocumentService.cs
├── Pages/
│   ├── Documents.razor         # Document list
│   └── DocumentUpload.razor    # Upload component
└── Controllers/
    └── FileDownloadController.cs # Secure file delivery
```

**Structure Decision**: Integrated into the existing monolithic ContosoDashboard structure, adhering to the established separation of concerns (Models, Services, Pages).

## Complexity Tracking

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| Controller added to Blazor app | Secure file downloads outside `wwwroot` | Direct static files bypass authorization. |
