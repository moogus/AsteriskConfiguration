using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.JsonViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoutingAdminController : Controller
    {
        private readonly IRepository _modelRepository;

        public RoutingAdminController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public ActionResult Index()
        {
            var dialPlansForRouting = _modelRepository.GetList<IDialplan>().Where(
                d => !d.Name.StartsWith("uncondition") &&
                !d.Name.StartsWith("onBusy") &&
                !d.Name.StartsWith("noAnwser") &&
                !d.Name.Equals("ddiDefault") &&
                !d.Name.Equals("fallThrough") &&
                !d.Name.Equals("24hour7Days"));

            return View(dialPlansForRouting);
        }

        public string AddRange(string id, int priority, string days, string time, string plan)
        {
            if (id != "") return "Invalid selection.";

            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var range = _modelRepository.Add<IDialplanRange>();

                range.DaysOfWeek = days;
                range.TimeRange = time;
                range.Dialplan = _modelRepository.GetFromName<IDialplan>(plan);
                range.Priority = priority;

                return transaction.Commit() ? "Added new dialplan range." : "Failed to add new dialplan range.";
            }
        }

        public string AddDate(string id, string date, string plan)
        {
            if (id != "") return "Invalid selection.";
            
            //Transaction is managed from inside the method calls below

            return date.Contains("-")
                ? AddDateMultiRange(date, plan) 
                : AddDateSingle(DateTime.Parse(date), plan);
        }

        public string UpdateRange(int id, int priority, string days, string time, string plan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var range = _modelRepository.GetFromId<IDialplanRange>(id);

                range.DaysOfWeek = days;
                range.TimeRange = time;
                range.Dialplan = _modelRepository.GetFromName<IDialplan>(plan);
                range.Priority = priority;

                return transaction.Commit() ? "Updated range." : "Failed to update range.";
            }
        }

        public string UpdateDate(int id, string date, string plan)
        {
            var planDate = _modelRepository.GetFromId<IDialplanDate>(id);

            return date.Contains('-') ? UpdateDateRange(id, date, plan) : UpdateSingleDate(planDate, date, plan);
        }

        public string DeleteRange(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var range = _modelRepository.GetFromId<IDialplanRange>(id);
                range.Delete();

                return transaction.Commit() ? "Deleted range." : "Failed to delete range.";
            }
        }

        public string DeleteDate(int id)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var planDate = _modelRepository.GetFromId<IDialplanDate>(id);
                planDate.Delete();

                return transaction.Commit() ? "Deleted date." : "Failed to delete date.";
            }
        }

        public JsonResult DialPlanRangeData()
        {
            var dialplanRanges = _modelRepository.GetList<IDialplanRange>();

            //TODO: This needs some further work as there is an NHibernate issue. See DialPlanRangeJsonViewModel.cs
            return Json(new DialPlanRangeJsonViewModel(dialplanRanges), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DialPlanDateData()
        {
            var dialplanDates = _modelRepository.GetList<IDialplanDate>();

            return Json(new DialPlanDateJsonViewModel(dialplanDates), JsonRequestBehavior.AllowGet);           
        }

        private static List<DateTime> GetDateRange(string date)
        {
            return date.Split('-').Select(DateTime.Parse).ToList();
        }

        public string AddDateSingle(DateTime date, string plan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var planDate = _modelRepository.Add<IDialplanDate>();

                planDate.StartDate = date;
                planDate.EndDate = date;
                planDate.Dialplan = _modelRepository.GetFromName<IDialplan>(plan);

                return transaction.Commit() ? "Added date." : "Failed to add date.";
            }
        }

        private string AddDateMultiRange(string date, string plan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var datesToAdd = GetDateRange(date);

                var planDate = _modelRepository.Add<IDialplanDate>();
                planDate.StartDate = datesToAdd[0];
                planDate.EndDate = datesToAdd[1];
                planDate.Dialplan = _modelRepository.GetFromName<IDialplan>(plan);

                return transaction.Commit() ? "Added date range." : "Failed to add date range.";
            }
        }


        public string UpdateSingleDate(IDialplanDate planDate, string date, string plan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                planDate.StartDate = DateTime.Parse(date);
                planDate.EndDate = DateTime.Parse(date);
                planDate.Dialplan = _modelRepository.GetFromName<IDialplan>(plan);

                return transaction.Commit() ? "Updated date." : "Failed to update date.";
            }
        }

        public string UpdateDateRange(int id, string date, string plan)
        {
            var transaction = _modelRepository.ModelTransaction();

            using (transaction)
            {
                var datesToAdd = GetDateRange(date);

                var planDate = _modelRepository.GetFromId<IDialplanDate>(id);
                planDate.StartDate = datesToAdd[0];
                planDate.EndDate = datesToAdd[1];
                planDate.Dialplan = _modelRepository.GetFromName<IDialplan>(plan);

                return transaction.Commit() ? "Updated date range." : "Failed to update date range.";
            }
        }
    }
}