using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Asterisk.AccountManagement.AccountModels
{
  public class ChangePasswordModel
  {
    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    [Microsoft.Build.Framework.Required]
    [StringLength(100, ErrorMessage = "The new password must be at least {2} characters.", MinimumLength = 4)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}