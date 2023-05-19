using Backup_Management_Service.Request;
using Flurl;

namespace Backup_Management_Service.Helper
{
    public class ScriptHelper
    {
        private readonly string _scriptFilePath;

        public ScriptHelper(IWebHostEnvironment env)
        {
            _scriptFilePath = Path.Combine(env.ContentRootPath, "Bash script samples", "backup.sh");
        }

        public async Task<string> GenerateBackupScript(BackupScriptGenerationRequest request, string rootUrl)
        {
            var fileContent = await File.ReadAllTextAsync(_scriptFilePath);
            var controllerUrl = $"{rootUrl}/api/backup/upload".SetQueryParam(nameof(request.UserId), request.UserId);
            
            fileContent = string.Format(fileContent, request.BackupName, request.UserLocalPath,
                string.IsNullOrWhiteSpace(request.UserBackupStoragePath) ? ".": request.UserBackupStoragePath, request.CronExpression, controllerUrl, request.TransferFile ? "0":"1", request.KeepFile ? "0": "1", request.SetCronJob ? "0" : "1");

            return fileContent;
        }
    }
}