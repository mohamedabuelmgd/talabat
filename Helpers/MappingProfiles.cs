using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());


            CreateMap<AddressDto,Core.Entities.Identity.Address>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto,Core.Entities.Order_Aggregate.Address>();
           //use to return data with specific type and just show the data that i want 
            CreateMap<Order, OrderToReturnDto>()//get data from order and show order to return dto 
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))//show the name of delivery method
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));//show the cost of delivery method
            CreateMap<OrderItem, OrderItemDto>()//get data from order item to order item dto and show it 
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))//get product id from product and put it in product id
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))//get product name from product and put it in product name
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))//get picture url from product and put it in picture Url
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());


        }
    }
}
