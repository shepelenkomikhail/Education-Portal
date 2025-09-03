using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using EducationPortal.WebMVC.Models;
using WebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly ISkillService skillService;
        private readonly IMaterialService materialService;

        public CoursesController(ICourseService courseService, IMaterialService materialService, ISkillService skillService)
        {
            this.courseService = courseService;
            this.materialService = materialService;
            this.skillService = skillService;
        }

        // GET: CoursesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CoursesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CoursesController/Create
        public async Task<ActionResult> Create()
        {
            var skills = await skillService.GetAllAsync();
            var skillModels = skills.Select(s => new SkillModel() { Id = s.Id, Name = s.Name });
            var materials = await materialService.GetAllAsync();
            var materialModels = materials.Select(m => new MaterialModel() { Id = m.Id, Title = m.Title });
            var viewModel = new CourseCreateViewModel
            {
                Skills = skillModels.ToList(),
                Materials = materialModels.ToList()
            };
            return View(viewModel);
        }

        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CourseCreateViewModel model)
        {
            var selectedSkillIds = new List<int>(model.SelectedSkillIds ?? new List<int>());
            var selectedMaterialIds = new List<int>(model.SelectedMaterialIds ?? new List<int>());
            
            if (!string.IsNullOrWhiteSpace(model.NewSkill?.Name))
            {
                var skillDto = new SkillDTO { Name = model.NewSkill.Name };
                var skillCreated = await skillService.InsertAsync(skillDto);
                if (skillCreated)
                {
                    var newSkill = await skillService.GetByNameAsync(model.NewSkill.Name);
                    if (newSkill != null)
                    {
                        selectedSkillIds.Add(newSkill.Id);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(model.NewMaterial?.Title) && !string.IsNullOrWhiteSpace(model.NewMaterial?.MaterialType))
            {
                var materialDto = CreateMaterialDto(model.NewMaterial);
                if (materialDto != null)
                {
                    var materialCreated = await materialService.InsertAsync(materialDto);
                    if (materialCreated)
                    {
                        var newMaterial = await materialService.GetByTitleAsync(model.NewMaterial.Title);
                        if (newMaterial != null)
                        {
                            selectedMaterialIds.Add(newMaterial.Id);
                        }
                    }
                }
            }
            
            if (selectedSkillIds.Count == 0)
            {
                ModelState.AddModelError("SelectedSkillIds", "Please select at least one skill or create a new one");
            }
            if (selectedMaterialIds.Count == 0)
            {
                ModelState.AddModelError("SelectedMaterialIds", "Please select at least one material or create a new one");
            }

            if (ModelState.IsValid)
            {
                var courseDto = new CourseDTO { Name = model.Name, Description = model.Description };
                bool result = await courseService.InsertWithRelationsAsync(courseDto, selectedSkillIds, selectedMaterialIds);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to create course with relationships");
            }
            
            var skills = await skillService.GetAllAsync();
            var materials = await materialService.GetAllAsync();
            var viewModel = new CourseCreateViewModel
            {
                Name = model.Name,
                Description = model.Description,
                Skills = skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList(),
                NewSkill = model.NewSkill,
                NewMaterial = model.NewMaterial
            };
            return View(viewModel);
        }

        private MaterialDTO? CreateMaterialDto(NewMaterialModel newMaterial)
        {
            return newMaterial.MaterialType.ToLower() switch
            {
                "book" => new BookDTO
                {
                    Title = newMaterial.Title,
                    Author = newMaterial.Author ?? string.Empty,
                    PageAmount = newMaterial.PageAmount ?? 0,
                    Formant = newMaterial.Formant ?? string.Empty,
                    PublicationDate = newMaterial.PublicationDate ?? DateTime.Now
                },
                "video" => new VideoDTO
                {
                    Title = newMaterial.Title,
                    Duration = newMaterial.Duration ?? 0,
                    Quality = newMaterial.Quality ?? 0
                },
                "article" => new ArticleDTO
                {
                    Title = newMaterial.Title,
                    Date = newMaterial.Date ?? DateTime.Now,
                    Resource = newMaterial.Resource ?? string.Empty
                },
                _ => null
            };
        }

        // GET: CoursesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CoursesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CoursesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
