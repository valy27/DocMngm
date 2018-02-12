using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentManagement.Repository.Models.Identity;

namespace DocumentManagement.Repository.Models
{
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public DateTime Registerd { get; set; }

    [ForeignKey("ApplicationUser")]
    public string IdentityId { get; set; }

    public virtual ApplicationUser Identity { get; set; }
  }
}