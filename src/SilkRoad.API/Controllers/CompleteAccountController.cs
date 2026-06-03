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
    public async Task<IActionResult> CompleteAccountCreation(CompleteAccountDTO account)
    {
        var appUserInfo = _mapper.Map<AppUserInfo>(account);
        await _uow.CompleteAccountRepository.CompleteAccountCreationAsync(appUserInfo);
        return Ok();
    }

    [HttpGet("all-countries")]
    public async Task<IActionResult> GetAllCountries(string? searchTerm = null)
    {
        var countries = await _uow.CompleteAccountRepository.GetAllCountriesAsync(searchTerm);
        var countryDTOs = _mapper.Map<List<CountryDTO>>(countries);
        return Ok(countryDTOs);
    }

    [HttpGet("states-by-country/{countryId}")]
    public async Task<IActionResult> GetStatesByCountryId(int countryId, string? search)
    {
        var states = await _uow.CompleteAccountRepository.GetStatesByCountryIdAsync(countryId, search);
        var stateDTOs = _mapper.Map<List<StateDTO>>(states);
        return Ok(stateDTOs);
    }

    [HttpGet("cities-by-state/{stateId}")]
    public async Task<IActionResult> GetCitiesByStateId(int stateId, string? search)
    {
        var cities = await _uow.CompleteAccountRepository.GetCitiesByStateIdAsync(stateId, search);
        var cityDTOs = _mapper.Map<List<CityDTO>>(cities);
        return Ok(cityDTOs);
    }
}
