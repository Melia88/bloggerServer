using System;
using System.Collections.Generic;
using System.Linq;
using bloggerServer.Models;
using bloggerServer.Repositories;

namespace bloggerServer.Services
{
  public class CommentsService
  {
    private readonly CommentsRepository _repo;

    public CommentsService(CommentsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Comment> GetCommentsByBlogId(int id)
    {
      return _repo.GetCommentsByBlogId(id);
    }

    internal Comment Create(Comment newComment)
    {
      return _repo.Create(newComment);
    }

    internal Comment Update(Comment update)
    {
      Comment original = GetCommentsByBlogId(update.Id).FirstOrDefault();
      // check if update.creatorId is the same as original.creator id
      if (update.CreatorId != original.CreatorId)
      {
        throw new Exception("You cant do that!");
      }

      original.Body = update.Body.Length > 0 ? update.Body : original.Body;

      if (_repo.Update(original))
      {
        return original;
      }
      throw new Exception("Something went wrong??");
    }

    internal void Delete(int id, string creatorId)
    {
      Comment comment = GetCommentsByBlogId(id).FirstOrDefault();
      if (comment.CreatorId != creatorId)
      {
        throw new Exception("You cannot delort another users comment!");
      }
      if (!_repo.Delete(id))
      {
        throw new Exception("Something has gone very very wrong!");
      }
    }
  }
}