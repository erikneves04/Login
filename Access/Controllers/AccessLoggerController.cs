using Access.Interfaces.Services;
using Access.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Access.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccessLoggerController : ControllerBase
{
    private readonly IAccessLoggerServices _services;

    public AccessLoggerController(IAccessLoggerServices services)
    {
        _services = services;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AccessLoggerView>> Get()
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
    public ActionResult<IEnumerable<AccessLoggerView>> Get(Guid id)
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

    [HttpGet("user/{id}")]
    public ActionResult<IEnumerable<AccessLoggerView>> GetByUser(Guid id)
    {
        try
        {
            var content = _services.ViewByUserId(id);
            return Ok(content);
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
            _services.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
