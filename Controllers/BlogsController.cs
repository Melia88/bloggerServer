using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bloggerServer.Models;
using bloggerServer.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bloggerServer.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BlogsController : ControllerBase
  {
    private readonly BlogsService _bService;
    private readonly AccountsService _acctService;
    private readonly CommentsService _cService;

    public BlogsController(BlogsService bService, AccountsService acctsService, CommentsService cService)
    {
      _bService = bService;
      _acctService = acctsService;
      _cService = cService;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Blog>> GetAll()
    {
      try
      {
        IEnumerable<Blog> blogs = _bService.GetAll();
        return Ok(blogs);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}")]
    public ActionResult<Blog> GetById(int id)
    {
      try
      {
        Blog found = _bService.GetById(id);
        return Ok(found);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpGet("{id}/comments")]
    public ActionResult<Blog> GetCommentsByBlogId(int id)
    {
      try
      {
        Comment found = _cService.GetCommentsByBlogId(id);
        return Ok(found);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreatedAtActionResult>> Create([FromBody] Blog newBlog)
    {
      try
      {
        // TODO[epic=Auth] Get the user info to set the creatorID
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // safety to make sure an account exists for that user before CREATE-ing stuff.
        Account fullAccount = _acctService.GetOrCreateAccount(userInfo);
        newBlog.CreatorId = userInfo.Id;

        Blog blog = _bService.Create(newBlog);
        //TODO[epic=Populate] adds the account to the new object as the creator
        blog.Creator = fullAccount;
        return Ok(blog);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Blog>> Update(int id, [FromBody] Blog update)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // _rService.Update(id, userInfo.Id);
        update.CreatorId = userInfo.Id;
        update.Id = id;
        Blog updated = _bService.Update(update);
        return Ok(updated);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]

    public async Task<ActionResult<Blog>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _bService.Delete(id, userInfo.Id);
        return Ok("Succesfully Deleted");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}