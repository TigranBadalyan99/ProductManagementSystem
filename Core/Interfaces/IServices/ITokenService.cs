using Core.Entites;

namespace Core.Interfaces.IServices;

public interface ITokenService
{
    Task<string> GenerateJwtToken(ApplicationUser user);
}