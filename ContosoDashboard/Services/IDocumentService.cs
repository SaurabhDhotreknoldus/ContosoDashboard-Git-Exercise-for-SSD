using ContosoDashboard.Models;

namespace ContosoDashboard.Services;

public interface IDocumentService
{
    Task<Document> UploadDocumentAsync(int userId, string title, string description, string category, int? projectId, string tags, Stream fileStream, string fileName, string contentType, long fileSize);
    Task<IEnumerable<Document>> GetUserDocumentsAsync(int userId);
    Task<IEnumerable<Document>> GetProjectDocumentsAsync(int projectId, int userId);
    Task<Document?> GetDocumentByIdAsync(int documentId, int userId);
    Task DeleteDocumentAsync(int documentId, int userId);
    Task ShareDocumentAsync(int documentId, int ownerId, int targetUserId);
    Task<Stream> DownloadDocumentAsync(int documentId, int userId);
}
