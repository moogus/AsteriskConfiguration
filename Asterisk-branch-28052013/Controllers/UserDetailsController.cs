using System.Web.Mvc;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize]
  public class UserDetailsController : Controller
  {
    private readonly IRepository _repository;

    public UserDetailsController(IRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index(string extn, bool isAdminEdit)
    {
      var userDetailsViewModel = new UserDetailsViewModel
        {
          Extension = _repository.GetFromName<IExtension>(extn),
          Message = isAdminEdit ? string.Format("Profile For Extension : {0}", extn) : "Profile Information"
        };
      return View(userDetailsViewModel);
    }

    [HttpPost]
    public ActionResult Index(int id, string firstName, string lastName, string email, string department,
                              string jobTitle)
    {
      var extension = _repository.GetFromId<IExtension>(id);
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