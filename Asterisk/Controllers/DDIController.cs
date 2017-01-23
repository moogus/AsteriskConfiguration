using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    public class DDIController : Controller
    {
        private readonly IRepository _modelRepository;

        public DDIController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(_modelRepository.GetList<ITrunk>());
        }

        [Authorize(Roles = "admin")]
        public string Add(string ddi, string trunk)
        {
            if (ddi == "") return "Invalid DDI supplied.";
            if (_modelRepository.GetFromName<IDDI>(ddi) != null) return "The DDI is already in use.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var d = _modelRepository.Add<IDDI>();

                d.DDINumber = ddi;
                d.Trunk = !string.IsNullOrEmpty(trunk)
                            ? _modelRepository.GetFromName<ITrunk>(trunk)
                            : _modelRepository.Add<ITrunk>();

                return transaction.Commit() ? "Added the new DDI." : "Failed to add the new DDI.";
            }            
        }

        [Authorize(Roles = "admin")]
        public string Update(int id, string ddi, string trunk)
        {
            var d = _modelRepository.GetFromId<IDDI>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                d.DDINumber = ddi;
                d.Trunk = !string.IsNullOrEmpty(trunk)
                              ? _modelRepository.GetFromName<ITrunk>(trunk)
                              : _modelRepository.Add<ITrunk>();

                return transaction.Commit() ? "Updated the DDI." : "Failed to update the DDI.";
            }
        }

        [Authorize(Roles = "admin")]
        public string Delete(int id)
        {
            var ddi = _modelRepository.GetFromId<IDDI>(id);

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                foreach (var r in _modelRepository.GetList<IRoutingRule>().Where(r => r.Number == ddi.DDINumber))
                {
                    r.Delete();
                }

                RemoveDDIFromExtensionAndQueues(ddi);
                
                ddi.Delete();

                return transaction.Commit() ? "Deleted the DDI." : " Failed to delete the DDI.";
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddRangeDDI(string ddiFrom, string ddiTo)
        {
            if (ddiFrom == "" || ddiTo == "") RedirectToAction("Index");            

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var ddF = int.Parse(ddiFrom);
                var ddt = int.Parse(ddiTo);

                for (var i = ddF; i < ddt + 1; i++)
                {
                    var ddiNumber = i.ToString(CultureInfo.InvariantCulture);
                    if (_modelRepository.GetFromName<IDDI>(ddiNumber) == null) continue;

                    var d = _modelRepository.Add<IDDI>();
                    d.DDINumber = ddiNumber;
                }

                transaction.Commit();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin,user")]
        public JsonResult DDIData()
        {
            var ddiData = _modelRepository.GetList<IDDI>();

            return Json(new DDIJsonViewModel(ddiData), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,user")]
        public JsonResult AvailableDDIs()
        {
            return
              Json(
                _modelRepository.GetList<IDDI>()
                           .Where(d => d.UsedOn == DDIUsedOn.NotUsed || d.UsedOn == DDIUsedOn.Default)
                           .Select(d => d.DDINumber), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        private void RemoveDDIFromExtensionAndQueues(IDDI ddi)
        {
            foreach (var q in _modelRepository.GetList<IQueue>().Where(q => q.DDI == ddi))
            {
                q.DDI = null;
            }

            foreach (var e in _modelRepository.GetList<IExtension>().Where(e => e.DDI == ddi))
            {
                e.DDI = null;
            }
        }
    }
}