using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backup_Management_Service.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ApplicationContext _context;

    [BindProperty] public UserLogin UserLogin { get; set; } = new();


    public LoginModel(ApplicationContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(UserLogin userLogin)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin.Login && u.PasswordHash == userLogin.PasswordHash);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToPage("Index");
    }
}

public class UserLogin
{
    [Required]
    public string Login { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;
}