using System;
using System.Web.Mvc;
using System.Web.Security;
using ModelAccess.Models;
using UserConfiguration.Models;

namespace UserConfiguration.Controllers
{
  public class AccountController : Controller
  {

    private readonly Repository<IUserConfig> _userConfigContext;

    public AccountController()
    {
      _userConfigContext = new Repository<IUserConfig>();
    }

    public ActionResult LogOn()
    {
      return View();
    }

    [HttpPost]
    public ActionResult LogOn(UserConfiguration.Models.LogOnModel model, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        if (Membership.ValidateUser(model.UserName, model.Password))
        {
          FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
          if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
          {
            return Redirect(returnUrl);
          }
          return model.UserName == model.Password ? RedirectToAction("SetFirstPassword", "Account") : RedirectToAction("Index", "UserConfigHome");
        }
        ModelState.AddModelError("", "The user name or password provided is incorrect.");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    public ActionResult LogOff()
    {
      FormsAuthentication.SignOut();
      return RedirectToAction("Index", "UserConfigHome");
    }

    [Authorize]
    public ActionResult SetFirstPassword()
    {
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult SetFirstPassword(UserConfiguration.Models.ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        var user = _userConfigContext.GetFromName(User.Identity.Name);
        if (user.Password == model.OldPassword)
        {
          user.Password = model.NewPassword;
          var update = user.Update();
          TempData["message"] = update ? "Your password has been updated." : "Something went wrong.";

          return RedirectToAction("Index", "UserConfigHome");
        }

        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
      }

      return View(model);
    }

    [Authorize]
    public ActionResult ChangePassword()
    {
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult ChangePassword(UserConfiguration.Models.ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        var user = _userConfigContext.GetFromName(User.Identity.Name);
        if (user.Password == model.OldPassword)
        {
          user.Password = model.NewPassword;
         var update = user.Update();
          TempData["message"] = update ? "Your password has been updated." : "Something went wrong.";

          return RedirectToAction("Index", "UserConfigHome");
        }

        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
      }

      return View(model);
    }

    #region Status Codes
    private static string ErrorCodeToString(MembershipCreateStatus createStatus)
    {
      // See http://go.microsoft.com/fwlink/?LinkID=177550 for
      // a full list of status codes.
      switch (createStatus)
      {
        case MembershipCreateStatus.DuplicateUserName:
          return "User name already exists. Please enter a different user name.";

        case MembershipCreateStatus.DuplicateEmail:
          return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        case MembershipCreateStatus.InvalidPassword:
          return "The password provided is invalid. Please enter a valid password value.";

        case MembershipCreateStatus.InvalidEmail:
          return "The e-mail address provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.InvalidAnswer:
          return "The password retrieval answer provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.InvalidQuestion:
          return "The password retrieval question provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.InvalidUserName:
          return "The user name provided is invalid. Please check the value and try again.";

        case MembershipCreateStatus.ProviderError:
          return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        case MembershipCreateStatus.UserRejected:
          return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        default:
          return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
      }
    }
    #endregion
  }
}
