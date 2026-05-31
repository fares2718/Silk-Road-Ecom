namespace SilkRoad.Core;

public interface IAuth
{
    Task<bool> CheckActiveAccount(ActiveAccountDTO activeAccountDTO);
    Task<string?> LoginAsync(LoginDTO loginDTO);
    Task<string?> RegisterAsync(RegisterDTO registerDTO);
    Task<string?> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
    Task<bool> SendForgetPasswordEmail(string email);
}
