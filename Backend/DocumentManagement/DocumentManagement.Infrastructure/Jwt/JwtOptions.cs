using System;
using Microsoft.IdentityModel.Tokens;

namespace DocumentManagement.Infrastructure.Jwt
{
  public class JwtOptions
  {
    public string Issuer { get; set; }
    public string Subject { get; set; }
    public string Audience { get; set; }
    public DateTime Expiration => IssuedAt.Add(ValidFor);
    public DateTime NotBefore { get; set; } = DateTime.UtcNow;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);


    public SigningCredentials SigningCredentials { get; set; }
  }
}