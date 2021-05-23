using System;
using System.Collections.Generic;
using System.Data;
using bloggerServer.Models;
using Dapper;

namespace bloggerServer.Repositories
{
  public class CommentsRepository
  {
    private readonly IDbConnection _db;

    public CommentsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Comment Create(Comment newComment)
    {
      string sql = @"
        INSERT INTO comments
        (creatorId, blogId, body)
        VALUES
        (@CreatorId, @BlogId, @Body)
        SELECT LAST_INSERT_ID";
      newComment.Id = _db.ExecuteScalar<int>(sql, newComment);
      return newComment;
    }

    public IEnumerable<Comment> GetCommentsByBlogId(int id)
    {
      string sql = @"
      SELECT 
        c.*,
        b.* 
      FROM comments c
      JOIN blogs b ON c.creatorId = b.id
      WHERE blogId = @id";
      return _db.Query<Comment, Blog, Comment>(sql, (comment, blog) =>
      {
        comment.BlogId = blog.Id;
        return comment;
      }
      , new { id }, splitOn: "id");
    }


    internal bool Update(Comment original)
    {
      string sql = @"
      SET 
        body = @Body,
      WHERE is = @id  
      ";
      int affectedRows = _db.Execute(sql, original);
      return affectedRows == 1;
    }

    internal bool Delete(int id)
    {
      string sql = "DELETE FROM comments WHERE id = @id LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id });
      return affectedRows == 1;
    }
  }
}