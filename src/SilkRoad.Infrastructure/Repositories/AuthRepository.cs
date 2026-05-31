using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

internal class AuthRepository : IAuth
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IGenerateToken _generateToken;

    public AuthRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IGenerateToken generateToken)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _generateToken = generateToken;
    }

    public async Task<string?> Login(LoginDTO loginDTO)
    {
        if (loginDTO is null || string.IsNullOrEmpty(loginDTO.Password)
            || string.IsNullOrEmpty(loginDTO.Email))
            return null;
        AppUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user is null)
            return "You aren't registered yet";
        if (!user.EmailConfirmed)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            await SendEmail(user.Email!, encodedToken, 
            "active", "Email Activation", 
            "Please activate your email, click on button to active");
            return "Please confirm your email, Check your Inbox";
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user,loginDTO.Password,true);
        if (!result.Succeeded)
            return "Invalid Credentials";
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _generateToken.GenerateAccessToken(user,roles);
        return accessToken;
    }

    public async Task<string?> RegisterAsync(RegisterDTO registerDTO)
    {
        if (registerDTO is null || string.IsNullOrEmpty(registerDTO.UserName)
            || string.IsNullOrEmpty(registerDTO.FirstName)
            || string.IsNullOrEmpty(registerDTO.LasName)
            || string.IsNullOrEmpty(registerDTO.Password)
            || string.IsNullOrEmpty(registerDTO.Email)
            )
        {
            return null;
        }
        if (await _userManager.FindByNameAsync(registerDTO.UserName) is not null
            || await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
            return "Username or Email already exists";
        AppUser user = new AppUser
        {
            UserName = registerDTO.UserName,
            Email = registerDTO.Email,
            FirstName = registerDTO.FirstName,
            MiddleName = registerDTO.MiddleName,
            LastName = registerDTO.LasName,
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (!result.Succeeded)
            return result.Errors.ToList()[0].Description;
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        await SendEmail(user.Email, encodedToken, "active", "Email Activation", "Please activate your email, click on button to active");
        return "Registered successfuly, Please confirm your email, Check your Inbox";
    }

    public async Task SendEmail(string email, string code,
    string component, string subject, string message)
    {
        EmailDTO emailDTO = new EmailDTO(
            email,
            "faresobaid2715@gmail.com",
            subject,
            EmailStringBody.SendEmail(email, code, component, message)
        );
    }
}
