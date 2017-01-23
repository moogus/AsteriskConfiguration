using System.Web.Mvc;
using System.Web.Security;
using Asterisk.AccountManagement.AccountModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _modelRepository;

        public AccountController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
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
                    if (model.UserName == model.Password)
                    {
                        return RedirectToAction("SetFirstPassword", "Account");
                    }
                    else
                    {
                        return RedirectToAction("Index",
                                                Roles.IsUserInRole(model.UserName, "admin") ? "ExtensionAdmin" : "UserConfigHome");
                    }
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn", "Account");
        }

        [Authorize]
        public ActionResult SetFirstPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetFirstPassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)            
            {
                var transaction = _modelRepository.ModelTransaction();

                using (transaction)
                {
                    var user = _modelRepository.GetFromName<IUserConfig>(User.Identity.Name);

                    if (user.Password == model.OldPassword)
                    {
                        user.Password = model.NewPassword;

                        if (transaction.Commit())
                        {
                            TempData["message"] = "Your password has been updated.";
                            return RedirectToAction("Index", "UserConfigHome");
                        }

                        ModelState.AddModelError("", "The new password is invalid.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect.");   
                    }                    
                }
            }
            else
            {
                ModelState.AddModelError("", "The model state is invalid.");
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
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var transaction = _modelRepository.ModelTransaction();

                using (transaction)
                {
                    var user = _modelRepository.GetFromName<IUserConfig>(User.Identity.Name);

                    if (user.Password == model.OldPassword)
                    {
                        user.Password = model.NewPassword;

                        if (transaction.Commit())
                        {
                            TempData["message"] = "Your password has been updated.";
                            return RedirectToAction("Index", "UserConfigHome");
                        }
                        
                        ModelState.AddModelError("", "The new password is invalid.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect.");   
                    }                    
                }
            }
            else
            {
                ModelState.AddModelError("", "The model state is invalid.");
            }

            return View(model);
        }

        public ActionResult PasswordReset(string extn)
        {
            ViewBag.data = extn;
            return View();
        }

        public ActionResult ResetPassword(string extn)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var userConfig = _modelRepository.GetFromName<IUserConfig>(extn);
                userConfig.Password = userConfig.Number;
                
                var message = transaction.Commit() ? "The password has been reset." : "Password reset operation failed.";
            }

            //TODO: fix message below.
            // return RedirectToAction("PasswordReset", new { extn = message });
            return RedirectToAction("PasswordReset", new { extn = "" });
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
                    return
                      "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                      "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                      "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}