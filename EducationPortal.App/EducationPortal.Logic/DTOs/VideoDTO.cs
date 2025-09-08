using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class VideoDTO : MaterialDTO
{
    public int Duration { get; set; }
    public int Quality { get; set; }
    
    public VideoDTO(){}

    internal VideoDTO(Video? video)
    {
        ArgumentNullException.ThrowIfNull(video, nameof(video));
        
        Id = video.Id;
        Title = video.Title;
        Duration = video.Duration;
        Quality = video.Quality;
    }

    internal Video ToVideo()
    {
        return new Video() 
        { 
            Id = this.Id, 
            Title = this.Title, 
            Duration = this.Duration, 
            Quality = this.Quality 
        };
    }
}