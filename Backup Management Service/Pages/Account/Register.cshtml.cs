using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;
using Backup_Management_Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backup_Management_Service.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly ApplicationContext _context;

    [BindProperty]
    public UserRegisterModel UserRegisterModel { get; set; } = new();

    public RegisterModel(ApplicationContext context)
    {
        _context = context;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == UserRegisterModel.Login);

        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Username already exists.");
            return Page();
        }

        var user = new User
        {
            Login = UserRegisterModel.Login,
            Name = UserRegisterModel.Name,
            PasswordHash = UserRegisterModel.PasswordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();


        return RedirectToPage("Login");
    }
}

public class UserRegisterModel
{
    [Required]
    public string Login { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;
}