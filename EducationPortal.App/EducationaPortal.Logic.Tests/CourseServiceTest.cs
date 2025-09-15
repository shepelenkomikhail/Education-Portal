using System.Linq.Expressions;
using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using EducationPortal.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace EducationaPortal.Service.Tests;

public class CourseServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<Course, int>> mockRepository;
    private readonly Mock<IUserService> mockUserService;
    private readonly Mock<ISkillService> mockSkillService;
    private readonly Mock<IMaterialService> mockMaterialService;
    private readonly Mock<IDbContextTransaction> mockTransaction;
    private readonly CourseService courseService;

    public CourseServiceTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockRepository = new Mock<IRepository<Course, int>>();
        mockUserService = new Mock<IUserService>();
        mockSkillService = new Mock<ISkillService>();
        mockMaterialService = new Mock<IMaterialService>();
        mockTransaction = new Mock<IDbContextTransaction>();
        
        mockUnitOfWork.Setup(u => u.Repository<Course, int>()).Returns(mockRepository.Object);
        mockUnitOfWork.Setup(u => u.Repository<Skill, int>()).Returns(new Mock<IRepository<Skill, int>>().Object);
        mockUnitOfWork.Setup(u => u.Repository<Material, int>()).Returns(new Mock<IRepository<Material, int>>().Object);
        mockUnitOfWork.Setup(u => u.BeginTransaction()).Returns(mockTransaction.Object);
        
        courseService = new CourseService(mockUnitOfWork.Object, mockUserService.Object, mockSkillService.Object, mockMaterialService.Object);
    }

    [Fact]
    public async Task InsertAsync_ValidCourse_ReturnsTrue()
    {
        var courseDto = new CourseDTO
        {
            Name = "Test Course",
            Description = "Test Description",
            CreatedByUserId = 1
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await courseService.InsertAsync(courseDto);

        Assert.True(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Course>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_SaveFails_ReturnsFalse()
    {
        var courseDto = new CourseDTO
        {
            Name = "Test Course",
            Description = "Test Description",
            CreatedByUserId = 1
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await courseService.InsertAsync(courseDto);

        Assert.False(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Course>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingCourse_ReturnsTrue()
    {
        var courseDto = new CourseDTO
        {
            Id = 1,
            Name = "Updated Course",
            Description = "Updated Description"
        };

        var existingCourse = new Course
        {
            Id = 1,
            Name = "Original Course",
            Description = "Original Description"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingCourse);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await courseService.UpdateAsync(courseDto);

        Assert.True(result);
        Assert.Equal("Updated Course", existingCourse.Name);
        Assert.Equal("Updated Description", existingCourse.Description);
        mockRepository.Verify(r => r.UpdateAsync(existingCourse), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentCourse_ReturnsFalse()
    {
        var courseDto = new CourseDTO
        {
            Id = 999,
            Name = "Non-existent Course",
            Description = "Non-existent Description"
        };

        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        var result = await courseService.UpdateAsync(courseDto);

        Assert.False(result);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Course>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await courseService.DeleteAsync(1);

        Assert.True(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsFalse()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await courseService.DeleteAsync(1);

        Assert.False(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingCourse_ReturnsCourseDto()
    {
        var course = new Course
        {
            Id = 1,
            Name = "Test Course",
            Description = "Test Description",
            CreatedByUserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(course);

        var result = await courseService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Course", result.Name);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(1, result.CreatedByUserId);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentCourse_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        var result = await courseService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByNameAsync_ExistingCourse_ReturnsCourseDto()
    {
        var course = new Course
        {
            Id = 1,
            Name = "Test Course",
            Description = "Test Description",
            CreatedByUserId = 1,
            CreatedAt = DateTime.UtcNow
        };

        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(course);

        var result = await courseService.GetByNameAsync("Test Course");

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Course", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_NonExistentCourse_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync((Course?)null);

        var result = await courseService.GetByNameAsync("Non-existent Course");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCourses()
    {
        var courses = new List<Course>
        {
            new Course { Id = 1, Name = "Course 1", Description = "Description 1", CreatedByUserId = 1, CreatedAt = DateTime.UtcNow },
            new Course { Id = 2, Name = "Course 2", Description = "Description 2", CreatedByUserId = 2, CreatedAt = DateTime.UtcNow }
        };

        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(courses);

        var result = await courseService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Name == "Course 1");
        Assert.Contains(result, c => c.Name == "Course 2");
    }

    [Fact]
    public async Task GetAllAsync_NoCourses_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Course, bool>>>()))
            .ReturnsAsync(new List<Course>());

        var result = await courseService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddSkillToCourseAsync_ValidCourseAndSkill_ReturnsTrue()
    {
        var course = new Course
        {
            Id = 1,
            Name = "Test Course",
            Skills = new List<Skill>()
        };

        var skill = new Skill
        {
            Id = 1,
            Name = "Test Skill"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(course);
        mockUnitOfWork.Setup(u => u.Repository<Skill, int>()).Returns(new Mock<IRepository<Skill, int>>().Object);
        mockUnitOfWork.Setup(u => u.Repository<Skill, int>().GetByIdAsync(1)).ReturnsAsync(skill);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await courseService.AddSkillToCourseAsync(1, 1);

        Assert.True(result);
        Assert.Single(course.Skills);
        Assert.Equal(1, course.Skills.First().Id);
        mockRepository.Verify(r => r.UpdateAsync(course), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddSkillToCourseAsync_NonExistentCourse_ReturnsFalse()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        var result = await courseService.AddSkillToCourseAsync(999, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddSkillToCourseAsync_NonExistentSkill_ReturnsFalse()
    {
        var course = new Course
        {
            Id = 1,
            Name = "Test Course",
            Skills = new List<Skill>()
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(course);
        mockUnitOfWork.Setup(u => u.Repository<Skill, int>()).Returns(new Mock<IRepository<Skill, int>>().Object);
        mockUnitOfWork.Setup(u => u.Repository<Skill, int>().GetByIdAsync(999)).ReturnsAsync((Skill?)null);

        var result = await courseService.AddSkillToCourseAsync(1, 999);

        Assert.False(result);
    }

    [Fact]
    public async Task AddMaterialToCourseAsync_ValidCourseAndMaterial_ReturnsTrue()
    {
        var course = new Course
        {
            Id = 1,
            Name = "Test Course",
            Materials = new List<Material>()
        };

        var material = new Book
        {
            Id = 1,
            Title = "Test Book"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(course);
        mockUnitOfWork.Setup(u => u.Repository<Material, int>()).Returns(new Mock<IRepository<Material, int>>().Object);
        mockUnitOfWork.Setup(u => u.Repository<Material, int>().GetByIdAsync(1)).ReturnsAsync(material);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await courseService.AddMaterialToCourseAsync(1, 1);

        Assert.True(result);
        Assert.Single(course.Materials);
        Assert.Equal(1, course.Materials.First().Id);
        mockRepository.Verify(r => r.UpdateAsync(course), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddMaterialToCourseAsync_NonExistentCourse_ReturnsFalse()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        var result = await courseService.AddMaterialToCourseAsync(999, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddMaterialToCourseAsync_NonExistentMaterial_ReturnsFalse()
    {
        var course = new Course
        {
            Id = 1,
            Name = "Test Course",
            Materials = new List<Material>()
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(course);
        mockUnitOfWork.Setup(u => u.Repository<Material, int>()).Returns(new Mock<IRepository<Material, int>>().Object);
        mockUnitOfWork.Setup(u => u.Repository<Material, int>().GetByIdAsync(999)).ReturnsAsync((Material?)null);

        var result = await courseService.AddMaterialToCourseAsync(1, 999);

        Assert.False(result);
    }


    [Fact]
    public async Task GetCourseCreateDataAsync_ReturnsData()
    {
        var skills = new List<SkillDTO>
        {
            new SkillDTO { Id = 1, Name = "Skill 1" },
            new SkillDTO { Id = 2, Name = "Skill 2" }
        };

        var materials = new List<MaterialDTO>
        {
            new BookDTO { Id = 1, Title = "Book 1" },
            new VideoDTO { Id = 2, Title = "Video 1" }
        };

        mockSkillService.Setup(s => s.GetAllAsync()).ReturnsAsync(skills);
        mockMaterialService.Setup(m => m.GetAllAsync()).ReturnsAsync(materials);

        var result = await courseService.GetCourseCreateDataAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Skills.Count);
        Assert.Equal(2, result.Materials.Count);
        Assert.Contains(result.Skills, s => s.Name == "Skill 1");
        Assert.Contains(result.Materials, m => m.Title == "Book 1");
    }
}
