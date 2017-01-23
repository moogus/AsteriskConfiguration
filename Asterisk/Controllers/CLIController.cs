using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    public class CLIController : Controller
    {
        private readonly IRepository _modelRepository;

        public CLIController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return RedirectToAction("CLIItems");
        }

        [Authorize(Roles = "admin")]
        public ActionResult CSVError()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult CLIItems()
        {
            IEnumerable<string> trunkList = _modelRepository.GetList<ITrunk>().Select(t => t.Name);
            return View(trunkList);
        }

        [Authorize(Roles = "admin")]
        public string Add(string cliName, string cliNumber, string trunk)
        {
            if (cliName == "") return "Please specify a valid CLI name.";
            if (cliNumber == "") return "Please specify a valid CLI number.";
            if (trunk == "") return "Please select a valid trunk.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var cli = _modelRepository.Add<ICLI>();
                cli.CLIName = cliName;
                cli.CLINumber = cliNumber;
                cli.Trunk = _modelRepository.GetFromName<ITrunk>(trunk);

                return transaction.Commit() ? String.Format("Added CLI {0}", cliName) : String.Format("Failed to add CLI {0}", cliName);                
            }
        }

        [Authorize(Roles = "admin")]
        public string Update(int id, string cliName, string cliNumber, string trunk)
        {
            if (cliName == "") return "Please specify a valid CLI name.";
            if (cliNumber == "") return "Please specify a valid CLI number.";
            if (trunk == "") return "Please select a valid trunk.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var cli = _modelRepository.GetFromId<ICLI>(id);

                cli.CLIName = cliName;
                cli.CLINumber = cliNumber;
                cli.Trunk = _modelRepository.GetFromName<ITrunk>(trunk);

                return transaction.Commit() ? String.Format("Updated CLI {0}", cliName) : String.Format("Failed to update {0}", cliName);
            }
        }

        [Authorize(Roles = "admin")]
        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var cli = _modelRepository.GetFromId<ICLI>(id);

                RemoveCLIFromExtensionsAndQueues(cli);

                cli.Delete();

                return transaction.Commit() ? String.Format("Deleted CLI {0}", cli.CLIName) : String.Format("Failed to delete {0}", cli.CLIName);
            }
        }

        [Authorize(Roles = "admin,user")]
        public JsonResult CLINumberData()
        {
            return Json(_modelRepository.GetList<ICLI>().OrderBy(c => c.Trunk).Select(c => c.CLINumber), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,user")]
        public JsonResult CLIData()
        {
            if (_modelRepository.GetList<ICLI>().Any())
            {
                var cliData = _modelRepository.GetList<ICLI>();//.OrderBy(c => c.Trunk.Id);

                return Json(new CLIJsonViewModel(cliData), JsonRequestBehavior.AllowGet);
            }

            return Json(new CLIJsonViewModel(_modelRepository.GetList<ICLI>()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult UploadCliByCSV(HttpPostedFileBase file)
        {
            var importData = new ImportDataFromCSV(file, Server);
            var importSuccess = true;

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                Action<DataRow> saveDataRow = (dataRow) =>
                    {
                        //TODO: Verify that the input fields are not empty.
                        var cli = _modelRepository.Add<ICLI>();
                        cli.CLIName = dataRow[0].ToString();
                        cli.CLINumber = dataRow[1].ToString();
                    };

                importSuccess &= importData.SaveCSVData(saveDataRow, 2);
                importSuccess &= transaction.Commit();
            }

            //TODO: Fix this redirect.
            return RedirectToAction(importSuccess ? "Index" : "CSVError");
        }

        [Authorize(Roles = "admin")]
        private void RemoveCLIFromExtensionsAndQueues(ICLI cli)
        {
            var allextension = _modelRepository.GetList<IExtension>().Where(e => e.CLI == cli);

            //NOTE: This does not need to be done as the calling body already opens a transaction.
            //var transaction = _modelRepository.ModelTransaction();
            //using (transaction)
            {
                foreach (var extension in allextension)
                {
                    extension.CLI = null;
                }

                var allQueues = _modelRepository.GetList<IQueue>().Where(q => q.CLI == cli);
                foreach (var queue in allQueues)
                {
                    queue.CLI = null;
                }
            }
        }
    }
}