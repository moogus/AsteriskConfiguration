using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelAccess.Models;

namespace UserConfiguration.Controllers
{
  public class UserDetailsController : Controller
  {
    private readonly Repository<IExtension> _extensionContext;

    public UserDetailsController()
    {
      _extensionContext = new Repository<IExtension>();
    }

    public ActionResult Index()
    {
      var currentExtension = _extensionContext.GetFromName(User.Identity.Name);
      return View(currentExtension);
    }

    [HttpPost]
    public ActionResult Index(int id, string firstName, string lastName, string email, string department, string jobTitle)
    {
      var extension = _extensionContext.GetFromId(id);
      extension.FirstName = firstName;
      extension.LastName = lastName;
      extension.Email = email;
      extension.Department = department;
      extension.JobTitle = jobTitle;

      var update = extension.Update();
      TempData["message"] = update ? "Your profile has been updated." : "Something went wrong.";

      return RedirectToAction("Index", "UserConfigHome");
    }
  }
}
