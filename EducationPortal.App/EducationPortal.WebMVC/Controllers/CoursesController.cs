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
            if (ModelState.IsValid)
            {
                if (model.SelectedSkillIds == null || !model.SelectedSkillIds.Any())
                {
                    ModelState.AddModelError("SelectedSkillIds", "Please select at least one skill");
                }
                
                if (model.SelectedMaterialIds == null || !model.SelectedMaterialIds.Any())
                {
                    ModelState.AddModelError("SelectedMaterialIds", "Please select at least one material");
                }
                
                if (ModelState.IsValid)
                {
                    var courseDto = new CourseDTO { Name = model.Name, Description = model.Description };
                    bool result = await courseService.InsertWithRelationsAsync(courseDto, model.SelectedSkillIds, model.SelectedMaterialIds);
                    
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    
                    ModelState.AddModelError("", "Failed to create course with relationships");
                }
            }
            
            var skills = await skillService.GetAllAsync();
            var materials = await materialService.GetAllAsync();
            var viewModel = new CourseCreateViewModel
            {
                Name = model.Name,
                Description = model.Description,
                Skills = skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList()
            };
            
            return View(viewModel);
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
