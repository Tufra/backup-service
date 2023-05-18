using Backup_Management_Service.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Backup_Management_Service.Controllers;

[Route("api/backup")]
[ApiController]
public class BackupController : ControllerBase
{
    private readonly ApplicationContext _context;

    private const string BackupPath = "./User_backups";

    public BackupController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadBackup(IFormFile data, [FromQuery] Guid userId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var dir = Directory.CreateDirectory(Path.Combine(BackupPath, userId.ToString()));
        var path = $"{dir.FullName}/{data.FileName}";

        try
        {
            await using var stream = System.IO.File.Create(path);
            await data.CopyToAsync(stream);

            var backupInfo = new BackupInfo
            {
                UserId = userId,
                Name = data.FileName,
                StoragePath = path,
                UserLocalPath = "",
                Created = DateTime.UtcNow,
                Description = "",
            };

            _context.BackupsInfo.Add(backupInfo);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            // Delete file if backup was unsuccessful
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            // If created directory is empty after deleting unsuccessful backup, then delete it
            if (!dir.EnumerateFiles().Any())
            {
                Directory.Delete(dir.FullName);
            }

            await transaction.RollbackAsync();
        }

        return Ok();
    }
}