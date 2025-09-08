using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class ArticleService: IArticleService
{
    private readonly IUnitOfWork unitOfWork;
    
    public ArticleService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(ArticleDTO article)
    {
        var articleEntity = new Article
        {
            Title = article.Title,
            Date = article.Date,
            Resource = article.Resource
        };
        await unitOfWork.Repository<Article, int>().InsertAsync(articleEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(ArticleDTO article)
    {
        var existingArticle = await unitOfWork.Repository<Article, int>().GetByIdAsync(article.Id);
        if (existingArticle == null) return false;
        
        existingArticle.Title = article.Title;
        existingArticle.Date = article.Date;
        existingArticle.Resource = article.Resource;
        await unitOfWork.Repository<Article, int>().UpdateAsync(existingArticle);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<Article, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<ArticleDTO?> GetByIdAsync(int id)
    {
        var article = await unitOfWork.Repository<Article, int>().GetByIdAsync(id);
        return article != null ? new ArticleDTO(article) : null;
    }

    public async Task<IEnumerable<ArticleDTO>> GetAllAsync()
    {
        var articles = await unitOfWork.Repository<Article, int>()
            .GetWhereAsync(a => true);
        return articles.Select(a => new ArticleDTO(a)).ToList();
    }


}