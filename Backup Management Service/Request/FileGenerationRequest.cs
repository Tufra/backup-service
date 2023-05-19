namespace Backup_Management_Service.Request
{
    public class FileGenerationRequest
    {
        public string UserLocalPath { get; set; }
        public string backupFrequency { get; set; }
    }
}
