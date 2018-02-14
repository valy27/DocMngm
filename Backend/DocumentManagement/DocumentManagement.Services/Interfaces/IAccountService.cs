using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;

namespace DocumentManagement.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ClaimsIdentity> GetClaimsIdentity(string username, string password);
        Task CreateUserAccount(ApplicationUser userIdentity, Account account, string password, string role);
        Account GetAccountForIdentity(string identityId);
    }
}