namespace SilkRoad.Core;

public interface ICompleteAccountRepository
{
    Task CompleteAccountCreationAsync(AppUserInfo account);
    Task<IReadOnlyList<Country>> GetAllCountriesAsync(string? searchTerm = null);
    Task<IReadOnlyList<City>> GetCitiesByStateIdAsync(int stateId, string? searchTerm = null);
    Task<IReadOnlyList<State>> GetStatesByCountryIdAsync(int countryId, string? searchTerm = null);
}
