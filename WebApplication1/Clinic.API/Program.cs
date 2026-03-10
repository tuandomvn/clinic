using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Clinic.API.Auth;
using Clinic.Services.Services.Auth;
using Clinic.Services.Services.Appointments;
using Clinic.Services.Services.Surgeries;
using Clinic.Services.Services.Prescriptions;

namespace Clinic.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ClinicDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
            builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<ISurgeryScheduleService, SurgeryScheduleService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

            var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT settings not found.");
            if (string.IsNullOrWhiteSpace(jwtOptions.SigningKey) || jwtOptions.SigningKey.StartsWith("CHANGE_ME", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Please set Jwt:SigningKey in appsettings.json to a strong secret.");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                // Require auth for all endpoints by default; opt-out with [AllowAnonymous].
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            var app = builder.Build();

            // Apply pending migrations and seed UserAccounts if empty
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
                db.Database.Migrate();
                var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                if (!db.UserAccounts.Any())
                {
                    var hash = hasher.Hash("Password@123");
                    var utc = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    db.UserAccounts.AddRange(
                        new UserAccount { StaffId = 1, Username = "doctor1", PasswordHash = hash, Role = "Doctor", IsActive = true, CreatedAt = utc },
                        new UserAccount { StaffId = 2, Username = "nurse1", PasswordHash = hash, Role = "Nurse", IsActive = true, CreatedAt = utc },
                        new UserAccount { StaffId = 3, Username = "doctor2", PasswordHash = hash, Role = "Doctor", IsActive = true, CreatedAt = utc },
                        new UserAccount { StaffId = 4, Username = "nurse2", PasswordHash = hash, Role = "Nurse", IsActive = true, CreatedAt = utc });
                    db.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi().AllowAnonymous();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
