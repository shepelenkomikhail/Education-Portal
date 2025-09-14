using Microsoft.AspNetCore.Identity;
using EducationPortal.Data.Models;

namespace EducationPortal.WebMVC.Services;

public class AdminInitializationHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AdminInitializationHostedService> _logger;

    public AdminInitializationHostedService(
        IServiceProvider serviceProvider,
        ILogger<AdminInitializationHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting admin initialization...");

        using var scope = _serviceProvider.CreateScope();
        var adminService = scope.ServiceProvider.GetRequiredService<AdminInitializationService>();
        
        await adminService.InitializeAdminAsync();
        
        _logger.LogInformation("Admin initialization completed.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
