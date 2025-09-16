using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Logic.Services;

public class UserService: IUserService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly PortalDbContext context;

    public UserService(IUnitOfWork unitOfWork, PortalDbContext context)
    {
        this.unitOfWork = unitOfWork;
        this.context = context;
    }
    
    public async Task<bool> InsertAsync(UserDTO user)
    {
        var existingUser = await unitOfWork.Repository<User, int>()
            .GetSingleOrDefaultAsync(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber);

        if (existingUser != null)
        {
            return false;
        }

        var userEntity = new User()
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        await unitOfWork.Repository<User, int>().InsertAsync(userEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(UserDTO user)
    {
        var existingUser = await unitOfWork.Repository<User, int>().GetByIdAsync(user.Id);
        if (existingUser == null) return false;

        existingUser.UserName = user.UserName;
        existingUser.FirstName = user.FirstName;
        existingUser.Surname = user.Surname;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        await unitOfWork.Repository<User, int>().UpdateAsync(existingUser);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<User, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        var user = await unitOfWork.Repository<User, int>().GetByIdAsync(id);
        return user != null ? new UserDTO(user) : null;
    }

    public async Task<UserDTO?> GetByEmailAsync(string email)
    {
        var user = await unitOfWork.Repository<User, int>().GetSingleOrDefaultAsync(u => u.Email == email);
        return user != null ? new UserDTO(user) : null;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        var users = await unitOfWork.Repository<User, int>().GetWhereAsync(u => true);
        return users.Select(u => new UserDTO(u)).ToList();
    }

    public async Task<UserDTO?> GetUserWithSkillsAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? new UserDTO(user) : null;
    }

    public async Task<UserDTO?> GetUserWithCoursesAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserCourses)
            .ThenInclude(uc => uc.Course)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? new UserDTO(user) : null;
    }

    public async Task<UserDTO?> GetUserWithSkillsAndCoursesAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .Include(u => u.UserCourses)
            .ThenInclude(uc => uc.Course)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? new UserDTO(user) : null;
    }

    public async Task<bool> EnrollUserInCourseAsync(int userId, int courseId)
    {
        if (await IsUserEnrolledInCourseAsync(userId, courseId))
        {
            return false;
        }
        
        var user = await unitOfWork.Repository<User, int>().GetByIdAsync(userId);
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);

        if (user == null || course == null)
        {
            return false;
        }

        var userCourse = new UserCourse
        {
            UserId = userId,
            CourseId = courseId,
            CompletionPercentage = 0
        };
        
        user.UserCourses.Add(userCourse);
        await unitOfWork.Repository<User, int>().UpdateAsync(user);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserCourses)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.UserCourses.Any(uc => uc.CourseId == courseId) ?? false;
    }

    public async Task<int> GetCourseProgressAsync(int userId, int courseId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserCourses)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var userCourse = user?.UserCourses.FirstOrDefault(uc => uc.CourseId == courseId);
        return userCourse?.CompletionPercentage ?? 0;
    }

    public async Task<bool> UpdateCourseProgressAsync(int userId, int courseId, int completionPercentage)
    {
        completionPercentage = Math.Max(0, Math.Min(100, completionPercentage));

        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserCourses)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return false;

        var userCourse = user.UserCourses.FirstOrDefault(uc => uc.CourseId == courseId);
        if (userCourse == null) return false;

        userCourse.CompletionPercentage = completionPercentage;
        await unitOfWork.Repository<User, int>().UpdateAsync(user);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> MarkMaterialAsCompletedAsync(int userId, int courseId, int materialId)
    {
        if (!await IsUserEnrolledInCourseAsync(userId, courseId))
        {
            return false;
        }
        
        if (await IsMaterialCompletedAsync(userId, materialId))
        {
            return true; 
        }
        
        var user = await unitOfWork.Repository<User, int>().GetByIdAsync(userId);
        var material = await unitOfWork.Repository<Material, int>().GetByIdAsync(materialId);

        if (user == null || material == null)
        {
            return false;
        }
        
        var userMaterial = new UserMaterial
        {
            UserId = userId,
            MaterialId = materialId,
            CompletedAt = DateTime.UtcNow,
            IsCompleted = true
        };

        user.UserMaterials.Add(userMaterial);
        
        await unitOfWork.Repository<User, int>().UpdateAsync(user);
        await CalculateAndUpdateCourseProgressAsync(userId, courseId);

        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> IsMaterialCompletedAsync(int userId, int materialId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserMaterials)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.UserMaterials.Any(um => um.MaterialId == materialId && um.IsCompleted) ?? false;
    }

    private async Task CalculateAndUpdateCourseProgressAsync(int userId, int courseId)
    {
        var course = await unitOfWork.Repository<Course, int>()
            .GetQueryable()
            .Include(c => c.Materials)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null || !course.Materials.Any())
        {
            return;
        }
        
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserMaterials)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return;
        }

        var courseMaterialIds = course.Materials.Select(m => m.Id).ToList();
        var completedMaterialIds = user.UserMaterials
            .Where(um => um.IsCompleted && courseMaterialIds.Contains(um.MaterialId))
            .Select(um => um.MaterialId)
            .ToList();
        
        int totalMaterials = courseMaterialIds.Count;
        int completedMaterials = completedMaterialIds.Count;
        int progressPercentage = totalMaterials > 0 ? (completedMaterials * 100) / totalMaterials : 0;
        
        await UpdateCourseProgressAsync(userId, courseId, progressPercentage);
        
        if (progressPercentage == 100)
        {
            await AssignCourseSkillsToUserAsync(userId, courseId);
        }
    }

    public async Task<bool> AssignCourseSkillsToUserAsync(int userId, int courseId)
    {
        try
        {
            var course = await unitOfWork.Repository<Course, int>()
                .GetQueryable()
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null || !course.Skills.Any())
            {
                return true; 
            }
            
            var user = await unitOfWork.Repository<User, int>()
                .GetQueryable()
                .Include(u => u.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }
            
            bool skillsAdded = false;
            foreach (var skill in course.Skills)
            {
                var existingUserSkill = user.UserSkills.FirstOrDefault(us => us.SkillId == skill.Id);

                if (existingUserSkill == null)
                {
                    var userSkill = new UserSkill
                    {
                        UserId = userId,
                        SkillId = skill.Id,
                        SkillLevel = 1
                    };

                    user.UserSkills.Add(userSkill);
                }
                else
                {
                    existingUserSkill.SkillLevel++;
                }

                skillsAdded = true;
            }

            if (skillsAdded)
            {
                await unitOfWork.Repository<User, int>().UpdateAsync(user);
                return await unitOfWork.SaveAsync();
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<UserDashboardDTO> GetUserDashboardDataAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
            .Include(u => u.UserCourses)
                .ThenInclude(uc => uc.Course)
                    .ThenInclude(c => c.Materials)
            .Include(u => u.CreatedCourses)
                .ThenInclude(c => c.Materials)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new UserDashboardDTO();
        }

        var dashboard = new UserDashboardDTO
        {
            UserInfo = new UserInfoDTO
            {
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email ?? string.Empty
            },
            UserSkills = user.UserSkills.Select(us => new UserSkillDTO
            {
                SkillName = us.Skill.Name,
                SkillLevel = us.SkillLevel
            }).ToList()
        };
        
        var enrolledCourses = new List<UserCourseProgressDTO>();
        var inProgressCourses = new List<UserCourseProgressDTO>();
        var completedCourses = new List<UserCourseProgressDTO>();

        foreach (var userCourse in user.UserCourses)
        {
            var completedMaterials = await context.UserMaterials
                .Where(um => um.UserId == userId &&
                           userCourse.Course.Materials.Select(m => m.Id).Contains(um.MaterialId) &&
                           um.IsCompleted)
                .CountAsync();

            var courseProgress = new UserCourseProgressDTO
            {
                CourseId = userCourse.CourseId,
                CourseName = userCourse.Course.Name,
                CourseDescription = userCourse.Course.Description,
                CompletionPercentage = userCourse.CompletionPercentage,
                TotalMaterials = userCourse.Course.Materials.Count,
                CompletedMaterials = completedMaterials
            };

            enrolledCourses.Add(courseProgress);
            
            if (userCourse.CompletionPercentage == 100)
            {
                completedCourses.Add(courseProgress);
            }
            else if (userCourse.CompletionPercentage > 0)
            {
                inProgressCourses.Add(courseProgress);
            }
        }

        dashboard.EnrolledCourses = enrolledCourses;
        dashboard.InProgressCourses = inProgressCourses;
        dashboard.CompletedCourses = completedCourses;
        
        var createdCourses = new List<CreatedCourseDTO>();
        foreach (var course in user.CreatedCourses)
        {
            var enrolledCount = await context.UserCourses
                .Where(uc => uc.CourseId == course.Id)
                .CountAsync();

            createdCourses.Add(new CreatedCourseDTO
            {
                CourseId = course.Id,
                CourseName = course.Name,
                CourseDescription = course.Description,
                CreatedAt = course.CreatedAt,
                EnrolledStudents = enrolledCount,
                TotalMaterials = course.Materials.Count
            });
        }

        dashboard.CreatedCourses = createdCourses;

        return dashboard;
    }
}