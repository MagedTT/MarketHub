using AutoMapper;
using MarketHub.Application.DTOs.Persistence.Wishlist;
using MarketHub.Application.Features.Brands.Commands.AddBrand;
using MarketHub.Application.Features.Brands.Commands.UpdateBrand;
using MarketHub.Application.Features.Carts.Commands.CreateCart;
using MarketHub.Application.Features.Inventories.Commands.DeleteInventory;
using MarketHub.Application.Features.Inventories.COmmands.CreateInventory;
using MarketHub.Application.Features.Products.Commands.AddProductCommand;
using MarketHub.Application.Features.Reviews.Commands.CreateReview;
using MarketHub.Application.Features.Reviews.Commands.UpdateReview;
using MarketHub.Application.Models.Authentication;
using MarketHub.Domain.Entities;

namespace MarketHub.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegisterationDto, User>();
        CreateMap<AddProductCommand, Product>()
            .ForMember(dest => dest.Specifications,
                options => options.MapFrom(src => src.Specifications.GetRawText()))
            .ForMember(dest => dest.Images, options => options.Ignore());

        CreateMap<AddBrandCommand, Brand>();
        CreateMap<UpdateBrandCommand, Brand>();

        CreateMap<CreateCartCommand, Cart>();

        CreateMap<Wishlist, WishlistDto>()
            .ForMember(dest => dest.WishlistItems, options => options.Ignore());

        CreateMap<CreateReviewCommand, Review>();
        CreateMap<UpdateReviewCommand, Review>()
            .ForMember(dest => dest.Id, options => options.MapFrom(src => src.ReviewId))
            .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.CurrentUserId));

        CreateMap<UpdateInventoryCommand, Inventory>();

        CreateMap<CreateInventoryCommand, Inventory>();
    }
}