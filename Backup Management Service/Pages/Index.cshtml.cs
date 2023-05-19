using Backup_Management_Service.Entities;
using Backup_Management_Service.Helper;
using Backup_Management_Service.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Backup_Management_Service.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _dbContext;
        private readonly ScriptHelper _scriptHelper;

        public List<BackupInfo> ListBackupsInfo { get; set; } = null!;

        [BindProperty]
        public BackupScriptGenerationRequest CreateBackupRequest { get; set; } = new();

        public IndexModel(ApplicationContext dbContext, ScriptHelper scriptHelper)
        {
            _dbContext = dbContext;
            _scriptHelper = scriptHelper;
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

        public async Task<IActionResult> OnPostGenerateScript()
        {
            string regexPattern = @"^(~|\.?\/).*";

            if (!string.IsNullOrWhiteSpace(CreateBackupRequest.UserLocalPath) && !Regex.IsMatch(CreateBackupRequest.UserLocalPath, regexPattern))
            {
                ModelState.AddModelError(nameof(CreateBackupRequest.UserLocalPath), "Invalid source path");
            }

            if (CreateBackupRequest.KeepFile)
            {
                if(string.IsNullOrWhiteSpace(CreateBackupRequest.UserBackupStoragePath))
                    ModelState.AddModelError(nameof(CreateBackupRequest.UserBackupStoragePath), "You specified to save the backup, but you didn't specify where in this example");
                else if (!Regex.IsMatch(CreateBackupRequest.UserBackupStoragePath, regexPattern))
                    ModelState.AddModelError(nameof(CreateBackupRequest.UserBackupStoragePath), "Invalid destination path");
            }

            if (!ModelState.IsValid)
            {
                ListBackupsInfo = await GetAllBackups(Guid.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!));
                return Page();
            }

            

            CreateBackupRequest.UserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var scriptContent = await _scriptHelper.GenerateBackupScript(CreateBackupRequest, HttpContext.Request.Host.Value);

            var byteArray = Encoding.UTF8.GetBytes(scriptContent);
            var fileName = $"{CreateBackupRequest.BackupName}_backup_script.sh";
            var contentType = "text/plain";

            return File(byteArray, contentType, fileName);
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