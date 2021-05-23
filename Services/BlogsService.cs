using System;
using System.Collections.Generic;
using bloggerServer.Models;
using bloggerServer.Repositories;

namespace bloggerServer.Services
{
  public class BlogsService
  {
    private readonly BlogsRepository _repo;

    public BlogsService(BlogsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Blog> GetAll()
    {
      return _repo.GetAll();
    }

    internal Blog GetById(int id)
    {
      Blog blog = _repo.GetById(id);
      if (blog == null)
      {
        throw new Exception("Invalid Blog Id");
      }
      return blog;
    }

    internal Blog Create(Blog newBlog)
    {
      return _repo.Create(newBlog);
    }

    internal Blog Update(Blog update)
    {
      // first we get the blog
      Blog original = GetById(update.Id);
      // check if update.creatorId is the same as original.creator id
      if (update.CreatorId != original.CreatorId)
      {
        throw new Exception("You cant do that!");
      }

      original.Title = update.Title.Length > 0 ? update.Title : original.Title;
      original.Body = update.Body.Length > 0 ? update.Body : original.Body;
      original.ImgUrl = update.ImgUrl.Length > 0 ? update.ImgUrl : original.ImgUrl;
      original.Published = update.Published ? update.Published : original.Published;
      if (_repo.Update(original))
      {
        return original;
      }
      throw new Exception("Something went wrong??");
    }

    internal void Delete(int id, string creatorId)
    {
      // this will fun the function GetById 
      Blog blog = GetById(id);
      //   this checks if the blogs creator is the person whosbtrying to delete
      if (blog.CreatorId != creatorId)
      {
        throw new Exception("You cannot delort another users blog!");
      }
      if (!_repo.Delete(id))
      {
        throw new Exception("Something has gone very very wrong!");
      }
    }

    internal IEnumerable<Blog> GetBlogsByCreatorId(string id)
    {
      return _repo.GetBlogsByCreatorId(id);
      // Blog myBlogs = _repo.GetBlogsByCreatorId(id);
      // if (myBlogs == null)
      // {
      //   throw new Exception("No Blogs to Show");
      // }
      // return myBlogs;
    }
  }
}