using System.Security.Claims;
using Newtonsoft.Json;

namespace DocumentManagement.Infrastructure.Jwt
{
    public interface IJwtService
    {
        string GenerateJwt(ClaimsIdentity identity, string userName, JsonSerializerSettings serializerSettings);
        string GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}