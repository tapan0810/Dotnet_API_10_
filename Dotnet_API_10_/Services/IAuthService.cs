using Dotnet_API_10_.Dtos;
using Dotnet_API_10_.Entities;

namespace Dotnet_API_10_.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(USerDto request);

        Task<string?> LoginAsync(USerDto request);
    }
}
