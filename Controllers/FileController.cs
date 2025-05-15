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

    //Endpoint för att ladda upp fil tll specifik mapp
    [HttpPost("uploadfile")]
    [Authorize]
    public async Task<ActionResult> UploadFileByIdAsync(IFormFile file, [FromForm] int folderId)
    {
        //Hämtar den inloggade användarens ID från tokenen
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Userid is missing.");
        }

        try
        {
            var uploadedFile = await service.UploadFileByIdAsync(file, folderId, userId);
            return Ok(uploadedFile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, ex.Message);
        }
    }

    //Endpoint för att ladda ned fil
    [HttpGet("download/{fileid}")]
    [Authorize]
    public async Task<ActionResult> DownloadFileByIdAsync(int fileId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Userid is missing.");
        }

        try
        {
            var file = await service.DownloadFileByIdAsync(userId, fileId);

            // Returnerar filen som ett nedladdningsbart svar till klienten.
            // Innehållet (byte[]), filtypen och filnamnet skickas med så att klienten kan spara filen.
            return File(file.Content, "application/octet-stream", file.FileName);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(403, ex.Message);
        }
    }

    //Endpoint för att radera fil baserat på id
    [HttpDelete("delete/{fileid}")]
    [Authorize]
    public async Task<ActionResult> DeleteFileByIdAsync(int fileId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Userid is missing.");
        }

        try
        {
            await service.DeleteFileByIdAsync(userId, fileId);
            return NoContent(); // 204 - lyckad radering utan innehåll
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, ex.Message);
        }
    }
}
