using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManagement.Repository;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.Services.Jwt;
using Microsoft.AspNetCore.Identity;

namespace DocumentManagement.Services.Account
{
  public class AccountService : IAccountService
  {
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _appDbContext;
    private readonly IJwtService _jwtService;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
      IJwtService jwtService, AppDbContext appDbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _jwtService = jwtService;
      _appDbContext = appDbContext;
    }

    public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
    {
      if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        return await Task.FromResult<ClaimsIdentity>(null);

      var user = _userManager.FindByNameAsync(userName).Result;
      if (user == null) return await Task.FromResult<ClaimsIdentity>(null);

      if (await _userManager.CheckPasswordAsync(user, password))
        return await Task.FromResult(_jwtService.GenerateClaimsIdentity(userName, user.Id));

      return await Task.FromResult<ClaimsIdentity>(null);
    }

    public async Task CreateUserAccount(ApplicationUser user, string password, string role)
    {
      try
      {
        var result = _userManager.CreateAsync(user, password).Result;

        var applicationRole = _roleManager.FindByNameAsync(role).Result;

        if (applicationRole != null)
        {
          var addToRole = _userManager.AddToRoleAsync(user, applicationRole.NormalizedName).Result;

          if (addToRole.Succeeded && result.Succeeded)
          {
            _appDbContext.Users.Add(new User {IdentityId = user.Id, FirstName = user.UserName});
            _appDbContext.SaveChanges();
          }
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
  }
}