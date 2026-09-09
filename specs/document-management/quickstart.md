# Quickstart: Document Upload and Management

## Prerequisites

- .NET SDK installed for the current environment
- The ContosoDashboard project is running locally
- A mock user account is available via the dashboard login page

## Validation scenarios

### 1. Upload a personal document

1. Start the app with `dotnet run` in the project folder.
2. Sign in with a mock user, such as a project team member.
3. Navigate to the document view or upload page.
4. Upload a valid PDF or image under 25 MB.
5. Provide a title, category, and optional description.
6. Confirm the uploaded item appears in the user’s personal document list.

Expected result: the document is stored securely and visible in the list with the uploaded metadata.

### 2. Upload a project document

1. Sign in as a user who belongs to a project.
2. Open the project details page.
3. Upload a document with the project association selected.
4. Refresh the project document view.

Expected result: the document is visible from the project page and is accessible to authorized project members.

### 3. Confirm access control

1. Sign out and sign in as a different user who is not a member of the project.
2. Attempt to access a file via known document ID or project context.

Expected result: the app denies access and returns an authorization error or forbidden response.

### 4. Share and delete document

1. Upload a document as the owner.
2. Share it with another mock user.
3. Log in as the shared recipient and verify the document is visible in their shared list.
4. Remove the document as the owner.

Expected result: access is revoked and the document is removed from storage and the database.

## Expected outcomes

- Files are saved outside `wwwroot`.
- Only authorized users can access document content.
- The local training app remains functional without external services.
