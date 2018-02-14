using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManagement.Infrastructure.Jwt;
using DocumentManagement.Repository;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DocumentManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IGenericRepository<Account> _accountRepository;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IJwtService jwtService, IGenericRepository<Account> accountRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _accountRepository = accountRepository;
        }

        public Account GetAccountForIdentity(string identityId)
        {
            return _accountRepository.Get().FirstOrDefault(a => a.IdentityId == identityId);
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            var result = new ClaimsIdentity();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            var user = _userManager.FindByNameAsync(userName).Result;
            if (user == null)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            if (await _userManager.CheckPasswordAsync(user, password))
            {
                result.AddClaims(_jwtService.GenerateClaimsIdentity(userName, user.Id).Claims);
            }

            var userRoles = _userManager.GetRolesAsync(user).Result;

            var roleClaim = new Claim("userRole", userRoles.FirstOrDefault());

            result.AddClaim(roleClaim);

            return await Task.FromResult<ClaimsIdentity>(result);
        }

        public async Task CreateUserAccount(ApplicationUser userIdentity, Account account, string password, string role)
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
                        account.IdentityId = userIdentity.Id;
                        _accountRepository.Insert(account);
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