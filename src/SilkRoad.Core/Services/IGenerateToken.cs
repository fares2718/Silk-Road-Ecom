namespace SilkRoad.Core;

public interface IGenerateToken
{
    public string GenerateAccessToken(AppUser user, IList<string> roles);
}
