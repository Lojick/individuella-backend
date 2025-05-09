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

    [HttpPost("add")]
    [Authorize]
    public async Task<ActionResult> AddAsync([FromBody] CreateFileDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var file = await service.AddAsync(dto, userId);
        return Ok(file);
    }
}
