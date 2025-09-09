using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EducationPortal.WebMVC.Models;
using EducationPortal.Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using EducationPortal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.WebMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly PortalDbContext _context;

    public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, PortalDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel();

        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                viewModel.IsAuthenticated = true;
                viewModel.FirstName = user.FirstName;
                viewModel.Surname = user.Surname;
                viewModel.Email = user.Email ?? string.Empty;

                var userSkills = await _context.UserSkills
                    .Include(us => us.Skill)
                    .Where(us => us.UserId == user.Id)
                    .ToListAsync();

                viewModel.UserSkills = userSkills.Select(us => new UserSkillViewModel
                {
                    SkillName = us.Skill.Name,
                    SkillLevel = us.SkillLevel
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