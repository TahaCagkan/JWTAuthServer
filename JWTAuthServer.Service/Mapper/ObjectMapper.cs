using AutoMapper;
using System;

namespace JWTAuthServer.Service.Mapper
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });
            return config.CreateMapper();
        });
        //ihtiyaç dahilinde büyük datların geç yüklenmesini sağlar    
        public static IMapper Mapper => lazy.Value;
    }
}
