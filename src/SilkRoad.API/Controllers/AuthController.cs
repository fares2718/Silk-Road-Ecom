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

    [HttpPost("activate-account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ActiveAccountDTO>> ActivateAccount(ActiveAccountDTO activeAccountDTO)
    {
        var result = await _uow.Auth.ActivateAccount(activeAccountDTO);
        return result ? Ok(new APIResponse(200)) : Unauthorized(new APIResponse(401, "Please activate your account"));
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        var result = await _uow.Auth.LoginAsync(loginDTO);
        if (result is null)
            return BadRequest(new APIResponse(400));
        if (result.Contains("aren't registered"))
            return NotFound(new APIResponse(404, result));
        if (result.StartsWith("Please"))
            return Unauthorized(new APIResponse(401, result));
        Response.Cookies.Append("token", result!, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddDays(1)
        });
        return Ok(new APIResponse(200));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        var result = await _uow.Auth.RegisterAsync(registerDTO);

        if (result != "done")
            return BadRequest(new APIResponse(400, result));

        return Ok(new APIResponse(200));
    }


    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<ActionResult<ActiveAccountDTO>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
    {
        var result = await _uow.Auth.ResetPasswordAsync(resetPasswordDTO);
        if (result is null)
            return Unauthorized(new APIResponse(401));
        if (result.Contains("successfuly"))
            return Ok(new APIResponse(200));
        return BadRequest(new APIResponse(400, result));

    }

    [HttpPost("send-forget-password-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ActiveAccountDTO>> SendForgetPasswordEmail(string email)
    {
        var result = await _uow.Auth.SendForgetPasswordEmail(email);
        return result ? Ok(new APIResponse(200, "Email has been sent successfuly")) : BadRequest(new APIResponse(400, "Invalid email"));
    }

    

}
