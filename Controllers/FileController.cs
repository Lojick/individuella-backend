using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/file")]
public class FileController : ControllerBase
{
    private readonly FileService service;

    public FileController(FileService service)
    {
        this.service = service;
    }

    [HttpPost("uploadfile")]
    [Authorize]
    public async Task<ActionResult> UploadFileAsync(IFormFile file, [FromForm] int folderId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        try
        {
            var uploadedFile = await service.UploadFileAsync(file, folderId, userId);
            return Ok(uploadedFile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound("You cannot save files into this folder.");
        }
    }

    [HttpGet("download/{fileid}")]
    [Authorize]
    public async Task<ActionResult> DownloadFileByIdAsync(int fileId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var file = await service.DownloadFileByIdAsync(userId, fileId);

        // Returnerar filen som ett nedladdningsbart svar till klienten.
        // Innehållet (byte[]), filtypen och filnamnet skickas med så att klienten kan spara filen.
        return File(file.Content, "application/octet-stream", file.FileName);
    }

    //Endpoint för att radera fil baserat på id
    [HttpDelete("delete/{fileid}")]
    [Authorize]
    public async Task<ActionResult> DeleteFileByIdAsync(int fileId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            await service.DeleteFileByIdAsync(userId, fileId);
            return NoContent(); // 204 - lyckad radering utan innehåll
        }
        catch (FileNotFoundException)
        {
            return NotFound("Filen hittades inte.");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("Du har inte rätt att radera denna fil.");
        }
    }
}
