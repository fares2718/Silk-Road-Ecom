using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.Core;

namespace SilkRoad.API.Controllers;

public class BugController : BaseController
{
    public BugController(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
    }

    [HttpGet("not-found")]

    public async Task<ActionResult> GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("server-error")]

    public async Task<ActionResult> GetServerError()
    {
        return Problem();
    }

    [HttpGet("bad-request/{id}")]

    public async Task<ActionResult> GetBadRequest(int id)
    {
        return BadRequest();
    }

    [HttpGet("bad-request/")]

    public async Task<ActionResult> GetBadRequest()
    {
        return BadRequest();
    }
}