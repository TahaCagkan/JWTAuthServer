using AutoMapper;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Entity;

namespace JWTAuthServer.Service.Mapper
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}
