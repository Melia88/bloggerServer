using System;
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
  public class CommentsController : ControllerBase
  {
    private readonly CommentsService _cService;
    private readonly BlogsService _bService;
    private readonly AccountsService _acctService;

    public CommentsController(CommentsService cService, BlogsService bService, AccountsService acctService)
    {
      _cService = cService;
      _bService = bService;
      _acctService = acctService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Comment>> Create([FromBody] Comment newComment)
    {
      try
      {
        // TODO[epic=Auth] Get the user info to set the creatorID
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // safety to make sure an account exists for that user before CREATE-ing stuff.
        Account fullAccount = _acctService.GetOrCreateAccount(userInfo);
        newComment.CreatorId = userInfo.Id;

        Comment comment = _cService.Create(newComment);
        //TODO[epic=Populate] adds the account to the new object as the creator
        comment.Creator = fullAccount;
        return Ok(comment);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }

    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Comment>> Update(int id, [FromBody] Comment update)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // _rService.Update(id, userInfo.Id);
        update.CreatorId = userInfo.Id;
        update.Id = id;
        Comment updated = _cService.Update(update);
        return Ok(updated);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpDelete("{id}")]
    [Authorize]

    public async Task<ActionResult<Comment>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _cService.Delete(id, userInfo.Id);
        return Ok("Successfully Deleted");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}

// POST: '/api/comments' Create new Comment *
// PUT: '/api/comments/:id' Edits Comment **
// DELETE: '/api/comments/:id' Deletes Comment **