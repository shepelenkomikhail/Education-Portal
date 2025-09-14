using Microsoft.AspNetCore.Identity;
using EducationPortal.Data.Models;

namespace EducationPortal.WebMVC.Services;

public class AdminInitializationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ILogger<AdminInitializationService> _logger;

    public AdminInitializationService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        ILogger<AdminInitializationService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task InitializeAdminAsync()
    {
        try
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>("Admin"));
                _logger.LogInformation("Admin role created successfully.");
            }

            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@educationportal.com",
                    FirstName = "Admin",
                    Surname = "User",
                    EmailConfirmed = true,
                    PhoneNumber = "+1234567890"
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                
                if (result.Succeeded)
                {
                    adminUser = await _userManager.FindByNameAsync("admin");
                    if (adminUser != null)
                    {
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                        _logger.LogInformation("Admin user created successfully with username: admin and password: Admin123!");
                    }
                }
                else
                {
                    _logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(adminUser);
                var resetResult = await _userManager.ResetPasswordAsync(adminUser, token, "Admin123!");
                
                if (resetResult.Succeeded)
                {
                    _logger.LogInformation("Admin password reset successfully.");
                }
                else
                {
                    _logger.LogError("Failed to reset admin password: {Errors}", string.Join(", ", resetResult.Errors.Select(e => e.Description)));
                }
                
                if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("Admin role assigned to existing admin user.");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing admin user and role.");
        }
    }
}
