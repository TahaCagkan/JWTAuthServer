using JWTAuthServer.Core.DTOs;
using JWTAuthServer.SharedLibrary.DTOS;
using System.Threading.Tasks;

namespace JWTAuthServer.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);

    }
}
