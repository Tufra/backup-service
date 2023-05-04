using System.Security.Claims;
using Backup_Management_Service.Entities;
using Backup_Management_Service.Helper;
using Backup_Management_Service.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backup_Management_Service.Pages
{
    [Authorize]
    public class TestModel : PageModel
    {
        private readonly ApplicationContext _dbContext;
        public List<Backup> ListBackups { get; set; } = new List<Backup>();

        public TestModel(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGet()
        {
            ListBackups = await GetAllBackups(Guid.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!));
        }

        public async Task OnGetBackupFile(Guid backupId)
        {
            GetBackupFile(backupId);
        }
        public async Task<List<Backup>> GetAllBackups(Guid userId)
        {
            var backups = await _dbContext.Backups.Where(x => x.UserId == userId).ToListAsync();
            return backups;
        }

        public async Task OnPost(BackupScriptGenerationRequest request)
        {
            GenerateScript(request);
        }
        public string GenerateScript(BackupScriptGenerationRequest request)
        {
            return ScriptHelper.GenerateBackupScript(request);
        }

        public void GetBackupFile(Guid backupId)
        {
            //todo спрашивать у удаленного сервера
        }
    }
}
