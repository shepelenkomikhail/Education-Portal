using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using EducationPortal.WebMVC.Models;
using WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EducationPortal.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebMVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly ISkillService skillService;
        private readonly IMaterialService materialService;
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public CoursesController(ICourseService courseService, IMaterialService materialService, ISkillService skillService, IUserService userService, UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.materialService = materialService;
            this.skillService = skillService;
            this.userService = userService;
            this.userManager = userManager;
        }

        // GET: CoursesController
        public async Task<ActionResult> Index()
        {
            bool isAdmin = false;
            int? currentUserId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                isAdmin = User.IsInRole("Admin");
                var user = await userManager.GetUserAsync(User);
                currentUserId = user?.Id;
            }

            var data = await courseService.GetCoursesWithPermissionsAsync(currentUserId, isAdmin);

            var viewModel = new CourseIndexViewModel
            {
                Courses = data.Courses.Select(c => new CourseWithPermissionsModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedByUserId = c.CreatedByUserId,
                    CanEdit = c.CanEdit
                }).ToList(),
                IsAdmin = data.IsAdmin,
                CurrentUserId = data.CurrentUserId
            };

            return View(viewModel);
        }

        // GET: CoursesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                userId = user?.Id;
            }

            var data = await courseService.GetCourseDetailsWithUserDataAsync(id, userId);

            var courseModel = new CourseDetailsModel
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                Skills = data.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = data.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList(),
                Books = data.Books.Select(b => new BookModel { Id = b.Id, Title = b.Title, Author = b.Author, PageAmount = b.PageAmount, Formant = b.Formant, PublicationDate = b.PublicationDate }).ToList(),
                Articles = data.Articles.Select(a => new ArticleModel { Id = a.Id, Title = a.Title, Date = a.Date, Resource = a.Resource }).ToList(),
                Videos = data.Videos.Select(v => new VideoModel { Id = v.Id, Title = v.Title, Duration = v.Duration, Quality = v.Quality }).ToList(),
                IsUserEnrolled = data.IsUserEnrolled,
                CompletionPercentage = data.CompletionPercentage,
                CompletedMaterialIds = data.CompletedMaterialIds
            };
            return View(courseModel);
        }

        // GET: CoursesController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var data = await courseService.GetCourseCreateDataAsync();
            var viewModel = new CourseCreateViewModel
            {
                Skills = data.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = data.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList()
            };
            return View(viewModel);
        }

        // POST: CoursesController/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CourseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var fallbackData = await courseService.GetCourseCreateDataAsync();
                model.Skills = fallbackData.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList();
                model.Materials = fallbackData.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList();
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found. Please log in again.");
                return View(model);
            }

            var request = new CourseCreateRequestDTO
            {
                Name = model.Name,
                Description = model.Description,
                CreatedByUserId = user.Id,
                SelectedSkillIds = model.SelectedSkillIds ?? new List<int>(),
                SelectedMaterialIds = model.SelectedMaterialIds ?? new List<int>(),
                NewSkills = model.NewSkills?.Select(ns => new NewSkillRequestDTO { Name = ns.Name }).ToList() ?? new List<NewSkillRequestDTO>(),
                NewMaterials = model.NewMaterials?.Select(nm => new NewMaterialRequestDTO
                {
                    Title = nm.Title ?? string.Empty,
                    MaterialType = nm.MaterialType ?? string.Empty,
                    Author = nm.Author,
                    PageAmount = nm.PageAmount,
                    Formant = nm.Formant,
                    PublicationDate = nm.PublicationDate,
                    Duration = nm.Duration,
                    Quality = nm.Quality,
                    Date = nm.Date,
                    Resource = nm.Resource
                }).ToList() ?? new List<NewMaterialRequestDTO>()
            };

            var result = await courseService.CreateCourseWithNewItemsAsync(request);

            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.ValidationErrors)
            {
                ModelState.AddModelError("", error);
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }

            if (result.FallbackData != null)
            {
                model.Skills = result.FallbackData.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList();
                model.Materials = result.FallbackData.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList();
            }

            return View(model);
        }



        // GET: CoursesController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            bool isAdmin = User.IsInRole("Admin");

            try
            {
                var data = await courseService.GetCourseEditDataAsync(id, user.Id, isAdmin);

                var viewModel = new CourseCreateViewModel
                {
                    Id = data.Course.Id,
                    Name = data.Course.Name,
                    Description = data.Course.Description,
                    Skills = data.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                    Materials = data.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList(),
                    SelectedSkillIds = data.CourseSkills.Select(s => s.Id).ToList(),
                    SelectedMaterialIds = data.CourseMaterials.Select(m => m.Id).ToList(),
                    NewSkills = new List<NewSkillModel>(),
                    NewMaterials = new List<NewMaterialModel>()
                };

                return View(viewModel);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CourseCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.GetUserAsync(User);
                    if (user == null) return Unauthorized();

                    bool isAdmin = User.IsInRole("Admin");
                    var fallbackData = await courseService.GetCourseEditDataAsync(id, user.Id, isAdmin);

                    model.Skills = fallbackData.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList();
                    model.Materials = fallbackData.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList();
                }
                catch
                {
                    return NotFound();
                }
                return View(model);
            }

            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var request = new CourseEditRequestDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                UserId = currentUser.Id,
                IsAdmin = User.IsInRole("Admin"),
                SelectedSkillIds = model.SelectedSkillIds ?? new List<int>(),
                SelectedMaterialIds = model.SelectedMaterialIds ?? new List<int>(),
                NewSkills = model.NewSkills?.Select(ns => new NewSkillRequestDTO { Name = ns.Name }).ToList() ?? new List<NewSkillRequestDTO>(),
                NewMaterials = model.NewMaterials?.Select(nm => new NewMaterialRequestDTO
                {
                    Title = nm.Title ?? string.Empty,
                    MaterialType = nm.MaterialType ?? string.Empty,
                    Author = nm.Author,
                    PageAmount = nm.PageAmount,
                    Formant = nm.Formant,
                    PublicationDate = nm.PublicationDate,
                    Duration = nm.Duration,
                    Quality = nm.Quality,
                    Date = nm.Date,
                    Resource = nm.Resource
                }).ToList() ?? new List<NewMaterialRequestDTO>()
            };

            var result = await courseService.UpdateCourseWithNewItemsAsync(request);

            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.ValidationErrors)
            {
                ModelState.AddModelError("", error);
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }

            if (result.FallbackData != null)
            {
                model.Skills = result.FallbackData.Skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList();
                model.Materials = result.FallbackData.Materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList();
            }

            return View(model);
        }

        // GET: CoursesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CoursesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await courseService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CoursesController/GetMaterialDetails
        [HttpGet]
        public async Task<ActionResult> GetMaterialDetails(string materialType, int materialId)
        {
            try
            {
                var material = await courseService.GetMaterialDetailsAsync(materialId, materialType);
                if (material == null)
                {
                    return PartialView("_ErrorMessage", "Material not found.");
                }

                return materialType.ToLower() switch
                {
                    "book" when material is BookDTO bookDto => PartialView("_BookDetails", new BookModel
                    {
                        Id = bookDto.Id,
                        Title = bookDto.Title,
                        Author = bookDto.Author,
                        PageAmount = bookDto.PageAmount,
                        Formant = bookDto.Formant,
                        PublicationDate = bookDto.PublicationDate
                    }),
                    "video" when material is VideoDTO videoDto => PartialView("_VideoDetails", new VideoModel
                    {
                        Id = videoDto.Id,
                        Title = videoDto.Title,
                        Duration = videoDto.Duration,
                        Quality = videoDto.Quality
                    }),
                    "article" when material is ArticleDTO articleDto => PartialView("_ArticleDetails", new ArticleModel
                    {
                        Id = articleDto.Id,
                        Title = articleDto.Title,
                        Date = articleDto.Date,
                        Resource = articleDto.Resource
                    }),
                    _ => PartialView("_ErrorMessage", "Unsupported material type.")
                };
            }
            catch (Exception)
            {
                return PartialView("_ErrorMessage", "Error loading material details.");
            }
        }

        // POST: CoursesController/Enroll/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll(int id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                var course = await courseService.GetByIdAsync(id);
                if (course == null)
                {
                    return Json(new { success = false, message = "Course not found." });
                }
                
                if (await userService.IsUserEnrolledInCourseAsync(user.Id, id))
                {
                    return Json(new { success = false, message = "You are already enrolled in this course." });
                }
                
                bool enrollmentResult = await userService.EnrollUserInCourseAsync(user.Id, id);

                return Json(enrollmentResult ? new { success = true, message = "Successfully enrolled in the course!" }
                    : new { success = false, message = "Failed to enroll in the course. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while enrolling in the course." });
            }
        }

        // POST: CoursesController/MarkMaterialCompleted
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkMaterialCompleted(int courseId, int materialId)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }
                
                if (!await userService.IsUserEnrolledInCourseAsync(user.Id, courseId))
                {
                    return Json(new { success = false, message = "You are not enrolled in this course." });
                }
                
                bool result = await userService.MarkMaterialAsCompletedAsync(user.Id, courseId, materialId);

                if (result)
                {
                    int newProgress = await userService.GetCourseProgressAsync(user.Id, courseId);
                    string message = "Material marked as completed!";
                    if (newProgress == 100)
                    {
                        message = "🎉 Congratulations! You've completed this course and earned new skills!";
                    }

                    return Json(new {
                        success = true,
                        message = message,
                        newProgress = newProgress,
                        courseCompleted = newProgress == 100
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to mark material as completed." });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while updating progress." });
            }
        }

        // POST: CoursesController/UpdateProgress
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProgress(int courseId, int progress)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }
                
                if (!await userService.IsUserEnrolledInCourseAsync(user.Id, courseId))
                {
                    return Json(new { success = false, message = "You are not enrolled in this course." });
                }
                
                bool result = await userService.UpdateCourseProgressAsync(user.Id, courseId, progress);

                return Json(result ? new { success = true, message = "Progress updated successfully!" } 
                    : new { success = false, message = "Failed to update progress." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while updating progress." });
            }
        }
    }
}
