namespace ContosoDashboard.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string directoryPath);
    Task DeleteAsync(string filePath);
    Task<Stream> DownloadAsync(string filePath);
}
