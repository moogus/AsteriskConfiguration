using System.Web.Mvc;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize]
    public class UserDetailsController : Controller
    {
        private readonly IRepository _modelRepository;

        public UserDetailsController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index(string extn, bool isAdminEdit)
        {
            var extension = _modelRepository.GetFromName<IExtension>(extn);

            var userDetailsViewModel = new UserDetailsViewModel
              {
                  Id = extension.Id,
                  FirstName = extension.FirstName,
                  LastName = extension.LastName,
                  Email = extension.Email,
                  Department = extension.Department,
                  JobTitle = extension.JobTitle,
                  Message = isAdminEdit ? string.Format("Profile For Extension : {0}", extn) : "Profile Information"
              };

            return View(userDetailsViewModel);
        }

        [HttpPost]
        public ActionResult Index(string id, string firstName, string lastName, string email, string department,
                                  string jobTitle)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var extension = _modelRepository.GetFromId<IExtension>(int.Parse(id));
                extension.FirstName = firstName;
                extension.LastName = lastName;
                extension.Email = email;
                extension.Department = department;
                extension.JobTitle = jobTitle;

                TempData["message"] = transaction.Commit() ? "The profile has been updated." : "Failed to update profile.";

                return RedirectToAction("Index", "UserConfigHome");
            }
        }
    }
}