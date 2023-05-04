using Backup_Management_Service.Common;

namespace Backup_Management_Service.Request
{
    public class BackupScriptGenerationRequest
    {
        public Guid? UserId { get; set; }
        public string UserLocalPath { get; set; }
        public BackupFrequency BackupFrequency { get; set; }
    }
}
