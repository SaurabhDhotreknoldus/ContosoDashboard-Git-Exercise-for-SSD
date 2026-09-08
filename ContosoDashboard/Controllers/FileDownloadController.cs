using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContosoDashboard.Services;
using System.Security.Claims;

namespace ContosoDashboard.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FileDownloadController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public FileDownloadController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Download(int id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized();
        }

        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id, userId);
            if (document == null)
            {
                return NotFound();
            }

            var stream = await _documentService.DownloadDocumentAsync(id, userId);
            
            // Return file with original name and content type
            return File(stream, document.FileType, document.Title + Path.GetExtension(document.FilePath));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}
