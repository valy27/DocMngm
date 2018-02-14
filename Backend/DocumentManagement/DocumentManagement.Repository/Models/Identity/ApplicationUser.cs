using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DocumentManagement.Repository.Models.Identity
{
  public class ApplicationUser : IdentityUser
  {
      public virtual Account  Account { get; set; }
  }
}