using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using bloggerServer.Models;
using Dapper;

namespace bloggerServer.Repositories
{
  public class BlogsRepository
  {
    private readonly IDbConnection _db;

    public BlogsRepository(IDbConnection db)
    {
      _db = db;
    }

    public IEnumerable<Blog> GetAll()
    {
      // blogs is what my table is names so this is calling FROM the blogs table
      string sql = @"
      SELECT 
        b.*,
        a.*
      FROM blogs b
      JOIN accounts a ON b.creatorId = a.id";
      return _db.Query<Blog, Account, Blog>(sql, (blog, account) =>
      {
        blog.Creator = account;
        return blog;
      }, splitOn: "id");
    }


    public IEnumerable<Blog> GetByCreatorId(string id)
    {
      string sql = @"
      SELECT 
        c.*,
        a.* 
      FROM cars c
      JOIN accounts a ON c.creatorId = a.id
      WHERE creatorId = @id";
      return _db.Query<Blog, Account, Blog>(sql, (blog, account) =>
      {
        blog.Creator = account;
        return blog;
      }
      , new { id }, splitOn: "id");
    }
    public Blog GetById(int id)
    {
      string sql = @"
      SELECT 
        c.*,
        a.* 
      FROM cars c
      JOIN accounts a ON c.creatorId = a.id
      WHERE id = @id";
      return _db.Query<Blog, Account, Blog>(sql, (blog, account) =>
      {
        blog.Creator = account;
        return blog;
      }
      , new { id }, splitOn: "id").FirstOrDefault();
    }

    internal Blog Create(Blog newBlog)
    {
      string sql = @"
      INSERT INTO blogs
      (creatorId, title, body, imgUrl, published)
      VALUES
      (@CreatorId, @Title, @Body, @ImgUrl, @Published)
      SELECT LAST_INSERT_ID";
      newBlog.Id = _db.ExecuteScalar<int>(sql, newBlog);
      return newBlog;
    }

    internal bool Delete(int id)
    {
      string sql = "DELETE FROM blogs WHERE id = @id LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id });
      return affectedRows == 1;
    }

    internal bool Update(Blog original)
    {
      string sql = @"
      SET 
        title = @Title,
        body = @Body,
        imgUrl = @ImgUrl,
        published = @Published
      WHERE is = @id  
      ";
      int affectedRows = _db.Execute(sql, original);
      return affectedRows == 1;
    }

  }
}