using AutoMapper;
using SilkRoad.Core;

namespace SilkRoad.API;

public class CompleteAccountMapper : Profile
{
    public CompleteAccountMapper()
    {
        CreateMap<CompleteAccountDTO, AppUserInfo>().ReverseMap();
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<State, StateDTO>().ReverseMap();
        CreateMap<City, CityDTO>().ReverseMap();
    }
}
