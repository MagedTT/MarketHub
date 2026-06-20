using System.Text.Json;
using AutoMapper;
using MarketHub.API.Controllers;
using MarketHub.Application.Features.Products.Commands.AddProductCommand;

namespace MarketHub.API;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<IFormFile, FileUploadDto>()
            .ForMember(fud => fud.FileName, options =>
                options.MapFrom(ff => ff.FileName))
            .ForMember(fud => fud.FileExtension, options =>
                options.MapFrom(ff => Path.GetExtension(ff.FileName)))
            .ForMember(fud => fud.ContentType, options =>
                options.MapFrom(ff => ff.ContentType))
            .ForMember(fud => fud.LengthInBytes, options =>
                options.MapFrom(ff => (int)ff.Length))
            .ForMember(fud => fud.Content, options =>
                options.MapFrom(ff => ff.OpenReadStream()));

        CreateMap<ProductDto, AddProductCommand>()
            .ForMember(dest => dest.Specifications, options =>
                options.MapFrom(src => JsonSerializer.Deserialize<JsonElement>(src.Specifications)));
    }
}