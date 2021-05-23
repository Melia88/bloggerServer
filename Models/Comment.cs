namespace bloggerServer.Models
{
  public class Comment
  {
    public int Id { get; set; }
    public string CreatorId { get; set; }
    public Account Creator { get; set; }
    public string Body { get; set; }
    public int BlogId { get; set; }
  }
}

// id INT NOT NULL AUTO_INCREMENT,
// --   creatorId VARCHAR (255) NOT NULL,
// --   body VARCHAR (255) NOT NULL,
// --   blogId INT NOT NULL,