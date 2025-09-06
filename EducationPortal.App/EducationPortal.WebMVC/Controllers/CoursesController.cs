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
        public async Task<ActionResult> Index()
        {
            var courses = await courseService.GetAllAsync();
            var courseModels = courses.Select(c => new CourseModel 
            { 
                Id = c.Id, 
                Name = c.Name, 
                Description = c.Description 
            }).ToList();
            
            return View(courseModels);
        }

        // GET: CoursesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            var skills = await courseService.GetSkillsForCourse(id);
            var materials = await courseService.GetMaterialsForCourse(id);
            var books = materials.OfType<BookDTO>().ToList();
            var articles = materials.OfType<ArticleDTO>().ToList();
            var videos = materials.OfType<VideoDTO>().ToList();
            var courseModel = new CourseDetailsModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Skills = skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList(),
                Books = books.Select(b => new BookModel { Id = b.Id, Title = b.Title, Author = b.Author, PageAmount = b.PageAmount, Formant = b.Formant, PublicationDate = b.PublicationDate }).ToList(),
                Articles = articles.Select(a => new ArticleModel { Id = a.Id, Title = a.Title, Date = a.Date, Resource = a.Resource }).ToList(),
                Videos = videos.Select(v => new VideoModel { Id = v.Id, Title = v.Title, Duration = v.Duration, Quality = v.Quality }).ToList()
            };
            return View(courseModel);
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
            
            if (model.NewSkills != null)
            {
                foreach (var newSkill in model.NewSkills)
                {
                    if (!string.IsNullOrWhiteSpace(newSkill?.Name))
                    {
                        var skillDto = new SkillDTO { Name = newSkill.Name };
                        var skillCreated = await skillService.InsertAsync(skillDto);
                        if (skillCreated)
                        {
                            var createdSkill = await skillService.GetByNameAsync(newSkill.Name);
                            if (createdSkill != null)
                            {
                                selectedSkillIds.Add(createdSkill.Id);
                            }
                        }
                    }
                }
            }
            
            if (model.NewMaterials != null)
            {
                foreach (var newMaterial in model.NewMaterials)
                {
                    if (!string.IsNullOrWhiteSpace(newMaterial?.Title) && !string.IsNullOrWhiteSpace(newMaterial?.MaterialType))
                    {
                        var materialDto = CreateMaterialDto(newMaterial);
                        if (materialDto != null)
                        {
                            var materialCreated = await materialService.InsertAsync(materialDto);
                            if (materialCreated)
                            {
                                var createdMaterial = await materialService.GetByTitleAsync(newMaterial.Title);
                                if (createdMaterial != null)
                                {
                                    selectedMaterialIds.Add(createdMaterial.Id);
                                }
                            }
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
                NewSkills = model.NewSkills ?? new List<NewSkillModel>(),
                NewMaterials = model.NewMaterials ?? new List<NewMaterialModel>()
            };
            return View(viewModel);
        }

        private MaterialDTO? CreateMaterialDto(NewMaterialModel newMaterial)
        {
            if (string.IsNullOrWhiteSpace(newMaterial.MaterialType) || string.IsNullOrWhiteSpace(newMaterial.Title))
                return null;
                
            return newMaterial.MaterialType.ToLower() switch
            {
                "book" => new BookDTO
                {
                    Title = newMaterial.Title ?? string.Empty,
                    Author = newMaterial.Author ?? string.Empty,
                    PageAmount = newMaterial.PageAmount ?? 0,
                    Formant = newMaterial.Formant ?? string.Empty,
                    PublicationDate = newMaterial.PublicationDate ?? DateTime.Now
                },
                "video" => new VideoDTO
                {
                    Title = newMaterial.Title ?? string.Empty,
                    Duration = newMaterial.Duration ?? 0,
                    Quality = newMaterial.Quality ?? 0
                },
                "article" => new ArticleDTO
                {
                    Title = newMaterial.Title ?? string.Empty,
                    Date = newMaterial.Date ?? DateTime.Now,
                    Resource = newMaterial.Resource ?? string.Empty
                },
                _ => null
            };
        }

        // GET: CoursesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var skills = await skillService.GetAllAsync();
            var materials = await materialService.GetAllAsync();
            var courseSkills = await courseService.GetSkillsForCourse(id);
            var courseMaterials = await courseService.GetMaterialsForCourse(id);
            
            var viewModel = new CourseCreateViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Skills = skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList(),
                SelectedSkillIds = courseSkills.Select(s => s.Id).ToList(),
                SelectedMaterialIds = courseMaterials.Select(m => m.Id).ToList(),
                NewSkills = new List<NewSkillModel>(),
                NewMaterials = new List<NewMaterialModel>()
            };
            
            return View(viewModel);
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CourseCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var selectedSkillIds = new List<int>(model.SelectedSkillIds ?? new List<int>());
            var selectedMaterialIds = new List<int>(model.SelectedMaterialIds ?? new List<int>());
            
            if (model.NewSkills != null)
            {
                foreach (var newSkill in model.NewSkills)
                {
                    if (!string.IsNullOrWhiteSpace(newSkill?.Name))
                    {
                        var skillDto = new SkillDTO { Name = newSkill.Name };
                        var skillCreated = await skillService.InsertAsync(skillDto);
                        if (skillCreated)
                        {
                            var createdSkill = await skillService.GetByNameAsync(newSkill.Name);
                            if (createdSkill != null)
                            {
                                selectedSkillIds.Add(createdSkill.Id);
                            }
                        }
                    }
                }
            }
            
            if (model.NewMaterials != null)
            {
                foreach (var newMaterial in model.NewMaterials)
                {
                    if (!string.IsNullOrWhiteSpace(newMaterial?.Title) && !string.IsNullOrWhiteSpace(newMaterial?.MaterialType))
                    {
                        var materialDto = CreateMaterialDto(newMaterial);
                        if (materialDto != null)
                        {
                            var materialCreated = await materialService.InsertAsync(materialDto);
                            if (materialCreated)
                            {
                                var createdMaterial = await materialService.GetByTitleAsync(newMaterial.Title);
                                if (createdMaterial != null)
                                {
                                    selectedMaterialIds.Add(createdMaterial.Id);
                                }
                            }
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
                var courseDto = new CourseDTO { Id = model.Id, Name = model.Name, Description = model.Description };
                bool result = await courseService.UpdateWithRelationsAsync(courseDto, selectedSkillIds, selectedMaterialIds);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to update course");
            }
            
            var skills = await skillService.GetAllAsync();
            var materials = await materialService.GetAllAsync();
            var viewModel = new CourseCreateViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Skills = skills.Select(s => new SkillModel { Id = s.Id, Name = s.Name }).ToList(),
                Materials = materials.Select(m => new MaterialModel { Id = m.Id, Title = m.Title }).ToList(),
                NewSkills = model.NewSkills ?? new List<NewSkillModel>(),
                NewMaterials = model.NewMaterials ?? new List<NewMaterialModel>()
            };
            return View(viewModel);
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
                courseService.DeleteAsync(id);
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
                var material = await materialService.GetByIdAsync(materialId);
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
    }
}
