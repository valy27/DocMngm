using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Repository.Models.Identity;

namespace DocumentManagement.Infrastructure.UserResolver
{
    public interface IUserResolverService
    {
        ApplicationUser GetUser();
        IEnumerable<string> GetUserRoles(ApplicationUser user);
    }
}
