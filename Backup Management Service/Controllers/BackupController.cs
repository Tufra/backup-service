using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backup_Management_Service.Controllers;

[Route("api/backup")]
[ApiController]
public class BackupController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly ILogger _logger;

    public BackupController(ApplicationContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task UploadBackup( IFormFile backup)
    {
        _logger.LogInformation("File is upload");
    }
}