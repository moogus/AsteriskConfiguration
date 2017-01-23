using System.ComponentModel.DataAnnotations;

namespace ModelAccess.Models
{
 public  class LogOnModel
  {
    [Required]
    [Display(Name = "Extension Number")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
  }
}
