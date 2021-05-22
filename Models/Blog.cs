using System.ComponentModel.DataAnnotations;

namespace bloggerServer.Models
{
  public class Blog
  {
    public int Id { get; set; }
    public string CreatorId { get; set; }
    public Account Creator { get; set; }
    [Required]
    [MaxLength(20)]

    public string Title { get; set; }

    public string Body { get; set; } = "No Description";
    public string ImgUrl { get; set; } = "http://placehold.it/200x200";
    public bool Published { get; set; } = false;
  }
}

