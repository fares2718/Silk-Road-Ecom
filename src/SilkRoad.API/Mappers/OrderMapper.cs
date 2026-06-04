using AutoMapper;
using SilkRoad.Core;

namespace SilkRoad.API;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<Order,OrderDTO>().ReverseMap();
        CreateMap<OrderItem,OrderItemDTO>().ReverseMap();
    }
}
