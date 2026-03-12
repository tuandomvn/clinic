using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Clinic.Web.Pages.Auth;

public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        var username = User.Identity?.Name ?? "Unknown";
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        _logger.LogInformation("User {Username} logged out at {LogoutTime}", username, DateTime.UtcNow);

        return RedirectToPage("/Auth/Login");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        // Auto-logout when accessing this page directly
        var username = User.Identity?.Name ?? "Unknown";
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        _logger.LogInformation("User {Username} logged out at {LogoutTime}", username, DateTime.UtcNow);

        return RedirectToPage("/Auth/Login");
    }
}
