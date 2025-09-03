using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class MaterialService: IMaterialService
{
    private readonly IUnitOfWork unitOfWork;
    
    public MaterialService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(MaterialDTO material)
    {
        Material materialEntity;
        
        if (material is BookDTO bookDto)
        {
            materialEntity = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                PageAmount = bookDto.PageAmount,
                Formant = bookDto.Formant,
                PublicationDate = bookDto.PublicationDate
            };
            await unitOfWork.Repository<Book, int>().InsertAsync((Book)materialEntity);
        }
        else if (material is VideoDTO videoDto)
        {
            materialEntity = new Video
            {
                Title = videoDto.Title,
                Duration = videoDto.Duration,
                Quality = videoDto.Quality
            };
            await unitOfWork.Repository<Video, int>().InsertAsync((Video)materialEntity);
        }
        else if (material is ArticleDTO articleDto)
        {
            materialEntity = new Article
            {
                Title = articleDto.Title,
                Date = articleDto.Date,
                Resource = articleDto.Resource
            };
            await unitOfWork.Repository<Article, int>().InsertAsync((Article)materialEntity);
        }
        else
        {
            return false;
        }
        
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(MaterialDTO material)
    {
        var book = await unitOfWork.Repository<Book, int>().GetByIdAsync(material.Id);
        if (book != null)
        {
            book.Title = material.Title;
            await unitOfWork.Repository<Book, int>().UpdateAsync(book);
            return await unitOfWork.SaveAsync();
        }

        var video = await unitOfWork.Repository<Video, int>().GetByIdAsync(material.Id);
        if (video != null)
        {
            video.Title = material.Title;
            await unitOfWork.Repository<Video, int>().UpdateAsync(video);
            return await unitOfWork.SaveAsync();
        }

        var article = await unitOfWork.Repository<Article, int>().GetByIdAsync(material.Id);
        if (article != null)
        {
            article.Title = material.Title;
            await unitOfWork.Repository<Article, int>().UpdateAsync(article);
            return await unitOfWork.SaveAsync();
        }

        return false;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await unitOfWork.Repository<Book, int>().GetByIdAsync(id);
        if (book != null)
        {
            await unitOfWork.Repository<Book, int>().DeleteAsync(book);
            return await unitOfWork.SaveAsync();
        }

        var video = await unitOfWork.Repository<Video, int>().GetByIdAsync(id);
        if (video != null)
        {
            await unitOfWork.Repository<Video, int>().DeleteAsync(video);
            return await unitOfWork.SaveAsync();
        }

        var article = await unitOfWork.Repository<Article, int>().GetByIdAsync(id);
        if (article != null)
        {
            await unitOfWork.Repository<Article, int>().DeleteAsync(article);
            return await unitOfWork.SaveAsync();
        }

        return false;
    }

    public async Task<MaterialDTO?> GetByIdAsync(int id)
    {
        var book = await unitOfWork.Repository<Book, int>().GetByIdAsync(id);
        if (book != null) return new MaterialDTO(book);

        var video = await unitOfWork.Repository<Video, int>().GetByIdAsync(id);
        if (video != null) return new MaterialDTO(video);

        var article = await unitOfWork.Repository<Article, int>().GetByIdAsync(id);
        if (article != null) return new MaterialDTO(article);

        return null;
    }

    public async Task<IEnumerable<MaterialDTO>> GetAllAsync()
    {
        var allMaterials = new List<MaterialDTO>();
        var books = await unitOfWork.Repository<Book, int>().GetWhereAsync(b => true);
        allMaterials.AddRange(books.Select(b => new MaterialDTO(b)));
        var videos = await unitOfWork.Repository<Video, int>().GetWhereAsync(v => true);
        allMaterials.AddRange(videos.Select(v => new MaterialDTO(v)));
        var articles = await unitOfWork.Repository<Article, int>().GetWhereAsync(a => true);
        allMaterials.AddRange(articles.Select(a => new MaterialDTO(a)));

        return allMaterials;
    }

    public async Task<IEnumerable<MaterialDTO>> GetByTypeAsync(string materialType)
    {
        return materialType.ToLower() switch
        {
            "book" => (await unitOfWork.Repository<Book, int>().GetWhereAsync(b => true))
                .Select(b => new MaterialDTO(b)),
            "video" => (await unitOfWork.Repository<Video, int>().GetWhereAsync(v => true))
                .Select(v => new MaterialDTO(v)),
            "article" => (await unitOfWork.Repository<Article, int>().GetWhereAsync(a => true))
                .Select(a => new MaterialDTO(a)),
            _ => new List<MaterialDTO>()
        };
    }
}