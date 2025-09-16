using System.Linq.Expressions;
using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Services;
using Moq;

namespace EducationaPortal.Service.Tests;

public class SkillServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<Skill, int>> mockRepository;
    private readonly SkillService skillService;

    public SkillServiceTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockRepository = new Mock<IRepository<Skill, int>>();
        mockUnitOfWork.Setup(u => u.Repository<Skill, int>()).Returns(mockRepository.Object);
        skillService = new SkillService(mockUnitOfWork.Object);
    }

    [Fact]
    public async Task InsertAsync_ValidSkill_ReturnsTrue()
    {
        var skillDto = new SkillDTO
        {
            Name = "Test Skill"
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await skillService.InsertAsync(skillDto);

        Assert.True(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Skill>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_SaveFails_ReturnsFalse()
    {
        var skillDto = new SkillDTO
        {
            Name = "Test Skill"
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await skillService.InsertAsync(skillDto);

        Assert.False(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Skill>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingSkill_ReturnsTrue()
    {
        var skillDto = new SkillDTO
        {
            Id = 1,
            Name = "Updated Skill"
        };

        var existingSkill = new Skill
        {
            Id = 1,
            Name = "Original Skill"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingSkill);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await skillService.UpdateAsync(skillDto);

        Assert.True(result);
        Assert.Equal("Updated Skill", existingSkill.Name);
        mockRepository.Verify(r => r.UpdateAsync(existingSkill), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentSkill_ReturnsFalse()
    {
        var skillDto = new SkillDTO
        {
            Id = 999,
            Name = "Non-existent Skill"
        };

        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Skill?)null);

        var result = await skillService.UpdateAsync(skillDto);

        Assert.False(result);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Skill>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await skillService.DeleteAsync(1);

        Assert.True(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsFalse()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await skillService.DeleteAsync(1);

        Assert.False(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingSkill_ReturnsSkillDto()
    {
        var skill = new Skill
        {
            Id = 1,
            Name = "Test Skill"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(skill);

        var result = await skillService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Skill", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentSkill_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Skill?)null);

        var result = await skillService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByNameAsync_ExistingSkill_ReturnsSkillDto()
    {
        var skill = new Skill
        {
            Id = 1,
            Name = "Test Skill"
        };

        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<Skill, bool>>>()))
            .ReturnsAsync(skill);

        var result = await skillService.GetByNameAsync("Test Skill");

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Skill", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_NonExistentSkill_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<Skill, bool>>>()))
            .ReturnsAsync((Skill?)null);

        var result = await skillService.GetByNameAsync("Non-existent Skill");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSkills()
    {
        var skills = new List<Skill>
        {
            new Skill { Id = 1, Name = "Skill 1" },
            new Skill { Id = 2, Name = "Skill 2" }
        };

        mockRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Skill, bool>>>()))
            .ReturnsAsync(skills);

        var result = await skillService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.Name == "Skill 1");
        Assert.Contains(result, s => s.Name == "Skill 2");
    }

    [Fact]
    public async Task GetAllAsync_NoSkills_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Skill, bool>>>()))
            .ReturnsAsync(new List<Skill>());

        var result = await skillService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}