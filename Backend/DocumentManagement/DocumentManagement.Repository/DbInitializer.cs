using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Repository
{
    public class DbInitializer: IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DbInitializer(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void Initialize()
        {
            _context.Database.Migrate();
            //If there is already an Administrator role, abort
            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {


                //Create the Administartor Role
                _roleManager.CreateAsync(new ApplicationRole {Name = "Admin", NormalizedName = "Administrator"})
                    .GetAwaiter().GetResult();

                //Create the default Admin account and apply the Administrator role
                string user = "admin";
                string password = "1234";
                _userManager
                    .CreateAsync(new ApplicationUser {Id = "1", UserName = user, Email = user, EmailConfirmed = false},
                        password).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Admin").GetAwaiter().GetResult();

                _context.SaveChanges();

                if (!_context.Accounts.Any())
                {
                    _context.Accounts.Add(new Account
                    {
                        FirstName = "Admin",
                        LastName = "Administrator",
                        IdentityId = _userManager.FindByNameAsync(user).Result.Id
                    });

                }
            }
        }
    }

    public interface IDbInitializer
    {
        void Initialize();
    }
}
