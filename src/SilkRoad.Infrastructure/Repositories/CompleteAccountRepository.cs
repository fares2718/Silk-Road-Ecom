using Microsoft.EntityFrameworkCore;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

public class CompleteAccountRepository : ICompleteAccountRepository
{
    private readonly AppDbContext _context;

    public CompleteAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAccountCreationAsync(AppUserInfo account)
    {
        _context.AppUserInfos.Add(account);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Country>> GetAllCountriesAsync(string? searchTerm = null)
    {
        var query = from country in _context.Countries
                    where searchTerm == null || country.CountryName.Contains(searchTerm)
                    select new Country
                    {
                        CountryID = country.CountryID,
                        CountryName = country.CountryName
                    };
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<City>> GetCitiesByStateIdAsync(int stateId, string? searchTerm = null)
    {
        var query = from city in _context.Cities
                    where city.StateID == stateId && (searchTerm == null || city.CityName.Contains(searchTerm))
                    select new City
                    {
                        CityID = city.CityID,
                        CityName = city.CityName,
                    };
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<State>> GetStatesByCountryIdAsync(int countryId, string? searchTerm = null)
    {
        var query = from state in _context.States
                    where state.CountryID == countryId && (searchTerm == null || state.StateName.Contains(searchTerm))
                    select new State
                    {
                        StateID = state.StateID,
                        StateName = state.StateName,
                    };
        return await query.ToListAsync();
    }
}
