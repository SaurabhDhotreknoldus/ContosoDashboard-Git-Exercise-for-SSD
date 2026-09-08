# Implementation Tasks: Document Upload and Management

## Phase 1: Data Layer & Core Abstractions
- [ ] 1. Create `Document` model in `ContosoDashboard/Models/Document.cs`.
- [ ] 2. Create `DocumentShare` model in `ContosoDashboard/Models/DocumentShare.cs`.
- [ ] 3. Update `ApplicationDbContext.cs` to include `DbSet<Document>` and `DbSet<DocumentShare>`.
- [ ] 4. Create database migration for the new entities and apply it (if applicable, or let `EnsureCreated` handle it).

## Phase 2: Service Layer
- [ ] 5. Create `IFileStorageService` in `ContosoDashboard/Services/`.
- [ ] 6. Create `LocalFileStorageService` implementing `IFileStorageService` (saves to `AppData/uploads`).
- [ ] 7. Create `IDocumentService` in `ContosoDashboard/Services/`.
- [ ] 8. Create `DocumentService` implementing `IDocumentService` (with IDOR checks).
- [ ] 9. Register the new services in `Program.cs`.

## Phase 3: Controller & Presentation Layer
- [ ] 10. Create `FileDownloadController` in `ContosoDashboard/Controllers/` to serve files securely.
- [ ] 11. Create `DocumentUpload.razor` component in `ContosoDashboard/Pages/`.
- [ ] 12. Create `Documents.razor` page in `ContosoDashboard/Pages/` to list documents.
- [ ] 13. Update `Shared/NavMenu.razor` to include a link to the Documents page.

## Phase 4: Verification
- [ ] 14. Verify upload functionality via Blazor UI.
- [ ] 15. Verify IDOR protection (attempt to download another user's document).
