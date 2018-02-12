using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManagement.Repository.Models.Identity;

namespace DocumentManagement.Services.Account
{
  public interface IAccountService
  {
    Task<ClaimsIdentity> GetClaimsIdentity(string username, string password);
    Task CreateUserAccount(ApplicationUser user, string password, string role);
  }
}