using EducationPortal.WebMVC.Models;

namespace WebMVC.Models;

public class CourseDetailsModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SkillModel> Skills { get; set; } = new();
    public List<MaterialModel> Materials { get; set; } = new();
    public List<BookModel> Books { get; set; } = new();
    public List<ArticleModel> Articles { get; set; } = new();
    public List<VideoModel> Videos { get; set; } = new();
}