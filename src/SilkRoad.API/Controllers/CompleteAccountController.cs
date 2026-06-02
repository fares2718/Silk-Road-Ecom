using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SilkRoad.API.Controllers;
using SilkRoad.Core;

namespace SilkRoad.API;

public class CompleteAccountController : BaseController
{
    public CompleteAccountController(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
    {
    }

    [HttpPost("complete-account")]
    public async Task<IActionResult> CompleteAccountCreation(AppUserInfo account)
    {
        await _uow.CompleteAccountRepository.CompleteAccountCreationAsync(account);
        return Ok();
    }

    [HttpGet("all-countries")]
    public async Task<IActionResult> GetAllCountries(string? searchTerm = null)
    {
        var countries = await _uow.CompleteAccountRepository.GetAllCountriesAsync(searchTerm);
        return Ok(countries);
    }

    [HttpGet("states-by-country/{countryId}")]
    public async Task<IActionResult> GetStatesByCountryId(int countryId, string? search)
    {
        var states = await _uow.CompleteAccountRepository.GetStatesByCountryIdAsync(countryId, search);
        return Ok(states);
    }

    [HttpGet("cities-by-state/{stateId}")]
    public async Task<IActionResult> GetCitiesByStateId(int stateId, string? search)
    {
        var cities = await _uow.CompleteAccountRepository.GetCitiesByStateIdAsync(stateId, search);
        return Ok(cities);
    }
}
