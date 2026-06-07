namespace SilkRoad.Core;

public interface IAuth
{
    Task<bool> ActivateAccount(ActiveAccountDTO activeAccountDTO);
    Task<bool> IsAuthenticatedAsync(string userId, string refreshToken);
    Task<TokenResponse?> LoginAsync(LoginDTO loginDTO);
    Task<bool> LogoutAsync(string userId);
    Task<TokenResponse?> RefreshAsync(string userId,string refreshToken);
    Task<string?> RegisterAsync(RegisterDTO registerDTO);
    Task<string?> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
    Task<bool> SendForgetPasswordEmail(string email);
}
