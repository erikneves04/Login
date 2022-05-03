using Access.Interfaces.Services;
using Access.Models.View;
using Access.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Access.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessLoggerController : Authorize
{
    private readonly IAccessLoggerServices _services;

    public AccessLoggerController(IAccessLoggerServices services, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor, services)
    {
        _services = services;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AccessLoggerView>> Get()
    {
        try
        {
            ValidateToken();

            var content = _services.ViewAll();
            return Ok(content);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<AccessLoggerView>> Get(Guid id)
    {
        try
        {
            ValidateToken();

            var content = _services.View(id);
            return Ok(content);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("user/{id}")]
    public ActionResult<IEnumerable<AccessLoggerView>> GetByUser(Guid id)
    {
        try
        {
            ValidateToken();

            var content = _services.ViewByUserId(id);
            return Ok(content);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    //[HttpPost]  -- Route blocked
    private ActionResult<AccessLoggerView> Post([FromBody] AccessLoggerInsertView logger)
    {
        try
        {
            ValidateToken();

            var content = _services.Insert(logger);
            return Created(this.Url.Action("POST"), content);
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

    [HttpPut("{id}")]
    public ActionResult<UserView> Put([FromBody] AccessLoggerInsertView logger, Guid id)
    {
        try
        {
            ValidateToken();

            var content = _services.Update(logger, id);
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
    public ActionResult Delete(Guid id)
    {
        try
        {
            ValidateToken();

            _services.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
