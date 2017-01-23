using System.Collections.Generic;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmergencyNumbersController : Controller
    {
        private readonly IRepository _modelRepository;

        public EmergencyNumbersController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public string Add(string number, string description, string isInternal)
        {
            if (number == "") return "Please specify a valid number.";
            if (description == "") return "Please specify a valid description.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var emergencyNumber = _modelRepository.Add<IEmergencyNumber>();

                emergencyNumber.Number = number;
                emergencyNumber.Description = description;

                return transaction.Commit() ? "Added emergency number." : "Failed to add emergency number.";
            }
        }

        public string Update(int id, string description, string number, string isInternal)
        {
            if (number == "") return "Please specify a valid number.";
            if (description == "") return "Please specify a valid description.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var emergencyNumber = _modelRepository.GetFromId<IEmergencyNumber>(id);

                emergencyNumber.Number = number;
                emergencyNumber.Description = description;

                return transaction.Commit() ? "Updated emergency number." : "Failed to update emergency number.";
            }
        }

        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var number = _modelRepository.GetFromId<IEmergencyNumber>(id);
                number.Delete();
                return transaction.Commit() ? "Deleted emergency number." : "Failed to delete emergency number.";
            }
        }

        public JsonResult EmergencyNumberData()
        {
            var emergencyData = _modelRepository.GetList<IEmergencyNumber>();
            
            return Json(new EmergencyNumberJsonViewModel(emergencyData), JsonRequestBehavior.AllowGet);
        }
    }
}