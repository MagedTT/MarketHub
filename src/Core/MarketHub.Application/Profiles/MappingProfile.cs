using AutoMapper;
using MarketHub.Application.DTOs.Persistence.Product;
using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Application.Features.Brands.Commands.AddBrand;
using MarketHub.Application.Features.Brands.Commands.UpdateBrand;
using MarketHub.Application.Features.Carts.Commands.CreateCart;
using MarketHub.Application.Features.Products.Commands.AddProductCommand;
using MarketHub.Application.Models.Authentication;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegisterationDto, User>();
        CreateMap<AddProductCommand, Product>()
            .ForMember(product => product.Specifications,
                options => options.MapFrom(x => x.Specifications.GetRawText()))
            .ForMember(product => product.Images, options => options.Ignore());

        CreateMap<AddBrandCommand, Brand>();
        CreateMap<UpdateBrandCommand, Brand>();

        CreateMap<CreateCartCommand, Cart>();

        CreateMap<Wishlist, WishlistDto>()
            .ForMember(dest => dest.WishlistItems, options => options.Ignore());
    }
}