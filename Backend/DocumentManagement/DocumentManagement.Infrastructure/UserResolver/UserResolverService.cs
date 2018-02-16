using DocumentManagement.Repository.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DocumentManagement.Infrastructure.UserResolver
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserResolverService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ApplicationUser GetUser()
        {
            return _userManager.FindByIdAsync(_context.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type =="id")?.Value).Result;
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
