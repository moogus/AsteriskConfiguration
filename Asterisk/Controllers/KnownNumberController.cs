using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class KnownNumberController : Controller
    {
        private readonly IRepository _modelRepository;

        public KnownNumberController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CSVError()
        {
            return View();
        }

        public string Add(string number, string description, string isInternal)
        {
            if (number == "") return "Please specify a valid number.";            

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var knownNumber = _modelRepository.Add<IKnownNumber>();

                knownNumber.Number = number;
                knownNumber.Description = description;
                knownNumber.IsInternal = !isInternal.EndsWith("no");

                return transaction.Commit() ? "Added number." : "Failed to add number.";
            }
        }

        public string Update(int id, string number, string description, string isInternal)
        {
            if (number == "") return "Please specify a valid number.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var knownNumber = _modelRepository.GetFromId<IKnownNumber>(id);

                knownNumber.Number = number;
                knownNumber.Description = description;
                knownNumber.IsInternal = !isInternal.EndsWith("no");

                return transaction.Commit() ? "Updated number." : "Failed to update number.";
            }
        }

        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var number = _modelRepository.GetFromId<IKnownNumber>(id);
                number.Delete();

                return transaction.Commit() ? "Deleted number." : "Failed to delete number.";
            }
        }

        public JsonResult KnownNumberData()
        {
            var ddiData = _modelRepository.GetList<IKnownNumber>();

            return Json(new KnownNumberJsonViewModel(ddiData), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadExtensionsByCSV(HttpPostedFileBase file)
        {
            var importData = new ImportDataFromCSV(file, Server);
            var importSuccess = true;

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                Action<DataRow> saveDataRow = (dataRow) =>
                    {
                        //TODO: Verify fields are valid.

                        var knownNumber = _modelRepository.Add<IKnownNumber>();
                        knownNumber.Number = dataRow[0].ToString();
                        knownNumber.Description = dataRow[1].ToString();
                        knownNumber.IsInternal = !dataRow[2].ToString().EndsWith("no");
                    };

                importSuccess &= importData.SaveCSVData(saveDataRow, 3);
                importSuccess &= transaction.Commit();
            }

            //TODO: Fix this redirect.
            return RedirectToAction(importSuccess ? "Index" : "CSVError");
        }
    }
}