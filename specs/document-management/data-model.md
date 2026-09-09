# Data Model: Document Upload and Management

## Entities

### Document
Represents a single uploaded file and the metadata associated with it.

- DocumentId: integer, primary key
- Title: string, required, max 255
- Description: string, optional, max 2000
- Category: string, required, max 100
- Tags: string, optional, max 500
- FilePath: string, required, used to locate the file in the secure local storage area
- FileType: string, required, max 255
- FileSize: long, required
- UploadDate: datetime, required
- UploaderId: integer, required, foreign key to User
- ProjectId: integer, optional, foreign key to Project
- Project: navigation to owning project when present
- Uploader: navigation to the user who uploaded the document
- Shares: collection of document-sharing relationships

### DocumentShare
Represents the grant of access for a document to a specific user.

- DocumentShareId: integer, primary key
- DocumentId: integer, required, foreign key to Document
- UserId: integer, required, foreign key to User
- SharedDate: datetime, required
- Document: navigation to the document
- User: navigation to the recipient user

### User
The existing user entity remains the identity and role anchor for all access checks.

- UserId: integer, primary key
- Email: unique string
- DisplayName: string
- Role: user role enum representing employee, team lead, project manager, or administrator
- Department and job metadata

### Project
The existing project entity provides the project-scoped document context.

- ProjectId: integer, primary key
- Name: string
- ProjectManagerId: integer, required
- Status: enum
- Documents: optional collection of project documents

## Relationships

- A User can upload many documents.
- A Project can contain many documents.
- A Document can have many share records.
- A User can be the recipient of many shared documents.

## Validation Rules

- Document title is required.
- Category must be a supported business value such as Project Documents, Team Resources, Personal Files, Reports, Presentations, or Other.
- Document file path must be generated securely before saving file metadata.
- Document access must be denied unless the user owns the document, is explicitly shared the document, or is a valid project member/manager for the related project.

## State transitions

- Draft/NotUploaded -> Uploaded: file is validated, saved to local storage, and record inserted.
- Uploaded -> Shared: a `DocumentShare` is created for a user.
- Uploaded -> Deleted: file is removed from storage and document record removed from database.
- Uploaded -> Updated: metadata edits or replacement may occur without changing the document identity.
