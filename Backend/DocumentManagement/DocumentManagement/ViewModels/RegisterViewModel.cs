using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.ViewModels
{
  public class RegisterViewModel
  {
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public DateTime Registerd { get; set; }
    public string Role { get; set; }

    [Required]
    [StringLength(4, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 4)]
    [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit password")]
    public string Password { get; set; }
  }
}