using EducationPortal.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.Interfaces;
using EducationPortal.Logic.Services;

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
        
        builder.Services.AddScoped<PortalDbContext>();

        builder.Services.AddScoped<UnitOfWorkRepository>();
        builder.Services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
        builder.Services.AddScoped<ISkillRepository, SkillRepository>();

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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}