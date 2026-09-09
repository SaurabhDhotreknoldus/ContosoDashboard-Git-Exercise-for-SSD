# File Download Contract

## Overview

The application exposes a secure file-download endpoint for document retrieval. This contract is used by the Blazor app to serve documents only after authorization checks succeed.

## Endpoint

- Method: GET
- Route: `/api/FileDownload/{id}`
- Authentication: Required
- Authorization: User must have permission to access the document

## Request

- Path parameter: `id` = document identifier integer
- Claims: authenticated user identifier from the cookie-based mock auth system

## Response

### Success
- Status: 200 OK
- Content-Type: the stored document MIME type
- Content-Disposition: attachment or inline preview headers, depending on the content type
- Body: raw file stream for the authorized document

### Failure
- 401 Unauthorized: user is not authenticated
- 403 Forbidden: user does not have access to the requested document
- 404 Not Found: document id does not exist or the file is missing

## Security requirements

- The controller must verify the authenticated user against the document owner, project membership, or explicit sharing permissions before streaming the file.
- The stored file path must never be exposed directly.
- All file content must be served through the application rather than direct static file access.
