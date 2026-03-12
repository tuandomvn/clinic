using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Clinic.Services.Services.Auth;
using Clinic.Services.Models.Auth;
using System.Security.Claims;

namespace Clinic.Web.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IAuthService authService, ILogger<LoginModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [BindProperty]
    public LoginRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        // If user is already authenticated, redirect to home
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Vui lòng nhập tên đăng nhập và mật khẩu.";
            return Page();
        }

        try
        {
            var result = await _authService.LoginAsync(Input);

            if (result == null)
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", Input.Username);
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                return Page();
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.StaffId.ToString()),
                new Claim(ClaimTypes.Name, result.Username),
                new Claim(ClaimTypes.Role, result.Role),
                new Claim("AccessToken", result.AccessToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = result.ExpiresAtUtc
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("User {Username} logged in successfully at {LoginTime}", Input.Username, DateTime.UtcNow);

            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/")
            {
                return Redirect("/");
            }

            // If returnUrl is a valid local URL, redirect to it
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for username: {Username}", Input.Username);
            ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.";
            return Page();
        }
    }
}
