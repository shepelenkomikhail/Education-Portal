using WebMVC.Models;

namespace EducationPortal.WebMVC.Models;

public class BookModel : MaterialModel
{
    public string Author { get; set; } = string.Empty;
    public int PageAmount { get; set; }
    public string Formant { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
}