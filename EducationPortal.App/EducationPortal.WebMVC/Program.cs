using EducationPortal.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.Interfaces;
using EducationPortal.Logic.Services;
using Microsoft.AspNetCore.DataProtection;
using EducationPortal.WebMVC.Services;

namespace EducationPortal.WebMVC;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<PortalDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        
        builder.Services.AddControllersWithViews();
        
        builder.Services.AddDataProtection()
            .PersistKeysToDbContext<PortalDbContext>()
            .SetApplicationName("EducationPortal");

        builder.Services
            .AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<PortalDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
        builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, Services.CustomUserClaimsPrincipalFactory>();
        
        builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMaterialService, MaterialService>();
        builder.Services.AddScoped<ISkillService, SkillService>();

        builder.Services.AddScoped<AdminInitializationService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
            db.Database.Migrate();
            
            var adminService = scope.ServiceProvider.GetRequiredService<AdminInitializationService>();
            await adminService.InitializeAdminAsync();
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}