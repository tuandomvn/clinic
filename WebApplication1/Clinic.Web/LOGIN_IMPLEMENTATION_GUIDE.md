# Login Feature Implementation Guide

## Overview
This document describes the authentication and login feature implemented for the Clinic.Web application using cookie-based authentication with ASP.NET Core Identity patterns.

## Architecture

### Components

1. **Authentication Service** (`IAuthService`)
   - Handles user credential validation
   - Authenticates users against the `UserAccounts` table
   - Returns login response with user details

2. **Password Hashing** (`IPasswordHasher`)
   - Securely hashes and verifies passwords using BCrypt
   - Never stores plain text passwords

3. **JWT Token Service** (`IJwtTokenService`)
   - Creates JWT access tokens for authenticated users
   - Sets token expiration time

4. **Cookie-Based Authentication**
   - Uses ASP.NET Core's `CookieAuthenticationScheme`
   - Secure HTTP-only cookies
   - 8-hour session expiration with sliding window

## Features

### ✅ Implemented Features

1. **User Login Page** (`/Auth/Login`)
   - Clean, responsive UI with gradient design
   - Username and password input validation
   - Error message display
   - Loading indicator during submission
   - Auto-focus on username field

2. **Logout Functionality** (`/Auth/Logout`)
   - Clears authentication cookie
   - Logs logout activity
   - Redirects to login page

3. **Access Control** (`/Auth/AccessDenied`)
   - Shows error message for unauthorized access
   - Provides navigation options

4. **Navigation Bar Authentication**
   - Shows login button for unauthenticated users
   - Shows user dropdown with name and role for authenticated users
   - Logout button in user dropdown

5. **Authorization Attributes**
   - Pages can be protected with `[Authorize]` attribute
   - Default protection applied to Index page

6. **Database Seeding**
   - Automatically creates admin user on first run
   - Default credentials: `admin` / `admin123`
   - Can be used to create additional user accounts

## Database Schema

### UserAccount Table
```sql
CREATE TABLE UserAccounts (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    StaffId INT NOT NULL UNIQUE,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(500) NOT NULL,
    Role VARCHAR(50) NOT NULL DEFAULT 'User',
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME,
    LastLoginAt DATETIME,
    FOREIGN KEY (StaffId) REFERENCES Staff(Id)
);
```

## Usage

### Default Admin Login
```
Username: admin
Password: admin123
```

⚠️ **IMPORTANT**: Change the default password immediately in production!

### Creating New User Accounts

#### Option 1: Using DatabaseSeeder Service
```csharp
await DatabaseSeeder.CreateUserAccountAsync(
    serviceProvider,
    staffId: 1,
    username: "doctor1",
    password: "SecurePassword123!",
    role: "Doctor"
);
```

#### Option 2: SQL Script
```sql
INSERT INTO UserAccounts (StaffId, Username, PasswordHash, Role, IsActive)
VALUES (1, 'doctor1', '<hashed_password>', 'Doctor', 1);
```

### Protecting Pages

Add the `[Authorize]` attribute to page model:
```csharp
[Authorize]
public class SecretPageModel : PageModel
{
    // Only authenticated users can access this page
}
```

### Role-Based Authorization

Restrict access by role:
```csharp
[Authorize(Roles = "Admin,Doctor")]
public class AdminOnlyModel : PageModel
{
    // Only Admin and Doctor roles can access
}
```

## Security Considerations

### ✅ Implemented Security Measures

1. **Password Hashing**
   - Uses BCrypt algorithm
   - Prevents password compromise even if database is breached

2. **Secure Cookies**
   - `HttpOnly` flag prevents JavaScript access
   - `Secure` flag requires HTTPS
   - SameSite policy prevents CSRF attacks

3. **Session Management**
   - 8-hour expiration with sliding window
   - Automatic logout after inactivity

4. **Input Validation**
   - Server-side validation of credentials
   - Prevents SQL injection via Entity Framework

5. **Audit Logging**
   - Login attempts are logged
   - LastLoginAt timestamp updated on successful login

### ⚠️ Recommended Changes for Production

1. **Change Default Admin Password**
   ```csharp
   // In DatabaseSeeder.cs, change from:
   PasswordHash = hasher.Hash("admin123")
   // To something secure:
   PasswordHash = hasher.Hash("YourSecurePassword123!")
   ```

2. **Enable HTTPS Only**
   ```csharp
   // In Program.cs
   options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   ```

3. **Add CSRF Protection**
   ```html
   <!-- Add to login form -->
   <input type="hidden" asp-antiforgery="true" />
   ```

4. **Implement Account Lockout**
   ```csharp
   // Track failed login attempts
   // Lock account after N attempts
   ```

5. **Add Two-Factor Authentication (2FA)**
   - Email or SMS verification
   - Time-based one-time passwords (TOTP)

## File Structure

```
Clinic.Web/
├── Pages/
│   ├── Auth/
│   │   ├── Login.cshtml
│   │   ├── Login.cshtml.cs
│   │   ├── Logout.cshtml.cs
│   │   ├── AccessDenied.cshtml
│   │   └── AccessDenied.cshtml.cs
│   ├── Index.cshtml
│   └── Index.cshtml.cs (updated with [Authorize])
├── Services/
│   └── DatabaseSeeder.cs (new)
├── Shared/
│   └── _Layout.cshtml (updated)
└── Program.cs (updated)

Clinic.Services/
├── Services/
│   └── Auth/
│       ├── AuthService.cs
│       ├── IAuthService.cs
│       ├── PasswordHasher.cs
│       ├── IPasswordHasher.cs
│       ├── JwtTokenService.cs
│       └── IJwtTokenService.cs
└── Models/
    └── Auth/
        ├── LoginRequest.cs
        └── LoginResponse.cs
```

## Testing

### Manual Testing Checklist

- [ ] Navigate to `/Auth/Login` - should display login page
- [ ] Enter invalid credentials - should show error message
- [ ] Enter valid credentials (`admin` / `admin123`) - should login successfully
- [ ] Access protected page (e.g., `/Index`) - should redirect to login if not authenticated
- [ ] Click logout - should clear session and redirect to login
- [ ] Try accessing protected page after logout - should be redirected to login

### Automated Testing (Optional)

```csharp
[TestClass]
public class AuthenticationTests
{
    [TestMethod]
    public async Task Login_WithValidCredentials_ReturnsSuccessResponse()
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

## Troubleshooting

### Common Issues

#### 1. "Login page keeps redirecting"
- **Cause**: Not properly authenticated after login
- **Solution**: Check `AuthService.LoginAsync()` returns valid data, verify password is hashed correctly

#### 2. "Cookie not being set"
- **Cause**: HTTPS not enforced in development
- **Solution**: Add to Program.cs:
  ```csharp
  options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
  ```

#### 3. "User claims not available"
- **Cause**: Claims not added during sign-in
- **Solution**: Check `Login.cshtml.cs` - ensure all claims are added in the ClaimsList

#### 4. "Password hash doesn't match"
- **Cause**: Using different hasher or salt
- **Solution**: Ensure same `IPasswordHasher` implementation is used for hashing and verification

## Next Steps

1. ✅ Implement login functionality (DONE)
2. Create user management admin page
3. Add password reset functionality
4. Implement two-factor authentication (2FA)
5. Add role-based access control (RBAC) pages
6. Create audit log viewer
7. Add email notifications for login attempts

## References

- [ASP.NET Core Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)
- [Cookie Authentication in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie)
- [Authorization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/introduction)
- [Password Hashing Best Practices](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html)
