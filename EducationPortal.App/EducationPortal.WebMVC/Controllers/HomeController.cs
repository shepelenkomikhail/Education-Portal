using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EducationPortal.WebMVC.Models;
using EducationPortal.Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using EducationPortal.Data.Models;

namespace EducationPortal.WebMVC.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly IUserService userService;

    public HomeController(UserManager<User> userManager, IUserService userService)
    {
        this.userManager = userManager;
        this.userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel();

        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                viewModel.IsAuthenticated = true;
                viewModel.IsAdmin = User.IsInRole("Admin");
                
                var dashboardData = await userService.GetUserDashboardDataAsync(user.Id);
                
                viewModel.FirstName = dashboardData.UserInfo.FirstName;
                viewModel.Surname = dashboardData.UserInfo.Surname;
                viewModel.Email = dashboardData.UserInfo.Email;

                viewModel.UserSkills = dashboardData.UserSkills.Select(us => new UserSkillViewModel
                {
                    SkillName = us.SkillName,
                    SkillLevel = us.SkillLevel
                }).ToList();

                viewModel.EnrolledCourses = dashboardData.EnrolledCourses.Select(ec => new UserCourseProgressViewModel
                {
                    CourseId = ec.CourseId,
                    CourseName = ec.CourseName,
                    CourseDescription = ec.CourseDescription,
                    CompletionPercentage = ec.CompletionPercentage,
                    TotalMaterials = ec.TotalMaterials,
                    CompletedMaterials = ec.CompletedMaterials
                }).ToList();

                viewModel.InProgressCourses = dashboardData.InProgressCourses.Select(ic => new UserCourseProgressViewModel
                {
                    CourseId = ic.CourseId,
                    CourseName = ic.CourseName,
                    CourseDescription = ic.CourseDescription,
                    CompletionPercentage = ic.CompletionPercentage,
                    TotalMaterials = ic.TotalMaterials,
                    CompletedMaterials = ic.CompletedMaterials
                }).ToList();

                viewModel.CompletedCourses = dashboardData.CompletedCourses.Select(cc => new UserCourseProgressViewModel
                {
                    CourseId = cc.CourseId,
                    CourseName = cc.CourseName,
                    CourseDescription = cc.CourseDescription,
                    CompletionPercentage = cc.CompletionPercentage,
                    TotalMaterials = cc.TotalMaterials,
                    CompletedMaterials = cc.CompletedMaterials
                }).ToList();

                viewModel.CreatedCourses = dashboardData.CreatedCourses.Select(cc => new CreatedCourseViewModel
                {
                    CourseId = cc.CourseId,
                    CourseName = cc.CourseName,
                    CourseDescription = cc.CourseDescription,
                    CreatedAt = cc.CreatedAt,
                    EnrolledStudents = cc.EnrolledStudents,
                    TotalMaterials = cc.TotalMaterials
                }).ToList();
            }
        }

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}