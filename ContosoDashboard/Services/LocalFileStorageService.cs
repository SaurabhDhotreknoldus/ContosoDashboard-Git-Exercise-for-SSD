namespace ContosoDashboard.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _baseStoragePath;

    public LocalFileStorageService(IWebHostEnvironment env)
    {
        // Store files outside wwwroot for security
        _baseStoragePath = Path.Combine(env.ContentRootPath, "AppData", "uploads");
        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string directoryPath)
    {
        var relativePath = Path.Combine(directoryPath, fileName);
        var fullPath = Path.Combine(_baseStoragePath, relativePath);
        
        var directory = Path.GetDirectoryName(fullPath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var fileStreamOnDisk = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await fileStream.CopyToAsync(fileStreamOnDisk);

        return relativePath.Replace("\\", "/"); // Ensure forward slashes for portability
    }

    public Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_baseStoragePath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        return Task.CompletedTask;
    }

    public Task<Stream> DownloadAsync(string filePath)
    {
        var fullPath = Path.Combine(_baseStoragePath, filePath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("The requested file was not found.", fullPath);
        }

        var memoryStream = new MemoryStream();
        using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            fileStream.CopyTo(memoryStream);
        }
        memoryStream.Position = 0;
        return Task.FromResult<Stream>(memoryStream);
    }
}
