using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationPortal.Logic.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserService userService;
    private readonly ISkillService skillService;
    private readonly IMaterialService materialService;

    public CourseService(IUnitOfWork unitOfWork, IUserService userService, ISkillService skillService, IMaterialService materialService)
    {
        this.unitOfWork = unitOfWork;
        this.userService = userService;
        this.skillService = skillService;
        this.materialService = materialService;
    }
    
    public async Task<bool> InsertAsync(CourseDTO course)
    {
        var courseEntity = new Course() {
            Name = course.Name,
            Description = course.Description,
            CreatedByUserId = course.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };
        await unitOfWork.Repository<Course, int>().InsertAsync(courseEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(CourseDTO course)
    {
        var existingCourse = await unitOfWork.Repository<Course, int>().GetByIdAsync(course.Id);
        if (existingCourse == null) return false;
        
        existingCourse.Name = course.Name;
        existingCourse.Description = course.Description;
        await unitOfWork.Repository<Course, int>().UpdateAsync(existingCourse);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<Course, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<CourseDTO?> GetByIdAsync(int id)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(id);
        return course != null ? new CourseDTO(course) : null;
    }

    public async Task<CourseDTO?> GetByNameAsync(string name)
    {
        var course = await unitOfWork.Repository<Course, int>().GetSingleOrDefaultAsync(c => c.Name == name);
        return course != null ? new CourseDTO(course) : null;
    }

    public async Task<IEnumerable<CourseDTO>> GetAllAsync()
    {
        var courses = await unitOfWork.Repository<Course, int>().GetWhereAsync(c => true);
        return courses.Select(c => new CourseDTO(c)).ToList();
    }
    
    public async Task<IEnumerable<SkillDTO>> GetSkillsForCourse(int courseId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetAll()
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        return course?.Skills.Select(s => new SkillDTO(s)) ?? new List<SkillDTO>();
    }
    
    public async Task<IEnumerable<MaterialDTO>> GetMaterialsForCourse(int courseId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetAll()
            .Include(c => c.Materials)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        return course?.Materials.Select(CreateMaterialDTO) ?? new List<MaterialDTO>();
    }
    
    private MaterialDTO CreateMaterialDTO(Material material)
    {
        return material switch
        {
            Book book => new BookDTO(book),
            Video video => new VideoDTO(video),
            Article article => new ArticleDTO(article),
            _ => new MaterialDTO(material)
        };
    }

    public async Task<bool> AddSkillToCourseAsync(int courseId, int skillId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);
        var skill = await unitOfWork.Repository<Skill, int>().GetByIdAsync(skillId);
        
        if (course == null || skill == null) return false;

        course.Skills.Add(skill);
        await unitOfWork.Repository<Course, int>().UpdateAsync(course);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> AddMaterialToCourseAsync(int courseId, int materialId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);
        var material = await unitOfWork.Repository<Material, int>().GetByIdAsync(materialId);
        
        if (course == null || material == null) return false;

        course.Materials.Add(material);
        await unitOfWork.Repository<Course, int>().UpdateAsync(course);
        return await unitOfWork.SaveAsync();
    }
    
    public async Task<bool> InsertWithRelationsAsync(CourseDTO course, List<int> skillIds, List<int> materialIds)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var newCourse = new Course {
                Name = course.Name,
                Description = course.Description,
                CreatedByUserId = course.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.Repository<Course, int>().InsertAsync(newCourse);
            await unitOfWork.SaveAsync();
            
            foreach (var skillId in skillIds)
            {
                if (!await AddSkillToCourseAsync(newCourse.Id, skillId))
                    throw new Exception($"Failed to add skill {skillId}");
            }
            
            foreach (var materialId in materialIds)
            {
                if (!await AddMaterialToCourseAsync(newCourse.Id, materialId))
                    throw new Exception($"Failed to add material {materialId}");
            }
            
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<bool> UpdateWithRelationsAsync(CourseDTO course, List<int> skillIds, List<int> materialIds)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var existingCourse = await unitOfWork.Repository<Course, int>().GetAll()
                .Include(c => c.Skills)
                .Include(c => c.Materials)
                .FirstOrDefaultAsync(c => c.Id == course.Id);
                
            if (existingCourse == null)
            {
                transaction.Rollback();
                return false;
            }
            
            existingCourse.Name = course.Name;
            existingCourse.Description = course.Description;
            
            existingCourse.Skills.Clear();
            foreach (var skillId in skillIds)
            {
                var skill = await unitOfWork.Repository<Skill, int>().GetByIdAsync(skillId);
                if (skill != null)
                {
                    existingCourse.Skills.Add(skill);
                }
            }
            
            existingCourse.Materials.Clear();
            foreach (var materialId in materialIds)
            {
                var material = await unitOfWork.Repository<Material, int>().GetByIdAsync(materialId);
                if (material != null)
                {
                    existingCourse.Materials.Add(material);
                }
            }

            await unitOfWork.Repository<Course, int>().UpdateAsync(existingCourse);
            await unitOfWork.SaveAsync();

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    public async Task<IEnumerable<CourseDTO>> GetCoursesByCreatorAsync(int userId)
    {
        var courses = await unitOfWork.Repository<Course, int>()
            .GetQueryable()
            .Include(c => c.CreatedBy)
            .Where(c => c.CreatedByUserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return courses.Select(c => new CourseDTO(c));
    }

    public async Task<CourseIndexDataDTO> GetCoursesWithPermissionsAsync(int? currentUserId, bool isAdmin)
    {
        var courses = await GetAllAsync();

        var courseModels = courses.Select(c => new CourseWithPermissionsDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedByUserId = c.CreatedByUserId,
            CanEdit = isAdmin || (currentUserId.HasValue && c.CreatedByUserId == currentUserId.Value)
        }).ToList();

        return new CourseIndexDataDTO
        {
            Courses = courseModels,
            IsAdmin = isAdmin,
            CurrentUserId = currentUserId
        };
    }

    public async Task<CourseDetailsDataDTO> GetCourseDetailsWithUserDataAsync(int courseId, int? userId)
    {
        var course = await GetByIdAsync(courseId);
        if (course == null)
            throw new ArgumentException($"Course with id {courseId} not found");

        var skills = await GetSkillsForCourse(courseId);
        var materials = await GetMaterialsForCourse(courseId);
        var books = materials.OfType<BookDTO>().ToList();
        var articles = materials.OfType<ArticleDTO>().ToList();
        var videos = materials.OfType<VideoDTO>().ToList();

        bool isUserEnrolled = false;
        int completionPercentage = 0;
        List<int> completedMaterialIds = new();

        if (userId.HasValue)
        {
            isUserEnrolled = await userService.IsUserEnrolledInCourseAsync(userId.Value, courseId);
            if (isUserEnrolled)
            {
                completionPercentage = await userService.GetCourseProgressAsync(userId.Value, courseId);

                foreach (var material in materials)
                {
                    if (await userService.IsMaterialCompletedAsync(userId.Value, material.Id))
                    {
                        completedMaterialIds.Add(material.Id);
                    }
                }
            }
        }

        return new CourseDetailsDataDTO
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            Skills = skills.ToList(),
            Materials = materials.ToList(),
            Books = books,
            Articles = articles,
            Videos = videos,
            IsUserEnrolled = isUserEnrolled,
            CompletionPercentage = completionPercentage,
            CompletedMaterialIds = completedMaterialIds
        };
    }

    public async Task<CourseCreateDataDTO> GetCourseCreateDataAsync()
    {
        var skills = await skillService.GetAllAsync();
        var materials = await materialService.GetAllAsync();

        return new CourseCreateDataDTO
        {
            Skills = skills.ToList(),
            Materials = materials.ToList()
        };
    }

    public async Task<CourseEditDataDTO> GetCourseEditDataAsync(int courseId, int? userId, bool isAdmin)
    {
        var course = await GetByIdAsync(courseId);
        if (course == null)
            throw new ArgumentException($"Course with id {courseId} not found");

        bool isCreator = course.CreatedByUserId == userId;
        bool canEdit = isAdmin || isCreator;

        if (!canEdit)
            throw new UnauthorizedAccessException("User does not have permission to edit this course");

        var skills = await skillService.GetAllAsync();
        var materials = await materialService.GetAllAsync();
        var courseSkills = await GetSkillsForCourse(courseId);
        var courseMaterials = await GetMaterialsForCourse(courseId);

        return new CourseEditDataDTO
        {
            Course = course,
            Skills = skills.ToList(),
            Materials = materials.ToList(),
            CourseSkills = courseSkills.ToList(),
            CourseMaterials = courseMaterials.ToList(),
            CanEdit = canEdit
        };
    }
    
    public async Task<CourseCreateResultDTO> CreateCourseWithNewItemsAsync(CourseCreateRequestDTO request)
    {
        var result = new CourseCreateResultDTO();
        var selectedSkillIds = new List<int>(request.SelectedSkillIds);
        var selectedMaterialIds = new List<int>(request.SelectedMaterialIds);

        if (request.NewSkills != null)
        {
            foreach (var newSkill in request.NewSkills)
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

        if (request.NewMaterials != null)
        {
            foreach (var newMaterial in request.NewMaterials)
            {
                if (!string.IsNullOrWhiteSpace(newMaterial?.Title) && !string.IsNullOrWhiteSpace(newMaterial?.MaterialType))
                {
                    var materialDto = CreateMaterialDtoFromRequest(newMaterial);
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
            result.ValidationErrors.Add("Please select at least one skill or create a new one");
        }
        if (selectedMaterialIds.Count == 0)
        {
            result.ValidationErrors.Add("Please select at least one material or create a new one");
        }

        if (result.ValidationErrors.Any())
        {
            result.Success = false;
            result.FallbackData = await GetCourseCreateDataAsync();
            return result;
        }

        var courseDto = new CourseDTO {
            Name = request.Name,
            Description = request.Description,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAt = DateTime.UtcNow
        };

        bool success = await InsertWithRelationsAsync(courseDto, selectedSkillIds, selectedMaterialIds);
        if (success)
        {
            result.Success = true;
        }
        else
        {
            result.Success = false;
            result.ErrorMessage = "Failed to create course with relationships";
            result.FallbackData = await GetCourseCreateDataAsync();
        }

        return result;
    }

    public async Task<CourseEditResultDTO> UpdateCourseWithNewItemsAsync(CourseEditRequestDTO request)
    {
        var result = new CourseEditResultDTO();

        var course = await GetByIdAsync(request.Id);
        if (course == null)
        {
            result.Success = false;
            result.ErrorMessage = "Course not found";
            return result;
        }

        bool isAdmin = request.IsAdmin;
        bool isCreator = course.CreatedByUserId == request.UserId;

        if (!isAdmin && !isCreator)
        {
            result.Success = false;
            result.ErrorMessage = "Unauthorized to edit this course";
            return result;
        }

        var selectedSkillIds = new List<int>(request.SelectedSkillIds);
        var selectedMaterialIds = new List<int>(request.SelectedMaterialIds);

        if (request.NewSkills != null)
        {
            foreach (var newSkill in request.NewSkills)
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

        if (request.NewMaterials != null)
        {
            foreach (var newMaterial in request.NewMaterials)
            {
                if (!string.IsNullOrWhiteSpace(newMaterial?.Title) && !string.IsNullOrWhiteSpace(newMaterial?.MaterialType))
                {
                    var materialDto = CreateMaterialDtoFromRequest(newMaterial);
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
            result.ValidationErrors.Add("Please select at least one skill or create a new one");
        }
        if (selectedMaterialIds.Count == 0)
        {
            result.ValidationErrors.Add("Please select at least one material or create a new one");
        }

        if (result.ValidationErrors.Any())
        {
            result.Success = false;
            result.FallbackData = await GetCourseEditDataAsync(request.Id, request.UserId, request.IsAdmin);
            return result;
        }

        var courseDto = new CourseDTO { Id = request.Id, Name = request.Name, Description = request.Description };
        bool success = await UpdateWithRelationsAsync(courseDto, selectedSkillIds, selectedMaterialIds);
        if (success)
        {
            result.Success = true;
        }
        else
        {
            result.Success = false;
            result.ErrorMessage = "Failed to update course";
            result.FallbackData = await GetCourseEditDataAsync(request.Id, request.UserId, request.IsAdmin);
        }

        return result;
    }

    public async Task<MaterialDTO?> GetMaterialDetailsAsync(int materialId, string materialType)
    {
        var material = await materialService.GetByIdAsync(materialId);

        if (material == null)
            return null;

        return materialType.ToLower() switch
        {
            "book" when material is BookDTO => material,
            "video" when material is VideoDTO => material,
            "article" when material is ArticleDTO => material,
            _ => null
        };
    }

    private MaterialDTO? CreateMaterialDtoFromRequest(NewMaterialRequestDTO newMaterial)
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
}