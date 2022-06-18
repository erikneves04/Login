using Access.Interfaces.Services;
using Access.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Access.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("Admin")]
public class UserController : ControllerBase
{
    private readonly IUserServices _services;
    private readonly IAccessServices _accessServices;

    public UserController(IUserServices services, IAccessServices accessServices)
    {
        _services = services;
        _accessServices = accessServices;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserView>> Get()
    {
        try
        {
            var content = _services.ViewAll();
            return Ok(content);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize("Common")]
    public ActionResult<UserView> Get(Guid id)
    {
        try
        {
            var content = _services.View(id);
            return Ok(content);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<TokenView> Login([FromBody] LoginView data)
    {
        try
        {
            var content = _accessServices.Login(data);
            return Ok(content);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult<UserView> Post([FromBody] UserInsertView user)
    {
        try
        {
            var content = _services.Insert(user);
            return Created(this.Url.Action("POST"), content);
        }
        catch(InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult<UserView> Put([FromBody] UserInsertView user, Guid id)
    {
        try
        {
            var content = _services.Update(user, id);
            return Ok(content);
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult<NoContentResult> Delete(Guid id)
    {
        try
        {
            _services.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}