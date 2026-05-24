using AutoMapper;
using SilkRoad.Core;
using SilkRoad.Core.Entities;

namespace SilkRoad.API;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<AddProductDTO, Product>()
        .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
        .ReverseMap();
        
        CreateMap<Product, ProductDTO>()
        .ForMember(dest => dest.ImageURLs,
        opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageURL).ToList()))
        .ReverseMap();

        CreateMap<UpdateProductDTO, Product>()
        .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
        {
            if (srcMember == null)
                return false; // Skip mapping if the source member is null
            if (srcMember is string str)
                return !string.IsNullOrEmpty(str); // Skip mapping if the source string is null or empty
            return true; // Map other types of members
        }));
    }

}
