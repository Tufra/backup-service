using System.ComponentModel.DataAnnotations;

namespace Backup_Management_Service.Entities;

public class User
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Login { get; set; } = null!;

    [Required] 
    public string Name { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;
}