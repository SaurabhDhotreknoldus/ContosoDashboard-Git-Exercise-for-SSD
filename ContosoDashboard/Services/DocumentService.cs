using Microsoft.EntityFrameworkCore;
using ContosoDashboard.Data;
using ContosoDashboard.Models;

namespace ContosoDashboard.Services;

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public DocumentService(ApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<Document> UploadDocumentAsync(int userId, string title, string description, string category, int? projectId, string tags, Stream fileStream, string fileName, string contentType, long fileSize)
    {
        // 1. Authorize: if project is specified, ensure user is a member
        if (projectId.HasValue)
        {
            var isMember = await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == projectId.Value && pm.UserId == userId);
                
            var isManager = await _context.Projects
                .AnyAsync(p => p.ProjectId == projectId.Value && p.ProjectManagerId == userId);

            if (!isMember && !isManager)
            {
                throw new UnauthorizedAccessException("You do not have permission to upload documents to this project.");
            }
        }

        // 2. Generate path
        var directoryPath = projectId.HasValue ? $"project-{projectId.Value}" : $"user-{userId}";
        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";

        // 3. Upload file
        var filePath = await _fileStorageService.UploadAsync(fileStream, uniqueFileName, contentType, directoryPath);

        // 4. Save metadata
        var document = new Document
        {
            Title = title,
            Description = description,
            Category = category,
            Tags = tags,
            FilePath = filePath,
            FileType = contentType,
            FileSize = fileSize,
            UploaderId = userId,
            ProjectId = projectId,
            UploadDate = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return document;
    }

    public async Task<IEnumerable<Document>> GetUserDocumentsAsync(int userId)
    {
        return await _context.Documents
            .Where(d => d.UploaderId == userId || d.Shares.Any(s => s.UserId == userId))
            .Include(d => d.Project)
            .OrderByDescending(d => d.UploadDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Document>> GetProjectDocumentsAsync(int projectId, int userId)
    {
        var isMember = await _context.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
            
        var isManager = await _context.Projects
            .AnyAsync(p => p.ProjectId == projectId && p.ProjectManagerId == userId);

        if (!isMember && !isManager)
        {
            throw new UnauthorizedAccessException("You do not have permission to view documents for this project.");
        }

        return await _context.Documents
            .Where(d => d.ProjectId == projectId)
            .Include(d => d.Uploader)
            .OrderByDescending(d => d.UploadDate)
            .ToListAsync();
    }

    public async Task<Document?> GetDocumentByIdAsync(int documentId, int userId)
    {
        var document = await _context.Documents
            .Include(d => d.Project)
            .FirstOrDefaultAsync(d => d.DocumentId == documentId);

        if (document == null) return null;

        // IDOR Check
        if (document.UploaderId != userId && !document.Shares.Any(s => s.UserId == userId))
        {
            if (document.ProjectId.HasValue)
            {
                var isMember = await _context.ProjectMembers
                    .AnyAsync(pm => pm.ProjectId == document.ProjectId.Value && pm.UserId == userId);
                    
                var isManager = await _context.Projects
                    .AnyAsync(p => p.ProjectId == document.ProjectId.Value && p.ProjectManagerId == userId);

                if (!isMember && !isManager)
                {
                    throw new UnauthorizedAccessException("You do not have permission to view this document.");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to view this document.");
            }
        }

        return document;
    }

    public async Task DeleteDocumentAsync(int documentId, int userId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null) return;

        // IDOR Check
        if (document.UploaderId != userId)
        {
            // Allow project managers to delete
            if (document.ProjectId.HasValue)
            {
                var isManager = await _context.Projects
                    .AnyAsync(p => p.ProjectId == document.ProjectId.Value && p.ProjectManagerId == userId);
                    
                if (!isManager)
                {
                    throw new UnauthorizedAccessException("Only the uploader or project manager can delete this document.");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("Only the uploader can delete this document.");
            }
        }

        await _fileStorageService.DeleteAsync(document.FilePath);
        
        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();
    }

    public async Task ShareDocumentAsync(int documentId, int ownerId, int targetUserId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document == null) return;

        if (document.UploaderId != ownerId)
        {
            throw new UnauthorizedAccessException("Only the owner can share this document.");
        }

        var shareExists = await _context.DocumentShares
            .AnyAsync(s => s.DocumentId == documentId && s.UserId == targetUserId);

        if (!shareExists)
        {
            _context.DocumentShares.Add(new DocumentShare
            {
                DocumentId = documentId,
                UserId = targetUserId
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Stream> DownloadDocumentAsync(int documentId, int userId)
    {
        var document = await GetDocumentByIdAsync(documentId, userId); // Reuses IDOR checks
        if (document == null)
        {
            throw new FileNotFoundException("Document not found");
        }

        return await _fileStorageService.DownloadAsync(document.FilePath);
    }
}
