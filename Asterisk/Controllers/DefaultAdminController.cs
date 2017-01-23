using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class DefaultAdminController : Controller
    {
        private readonly IRepository _modelRepository;

        public DefaultAdminController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public string Add(string defaultValue)
        {
            return "";
        }

        public string Update(int id, string defaultValue)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var thisDefault = _modelRepository.GetFromId<IDefault>(id);
                thisDefault.DefaultValue = defaultValue;

                return transaction.Commit() ? "The default value has been updated." : "Failed to update the default value.";
            }
        }

        public string Delete(int id)
        {
            return "";
        }

        public JsonResult DefaultData()
        {
            var defaults = _modelRepository.GetList<IDefault>().Where(d => !string.IsNullOrEmpty(d.DefaultValue));

            return Json(new DefaultJsonViewModel(defaults), JsonRequestBehavior.AllowGet);
        }
    }
}