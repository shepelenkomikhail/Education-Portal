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
        public ActionResult Create()
        {
            var skills = skillService.GetAll();
            var skillModels = skills.Select(s => new SkillModel() { Id = s.Id, Name = s.Name });
            
            var materials = materialService.GetAll();
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
        public ActionResult Create(CourseModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new CourseDTO { Name = model.Name, Description = model.Description };
                var result = courseService.Insert(dto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to add course.");
            }
            return View(model);
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
