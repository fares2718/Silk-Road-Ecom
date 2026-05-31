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

    [HttpPost("login")]

    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        var result = await _uow.Auth.LoginAsync(loginDTO);
        if(result is null)
            return BadRequest(new APIResponse(400));
        if(result.Contains("aren't registered"))
            return NotFound(new APIResponse(404,result));
        if(result.StartsWith("Please"))
            return Unauthorized(new APIResponse(401,result));
        Response.Cookies.Append("token",result!,new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.Now.AddDays(1)
        });
        return Ok(new APIResponse(200));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        var result = await _uow.Auth.RegisterAsync(registerDTO);
            
        if(result!="done")
            return BadRequest(new APIResponse(400,result));
            
        return Ok(new APIResponse(200));
    }
}
