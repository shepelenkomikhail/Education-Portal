using EducationPortal.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.Interfaces;
using EducationPortal.Logic.Services;
using Microsoft.AspNetCore.DataProtection;

namespace EducationPortal.WebMVC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<PortalDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<PortalDbContext>();
        builder.Services.AddControllersWithViews();
        
        builder.Services.AddDataProtection()
            .PersistKeysToDbContext<PortalDbContext>()
            .SetApplicationName("EducationPortal");
        
        builder.Services.AddScoped<PortalDbContext>();

        // Register the new generic UnitOfWork pattern
        builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMaterialService, MaterialService>();
        builder.Services.AddScoped<ISkillService, SkillService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
            db.Database.Migrate();
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