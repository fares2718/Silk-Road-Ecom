using System.Security.Claims;
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

    [HttpGet("is-authenticated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> IsAuthenticated()
    {
        if (!Request.Cookies.TryGetValue("token", out string? accessToken) || string.IsNullOrEmpty(accessToken))
        {
            return Ok(false);
        }

        try
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier
                                                               || c.Type == "sub");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Ok(false);
            }

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                if (Request.Cookies.TryGetValue("refreshToken", out string? refreshToken) && !string.IsNullOrEmpty(refreshToken))
                {
                    bool isTokenValid = await _uow.Auth.IsAuthenticatedAsync(userIdClaim.Value, refreshToken);
                    if (isTokenValid)
                    {
                        return Ok(true);
                    }
                }
                return Ok(false);
            }

            return Ok(true);
        }
        catch
        {
            return Ok(false);
        }
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
        if (!string.IsNullOrEmpty(result.Error))
        {
            if (result.Error.Contains("aren't registered"))
                return NotFound(new APIResponse(404, result.Error));
            if (result.Error.StartsWith("Please"))
                return Unauthorized(new APIResponse(401, result.Error));
        }
        Response.Cookies.Append("token", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddDays(1)
        });
        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddDays(3)
        });
        return Ok(new APIResponse(200));
    }


    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue("token", out string? accessToken) && !string.IsNullOrEmpty(accessToken))
        {
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier
                                                                   || c.Type == "sub");

                if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
                {
                    await _uow.Auth.LogoutAsync(userIdClaim.Value);
                }
            }
            catch
            {
                // Fail silently on token parsing issues during logout 
                // to ensure client cookies still get cleaned up below
            }
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        Response.Cookies.Delete("token", cookieOptions);
        Response.Cookies.Delete("refreshToken", cookieOptions);

        return Ok(new APIResponse(200, "Logged out successfully."));
    }


    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out string? refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(new APIResponse(400, "Refresh token is missing."));
        }

        if (!Request.Cookies.TryGetValue("token", out string? accessToken) || string.IsNullOrEmpty(accessToken))
        {
            return BadRequest(new APIResponse(400, "Access token is missing."));
        }

        string userId;
        try
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier
                                                               || c.Type == "sub");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Unauthorized(new APIResponse(401, "Invalid access token payload."));
            }

            userId = userIdClaim.Value;
        }
        catch
        {
            return Unauthorized(new APIResponse(401, "Malformed access token."));
        }

        var result = await _uow.Auth.RefreshAsync(userId, refreshToken);

        if (result is null)
        {
            return Unauthorized(new APIResponse(401, "User no longer exists."));
        }

        if (!string.IsNullOrEmpty(result.Error))
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("refreshToken");
            return Unauthorized(new APIResponse(401, result.Error));
        }

        var baseCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost"
        };

        Response.Cookies.Append("token", result.AccessToken!, new CookieOptions(baseCookieOptions)
        {
            Expires = DateTime.UtcNow.AddDays(1)
        });

        Response.Cookies.Append("refreshToken", result.RefreshToken!, new CookieOptions(baseCookieOptions)
        {
            Expires = DateTime.UtcNow.AddDays(3)
        });

        return Ok(new APIResponse(200, "Token refreshed successfully."));
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
