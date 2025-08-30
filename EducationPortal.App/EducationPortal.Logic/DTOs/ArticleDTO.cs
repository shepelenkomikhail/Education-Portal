using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class ArticleDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Resource { get; set; } = string.Empty;
    
    public ArticleDTO(){}

    internal ArticleDTO(Article? article)
    {
        ArgumentNullException.ThrowIfNull(article, nameof(article));
        
        Id = article.Id;
        Title = article.Title;
        Date = article.Date;
        Resource = article.Resource;
    }

    internal Article ToArticle()
    {
        return new Article() 
        { 
            Id = this.Id, 
            Title = this.Title, 
            Date = this.Date, 
            Resource = this.Resource 
        };
    }
}