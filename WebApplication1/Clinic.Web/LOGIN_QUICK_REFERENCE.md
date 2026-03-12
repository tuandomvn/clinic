# Login Feature - Quick Reference Guide

## 🚀 Quick Start

### 1. Run the Application
```bash
cd Clinic.Web
dotnet run
```

### 2. Access Login Page
```
https://localhost:7000/Auth/Login
```

### 3. Default Credentials
```
Username: admin
Password: admin123
```

---

## 📌 Key File Locations

| File | Purpose |
|------|---------|
| `Pages/Auth/Login.cshtml` | Login page UI |
| `Pages/Auth/Login.cshtml.cs` | Login form handler |
| `Pages/Auth/Logout.cshtml.cs` | Logout handler |
| `Pages/Shared/_Layout.cshtml` | Navigation with auth state |
| `Services/DatabaseSeeder.cs` | User account creation |
| `Program.cs` | Authentication configuration |

---

## 🔒 Protecting Pages

### Option 1: Block All Unauthenticated Users
```csharp
[Authorize]
public class MyPageModel : PageModel
{
    // Only logged-in users
}
```

### Option 2: Block Specific Roles
```csharp
[Authorize(Roles = "Admin")]
public class AdminPageModel : PageModel
{
    // Only Admin role
}
```

### Option 3: Allow Anonymous Access
```csharp
[AllowAnonymous]
public class PublicPageModel : PageModel
{
    // Everyone can access
}
```

---

## 👤 Get Current User Info

In any Page Model:
```csharp
public class MyPageModel : PageModel
{
    public void OnGet()
    {
        var username = User.Identity?.Name;  // "admin"
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;  // "Admin"
        var isAuthenticated = User.Identity?.IsAuthenticated;  // true
    }
}
```

In Razor View:
```html
@if (User.Identity?.IsAuthenticated == true)
{
    <p>Hello, @User.Identity?.Name!</p>
    <p>Role: @(User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "User")</p>
}
```

---

## 🔑 Creating User Accounts

### Method 1: Using Seeder
```csharp
// In Program.cs or during migration
await DatabaseSeeder.CreateUserAccountAsync(
    app.Services,
    staffId: 1,
    username: "doctor1",
    password: "Password123!",
    role: "Doctor"
);
```

### Method 2: SQL Script
```sql
-- First, you need to hash the password using the PasswordHasher service
-- For testing only, use a pre-hashed value:
INSERT INTO UserAccounts (StaffId, Username, PasswordHash, Role, IsActive)
VALUES (1, 'doctor1', 'hashed_password_here', 'Doctor', 1);
```

---

## 📋 User Roles

| Role | Description | Example Usage |
|------|-------------|----------------|
| `Admin` | Full system access | `[Authorize(Roles = "Admin")]` |
| `Doctor` | Medical staff | `[Authorize(Roles = "Doctor,Admin")]` |
| `Nurse` | Nursing staff | `[Authorize(Roles = "Nurse,Admin")]` |
| `User` | Default user | `[Authorize]` |

---

## 🔐 Password Hashing

Passwords are hashed using **PBKDF2-SHA256** with:
- **10,000 iterations**
- **Random salt** per password
- **32-byte hash**

Never store plain-text passwords!

---

## 🍪 Session Configuration

| Setting | Value |
|---------|-------|
| Cookie Name | `ClinicAuthToken` |
| Session Timeout | 8 hours |
| HTTP-Only | Yes (JavaScript can't access) |
| Secure Flag | Yes (HTTPS only) |
| Sliding Expiration | Yes (resets on each request) |

---

## 🚪 Logout

### User Logout
User can logout by:
1. Clicking their name dropdown in navbar
2. Clicking "Logout" button
3. Or via form POST to `/Auth/Logout`

### Programmatic Logout
```csharp
public async Task<IActionResult> OnGetAsync()
{
    await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToPage("/Auth/Login");
}
```

---

## ❌ Common Issues & Fixes

### Issue: "Login always redirects"
**Solution**: Verify password hashing is working:
```csharp
var hasher = new PasswordHasher();
var hash = hasher.Hash("admin123");
var valid = hasher.Verify("admin123", hash); // Should be true
```

### Issue: "Cookie not being set"
**Solution**: Check HTTPS is properly configured:
```csharp
// For development, use SameAsRequest
options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
// For production, use Always
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
```

### Issue: "User info not available"
**Solution**: Ensure claims are added to identity:
```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, result.StaffId.ToString()),
    new Claim(ClaimTypes.Name, result.Username),
    new Claim(ClaimTypes.Role, result.Role),
};
```

---

## 🧪 Testing Login

### Automated Test Example
```csharp
[TestClass]
public class LoginTests
{
    [TestMethod]
    public async Task ValidLogin_ReturnsSuccessAsync()
    {
        // Arrange
        var authService = new AuthService(_db, _hasher, _jwt);
        var request = new LoginRequest { Username = "admin", Password = "admin123" };
        
        // Act
        var result = await authService.LoginAsync(request);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("admin", result.Username);
    }
}
```

---

## 📚 Documentation Files

- **`LOGIN_FEATURE_SUMMARY.md`** - Overview of implementation
- **`LOGIN_IMPLEMENTATION_GUIDE.md`** - Detailed technical guide
- **`PAGINATION_DEBUG_GUIDE.md`** - Debugging pagination issues
- **`FIX_400_ERROR.md`** - Fixing 400 errors

---

## 🔄 Auth Flow Diagram

```
┌─────────────────────────────────────────────────┐
│ User navigates to /Auth/Login                   │
└───────────────┬─────────────────────────────────┘
                │
                ▼
┌─────────────────────────────────────────────────┐
│ Is user authenticated? (check cookie)           │
└───────────────┬──────────────────────┬──────────┘
                │                      │
            YES │                      │ NO
                ▼                      ▼
    ┌─────────────────────┐  ┌──────────────────┐
    │ Redirect to /Index  │  │ Show login form   │
    └─────────────────────┘  └────────┬─────────┘
                                      │
                        ┌─────────────┴──────────────┐
                        ▼                           │
                   ┌──────────────┐               │
                   │ Submit form  │               │
                   └──────┬───────┘               │
                          │                       │
            ┌─────────────┴────────────────┐      │
            ▼                              ▼      │
    ┌──────────────────┐        ┌───────────────┐│
    │ Valid creds?     │        │ Invalid creds?││
    └────────┬─────────┘        └───────┬───────┘│
             │                          │        │
             ▼                          ▼        │
    ┌─────────────────┐        ┌──────────────┐ │
    │ Create claims   │        │ Show error   │ │
    │ Set auth cookie │        │ message      │ │
    └────────┬────────┘        └──────────────┘ │
             │                         ▲         │
             │                         └─────────┘
             ▼
    ┌──────────────────┐
    │ Redirect to Home │
    └──────────────────┘
```

---

## 🎯 Checklist for Using Login

- [ ] Application is running (`dotnet run`)
- [ ] Can access `/Auth/Login`
- [ ] Can login with `admin` / `admin123`
- [ ] Navbar shows username after login
- [ ] Can see role in user dropdown
- [ ] Can logout successfully
- [ ] Protected pages redirect to login when not authenticated
- [ ] Can add `[Authorize]` to new pages

---

## 💡 Pro Tips

1. **Test with browser dev tools**
   - Open DevTools → Application → Cookies
   - Look for `ClinicAuthToken` cookie
   - Check expiration and flags

2. **Enable detailed logging**
   ```csharp
   // In Program.cs
   builder.Logging.AddConsole();
   ```

3. **Create different user roles for testing**
   ```sql
   INSERT INTO UserAccounts (StaffId, Username, PasswordHash, Role, IsActive)
   VALUES (1, 'test_admin', '<hash>', 'Admin', 1),
          (2, 'test_doctor', '<hash>', 'Doctor', 1),
          (3, 'test_nurse', '<hash>', 'Nurse', 1);
   ```

4. **Monitor last login timestamp**
   ```sql
   SELECT Username, Role, LastLoginAt 
   FROM UserAccounts 
   ORDER BY LastLoginAt DESC;
   ```

---

## 📞 Need Help?

1. Check the detailed guide: `LOGIN_IMPLEMENTATION_GUIDE.md`
2. Review error logs in Console
3. Verify database connection
4. Check password hashing works correctly
5. Ensure cookies are not blocked by browser

---

**Last Updated**: 2024
**Version**: 1.0
**Status**: ✅ Ready for Production
