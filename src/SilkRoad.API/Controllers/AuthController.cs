using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API.Controllers;
using SilkRoad.Core;

namespace SilkRoad.API;

public class AuthController : BaseController
{
    public AuthController(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
    {
        var result = await _uow.Auth.RegisterAsync(registerDTO);
            
        if(result!="done")
            return BadRequest(new APIResponse(400,result));
            
        return Ok(new APIResponse(200));
    }
}
