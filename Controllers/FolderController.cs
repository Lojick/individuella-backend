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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var folder = await service.AddAsync(dto, userId);
        return Ok(folder);
    }

    [HttpGet("get")]
    [Authorize]
    public async Task<ActionResult> GetAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var folders = await service.GetAsync(userId);
        return Ok(folders);
    }
}
