using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/folder")]
public class FolderController : ControllerBase
{
    private readonly FolderService service;

    public FolderController(FolderService service)
    {
        this.service = service;
    }

    [HttpPost("add")]
    [Authorize]
    //Endpoint för att skapa mappar
    public async Task<ActionResult> AddAsync([FromBody] CreateFolderDto dto)
    {
        //Hämtar den inloggade användarens ID från tokenen
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var folder = await service.AddAsync(dto, userId);
        return Ok(folder);
    }

    //Endpoint för att hämta mappar och dess innehåll (filer)
    [HttpGet("getfolderswithfiles")]
    [Authorize]
    public async Task<ActionResult> GetFoldersWithFilesAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var folder = await service.GetFoldersWithFilesAsync(userId);
        return Ok(folder);
    }
}
