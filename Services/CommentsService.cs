using System;
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

    internal Comment GetCommentsByBlogId(int id)
    {
      Comment comment = _repo.GetCommentsByBlogId(id);
      if (comment == null)
      {
        throw new Exception("Invalid Blog Id");
      }
      return comment;
    }

    internal Comment Create(Comment newComment)
    {
      return _repo.Create(newComment);
    }

    internal Comment Update(Comment update)
    {
      Comment original = GetCommentsByBlogId(update.Id);
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

    internal void Delete(int id1, string id2)
    {
      throw new NotImplementedException();
    }
  }
}