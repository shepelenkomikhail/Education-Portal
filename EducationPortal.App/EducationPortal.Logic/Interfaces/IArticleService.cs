using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IArticleService
{
    Task<bool> InsertAsync(ArticleDTO article);
    Task<bool> UpdateAsync(ArticleDTO article);
    Task<bool> DeleteAsync(int id);
    Task<ArticleDTO?> GetByIdAsync(int id);
    Task<IEnumerable<ArticleDTO>> GetAllAsync();
}