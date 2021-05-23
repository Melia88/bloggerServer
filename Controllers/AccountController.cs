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
  [Route("[controller]")]

  // TODO[epic=Auth] Adds authguard to all routes on the whole controller
  [Authorize]
  public class AccountController : ControllerBase
  {
    private readonly AccountsService _service;
    private readonly BlogsService _bservice;

    public AccountController(AccountsService service, BlogsService bservice)
    {
      _service = service;
      _bservice = bservice;
    }

    [HttpGet]
    public async Task<ActionResult<Account>> Get()
    {
      try
      {
        // TODO[epic=Auth] Replaces req.userinfo
        // IF YOU EVER NEED THE ACTIVE USERS INFO THIS IS HOW YOU DO IT (FROM AUTH0)
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Account currentUser = _service.GetOrCreateAccount(userInfo);
        return Ok(currentUser);
      }
      catch (Exception error)
      {
        return BadRequest(error.Message);
      }
    }

    [HttpGet("{id}/blogs")]
    public async Task<ActionResult<IEnumerable<Blog>>> GetMyBlogs()
    {

      // TODO[epic=Auth] Replaces req.userinfo
      // IF YOU EVER NEED THE ACTIVE USERS INFO THIS IS HOW YOU DO IT (FROM AUTH0)
      Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
      IEnumerable<Blog> blogs = _bservice.GetBlogsByCreatorId(userInfo.Id);
      return Ok(blogs);
    }
  }
}
