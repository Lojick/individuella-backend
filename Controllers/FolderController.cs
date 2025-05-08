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

    [HttpGet("getfolders")]
    [Authorize]
    public async Task<ActionResult> GetAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var folder = await service.GetAsync(userId);

        if (folder == null)
        {
            return NotFound();
        }
        return Ok(folder);
    }
}
