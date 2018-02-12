using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DocumentManagement.Services.Jwt
{
  public class JwtService : IJwtService
  {
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
      _jwtOptions = jwtOptions.Value;
    }

    public string GenerateJwt(ClaimsIdentity identity, string userName, JsonSerializerSettings serializerSettings)
    {
      try
      {
        var response = new
        {
          id = identity.Claims.Single(c => c.Type == "id").Value,
          auth_token = GenerateEncodedToken(userName, identity),
          expires_in = _jwtOptions.Expiration
        };

        return JsonConvert.SerializeObject(response, serializerSettings);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    public string GenerateEncodedToken(string userName, ClaimsIdentity identity)
    {
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, userName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        identity.FindFirst("rol"),
        identity.FindFirst("id")
      };

      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(
        _jwtOptions.Issuer,
        _jwtOptions.Audience,
        claims,
        _jwtOptions.NotBefore,
        _jwtOptions.Expiration,
        _jwtOptions.SigningCredentials);

      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      return encodedJwt;
    }

    public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
    {
      return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
      {
        new Claim("id", id),
        new Claim("rol", "api_access")
      });
    }
  }
}