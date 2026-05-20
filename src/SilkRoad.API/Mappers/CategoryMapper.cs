using AutoMapper;
using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.API;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<AddCategoryDTO,Category>();
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}
