using WebMVC.Models;

namespace EducationPortal.WebMVC.Models;

public class ArticleModel : MaterialModel
{
    public DateTime Date { get; set; }
    public string Resource { get; set; } = string.Empty;
}