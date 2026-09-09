# Tasks: Document Upload and Management

**Input**: Design documents from `/specs/document-management/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Confirm the existing Blazor app baseline and prepare the document-management feature surfaces.

- [x] T001 [P] Confirm the SQLite provider and app startup configuration in ContosoDashboard/Program.cs and ContosoDashboard/appsettings.json
- [x] T002 [P] Review and finalize the Document and DocumentShare metadata contracts in ContosoDashboard/Models/Document.cs and ContosoDashboard/Models/DocumentShare.cs
- [x] T003 [P] Validate the local file storage abstraction and folder conventions in ContosoDashboard/Services/IFileStorageService.cs and ContosoDashboard/Services/LocalFileStorageService.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before any user story begins.

- [x] T004 Create the document service contract in ContosoDashboard/Services/IDocumentService.cs
- [x] T005 [P] Implement secure local file upload, delete, and download behavior in ContosoDashboard/Services/LocalFileStorageService.cs
- [x] T006 [P] Register the document and storage services in the DI container in ContosoDashboard/Program.cs
- [x] T007 Add document persistence setup, indexes, and entity relationships in ContosoDashboard/Data/ApplicationDbContext.cs
- [x] T008 Implement authenticated file delivery and authorization checks in ContosoDashboard/Controllers/FileDownloadController.cs
- [x] T009 Add secure upload, ownership validation, and project access checks in ContosoDashboard/Services/DocumentService.cs

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Upload and manage personal documents (Priority: P1) 🎯 MVP

**Goal**: Allow an employee to upload and manage their own documents while enforcing secure local storage and access checks.

**Independent Test**: A user can sign in, upload a supported file, complete the required metadata, and confirm the uploaded document appears in their personal document list with the expected details.

### Implementation for User Story 1

- [x] T010 [US1] Add upload validation for required title/category, supported files, and 25MB file limits in ContosoDashboard/Services/DocumentService.cs
- [x] T011 [P] [US1] Update document metadata constraints and navigation properties in ContosoDashboard/Models/Document.cs to match the validation rules from data-model.md
- [x] T012 [US1] Build the personal document list, search, and sorting behavior in ContosoDashboard/Pages/Documents.razor
- [x] T013 [US1] Create the upload form and validation feedback in ContosoDashboard/Pages/DocumentUpload.razor
- [x] T014 [US1] Confirm the user-owned document access flow remains protected against IDOR attempts in ContosoDashboard/Services/DocumentService.cs and ContosoDashboard/Controllers/FileDownloadController.cs
- [x] T015 [US1] Ensure files are written to the secure storage location and never exposed directly via the web root in ContosoDashboard/Services/LocalFileStorageService.cs

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - Collaborate on project documents (Priority: P2)

**Goal**: Let authorized team members view and manage project documents in project context while denying unauthorized access.

**Independent Test**: A project participant can upload a file for a project, view it from that project’s context, and verify that a non-member is denied access.

### Implementation for User Story 2

- [x] T016 [US2] Add project membership and manager checks for upload and listing flows in ContosoDashboard/Services/DocumentService.cs
- [x] T017 [P] [US2] Add project document rendering and permission-aware behavior in ContosoDashboard/Pages/ProjectDetails.razor
- [x] T018 [US2] Ensure project document retrieval returns only authorized records in ContosoDashboard/Services/DocumentService.cs
- [x] T019 [P] [US2] Verify project document download and preview requests are rejected for unauthorized users in ContosoDashboard/Controllers/FileDownloadController.cs
- [x] T020 [US2] Surface project document status in the project dashboard flows in ContosoDashboard/Pages/Projects.razor

**Checkpoint**: At this point, User Stories 1 and 2 should both work independently

---

## Phase 5: User Story 3 - Share, update, and remove documents responsibly (Priority: P3)

**Goal**: Allow document owners and managers to share, update, and remove documents without leaving orphaned records or access leaks.

**Independent Test**: A document owner can share a document with another user, update the metadata, and then remove the document without leaving stale storage or share records.

### Implementation for User Story 3

- [x] T021 [US3] Implement share creation and duplicate-share prevention in ContosoDashboard/Services/DocumentService.cs
- [x] T022 [P] [US3] Add shared document actions and management UI in ContosoDashboard/Pages/Documents.razor
- [x] T023 [US3] Implement document metadata updates and file replacement flow in ContosoDashboard/Pages/DocumentUpload.razor and ContosoDashboard/Services/DocumentService.cs
- [x] T024 [US3] Enforce owner-or-manager delete permissions and remove the file and database record in ContosoDashboard/Services/DocumentService.cs
- [x] T025 [US3] Trigger in-app notifications for share events and project document additions in ContosoDashboard/Services/NotificationService.cs and ContosoDashboard/Services/DocumentService.cs

**Checkpoint**: All user stories should now be independently functional

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation, security review, and product polish across all user stories.

- [x] T026 [P] Review authorization, logging, and file-path handling across ContosoDashboard/Services/DocumentService.cs, ContosoDashboard/Controllers/FileDownloadController.cs, and ContosoDashboard/Data/ApplicationDbContext.cs
- [x] T027 [P] Refresh feature docs and validation guidance in README.md and specs/document-management/quickstart.md
- [x] T028 Run the final build and end-to-end validation pass in the ContosoDashboard project folder using the quickstart scenarios in specs/document-management/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1): no dependencies; can start immediately
- Foundational (Phase 2): depends on Setup completion and blocks all user stories
- User Story 1 (Phase 3): depends on Phase 2 completion and forms the MVP
- User Story 2 (Phase 4): depends on Phase 2 completion; can proceed after Story 1 or in parallel with limited integration
- User Story 3 (Phase 5): depends on Phase 2 completion and builds on the upload, access, and project document flows
- Polish (Phase 6): depends on all desired user stories being complete

### User Story Dependencies

- User Story 1 (P1): no cross-story dependency; can be delivered independently as the MVP
- User Story 2 (P2): depends on the secure base infrastructure and project membership model established in Phase 2
- User Story 3 (P3): depends on successful completion of upload, access, and project document flows

### Parallel Opportunities

- Phase 1 tasks can execute in parallel because they touch different files and do not depend on implementation work.
- Phase 2 tasks marked [P] can execute simultaneously once the repo baseline is confirmed.
- User Story 1 work can be developed independently of User Story 2 and 3 after foundation is available.
- The file-storage and access-control work can be parallelized with page UI authoring because they touch different layers of the app.

### Parallel Example: User Story 1

```bash
# Run the UI and service work in parallel once foundational tasks are complete
Task: "Build the personal document list and search behavior in ContosoDashboard/Pages/Documents.razor"
Task: "Add upload validation and secure storage logic in ContosoDashboard/Services/DocumentService.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate user-story-level success
5. Expand to Story 2 and Story 3 once the MVP is stable

### Incremental Delivery

1. Setup + foundational tasks
2. Personal document upload and list management
3. Project-scoped document collaboration and access enforcement
4. Document sharing, editing, and deletion
5. Security review and documentation polish

### Parallel Team Strategy

With multiple contributors:

1. One engineer completes the shared foundation in Phase 2.
2. Developer A focuses on User Story 1.
3. Developer B focuses on User Story 2.
4. Developer C focuses on User Story 3.
5. Final validation and documentation happen once all stories are integrated.

---

## Notes

- [P] = can run in parallel because different files or no dependencies
- [US1], [US2], [US3] map directly to the user stories in spec.md
- Each user story remains independently testable and deployable after its phase
- Prefer completing one story checkpoint before starting the next priority
- Keep all changes aligned with the constitution’s offline-first, infrastructure-abstraction, and defense-in-depth principles
