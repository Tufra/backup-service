namespace Backup_Management_Service.Entities
{
    public class Backup
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string UserLocalPath { get; set; }
        public string StoragePath { get; set; } = null!;
    }
}
