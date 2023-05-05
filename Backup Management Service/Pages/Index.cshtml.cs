using Backup_Management_Service.Entities;
using Backup_Management_Service.Helper;
using Backup_Management_Service.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;

namespace Backup_Management_Service.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _dbContext;

        public List<BackupInfo> ListBackupsInfo { get; set; }

        [BindProperty]
        public BackupScriptGenerationRequest CreateBackupRequest { get; set; }

        public IndexModel(ApplicationContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
        }

        public async Task OnGet()
        {
            ListBackupsInfo = await GetAllBackups(Guid.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!));
        }

        public async Task<List<BackupInfo>> GetAllBackups(Guid userId)
        {
            var backups = await _dbContext.BackupsInfo.Where(x => x.UserId == userId).ToListAsync();
            return backups;
        }

        public async Task<IActionResult> OnPostCreateBackUp()
        {
            GenerateScript();
            return Redirect("./");
        }
        public string GenerateScript()
        {
            CreateBackupRequest.UserId = Guid.Parse( HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return ScriptHelper.GenerateBackupScript(CreateBackupRequest);
        }

        public async Task<IActionResult> OnPostDownloadBackupFile(Guid backupId)
        {
            var backUpInfo = await _dbContext.BackupsInfo.FirstAsync(b => b.Id == backupId);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(backUpInfo.StoragePath);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(backUpInfo.StoragePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(fileBytes, contentType, backUpInfo.Name);
        }
    }
}