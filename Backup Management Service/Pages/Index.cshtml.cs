using Backup_Management_Service.Entities;
using Backup_Management_Service.Helper;
using Backup_Management_Service.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Backup_Management_Service.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _dbContext;
        public List<BackupInfo> ListBackupsInfo { get; set; } = new List<BackupInfo>();

        [BindProperty]
        public BackupScriptGenerationRequest CreateBackupRequest { get; set; }

        public IndexModel(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGet()
        {
            ListBackupsInfo = await GetAllBackups(Guid.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!));
        }

        public async Task OnGetBackupFile(Guid backupId)
        {
            GetBackupFile(backupId);
        }
        public async Task<List<BackupInfo>> GetAllBackups(Guid userId)
        {
            var backups = await _dbContext.BackupsInfo.Where(x => x.UserId == userId).ToListAsync();
            return backups;
        }

        public async Task OnPostCreateBackUp()
        {
            GenerateScript();
        }
        public string GenerateScript()
        {
            CreateBackupRequest.UserId = Guid.Parse( HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return ScriptHelper.GenerateBackupScript(CreateBackupRequest);
        }

        public void GetBackupFile(Guid backupId)
        {
            //todo спрашивать у удаленного сервера
        }
    }
}