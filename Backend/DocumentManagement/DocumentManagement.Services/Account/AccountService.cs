using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManagement.Repository;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.Services.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
      var result = new ClaimsIdentity();
      if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        return await Task.FromResult<ClaimsIdentity>(null);

      var user = _userManager.FindByNameAsync(userName).Result;
      if (user == null) return await Task.FromResult<ClaimsIdentity>(null);

      //if (await _userManager.CheckPasswordAsync(user, password))
      //  return await Task.FromResult(_jwtService.GenerateClaimsIdentity(userName, user.Id));

      if (await _userManager.CheckPasswordAsync(user, password))
        result.AddClaims(_jwtService.GenerateClaimsIdentity(userName,user.Id).Claims);

      var userRoles = _userManager.GetRolesAsync(user).Result;

       var roleClaim = new Claim("userRole", userRoles.FirstOrDefault());

      result.AddClaim(roleClaim);
      //var userAppRoles = _roleManager.Roles.Where(r => userRoles.Contains(r.Name)).ToList();
      //userAppRoles.ForEach(role => result.AddClaims(_roleManager.GetClaimsAsync(role).Result));

      //if(roleCliams.IsCompletedSuccessfully)

      return await Task.FromResult<ClaimsIdentity>(result);
    }

    public async Task CreateUserAccount(ApplicationUser userIdentity,User user , string password, string role)
    {
      try
      {
        var result = _userManager.CreateAsync(userIdentity, password).Result;

        var applicationRole = _roleManager.FindByNameAsync(role).Result;

        if (applicationRole != null)
        {
          var addToRole = _userManager.AddToRoleAsync(userIdentity, applicationRole.NormalizedName).Result;

          if (addToRole.Succeeded && result.Succeeded)
          {
            user.IdentityId = userIdentity.Id;
            _appDbContext.Users.Add(user);
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