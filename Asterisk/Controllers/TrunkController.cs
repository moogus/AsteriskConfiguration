using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.ControllerHelpers.Trunk;
using Asterisk.ControllerHelpers.Trunk.Interfaces;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
    [Authorize(Roles = "admin")]
    public class TrunkController : Controller
    {
        private readonly IRepository _modelRepository;
        private readonly ITrunkHelper _trunkHelper;

        public TrunkController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
            _trunkHelper = new TrunkHelper(_modelRepository);
        }

        public ActionResult Index()
        {
            return View();
        }

        public string Add(string name, string accessCodes, string info, string destination)
        {
            //TODO: ensure trunk names are unique !!!! otherwise will not work for SIP in asterisk
            if (name != "" && _modelRepository.GetFromName<ITrunk>(name) == null && !string.IsNullOrEmpty(accessCodes) &&
                !string.IsNullOrEmpty(info))
            {
                var transaction = _modelRepository.ModelTransaction();
                using (transaction)
                {
                    ITrunk trunk;
                    TrunkType trunkType = _trunkHelper.GetTrunkType(info);
                    switch (trunkType)
                    {
                        case TrunkType.Sip:
                            trunk = _trunkHelper.SetSipTrunkValues(info);
                            trunk.TrunkType = TrunkType.Sip;
                            break;
                        case TrunkType.Bri:
                            trunk = _trunkHelper.SetBriTrunkValues(info);
                            trunk.TrunkType = TrunkType.Bri;
                            break;
                        case TrunkType.Iax:
                            trunk = _trunkHelper.SetIaxTrunkValues(info);
                            trunk.TrunkType = TrunkType.Iax;
                            break;
                        default:
                            trunk = _modelRepository.Add<ITrunk>();
                            break;
                    }
                    trunk.Name = name;
                    trunk.DefaultDestination = destination == "undefined" ? string.Empty : destination;

                    SetAccessCodesOnTheTrunk(accessCodes, trunk);

                    return transaction.Commit() ? string.Format("added Trunk {0}", name) : "something went wrong";
                }

            }
            return "something went wrong";
        }



        public string Update(int id, string name, string accessCodes, string info, string destination)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(accessCodes) && !string.IsNullOrEmpty(info))
            {
                var transaction = _modelRepository.ModelTransaction();
                using (transaction)
                {
                    ITrunk trunk;
                    TrunkType trunkType = _trunkHelper.GetTrunkType(info);
                    switch (trunkType)
                    {
                        case TrunkType.Sip:
                            trunk = _trunkHelper.GetSipTrunkValues(info, id);
                            trunk.TrunkType = TrunkType.Sip;
                            break;
                        case TrunkType.Bri:
                            trunk = _trunkHelper.GetBriTrunkValues(info, id);
                            trunk.TrunkType = TrunkType.Bri;
                            break;
                        case TrunkType.Iax:
                            trunk = _trunkHelper.GetIaxTrunkValues(info, id);
                            trunk.TrunkType = TrunkType.Iax;
                            break;
                        default:
                            trunk = _modelRepository.GetFromId<ITrunk>(id);
                            break;
                    }
                    trunk.Name = name;
                    trunk.DefaultDestination = destination == "undefined" ? string.Empty : destination;

                    SetAccessCodesOnTheTrunk(accessCodes, trunk);

                    return transaction.Commit() ? string.Format("added Trunk {0}", name) : "something went wrong";
                }
            }
            return "something went wrong";
        }

        public string Delete(int id)
        {
            var trunk = _modelRepository.GetFromId<ITrunk>(id);
            var transaction = _modelRepository.ModelTransaction();
            using (transaction)
            {
                switch (trunk.TrunkType)
                {
                    case TrunkType.Sip:
                        var sipTrunk = _modelRepository.GetFromId<ISipTrunk>(id);
                        DeleteTrunkAccessCodes(trunk.Id);
                        sipTrunk.Delete();
                        break;
                    case TrunkType.Bri:
                        var briTrunk = _modelRepository.GetFromId<IBriTrunk>(id);
                        DeleteTrunkAccessCodes(trunk.Id);
                        briTrunk.Delete();
                        break;
                    case TrunkType.Iax:
                        var iaxTrunk = _modelRepository.GetFromId<IIaxTrunk>(id);
                        DeleteTrunkAccessCodes(trunk.Id);
                        iaxTrunk.Delete();
                        break;
                    default:
                        DeleteTrunkAccessCodes(trunk.Id);
                        trunk.Delete();
                        break;
                }

                return transaction.Commit() ? string.Format("deleted Trunk {0}", trunk.Name) : "something went wrong";
            }
        }

        public JsonResult TrunkData()
        {
            return Json(new TrunkJsonViewModel(_trunkHelper.GetValidTrunks(), new TrunkValueGenerator(_modelRepository)),
                        JsonRequestBehavior.AllowGet);
        }

        private void SetAccessCodesOnTheTrunk(string accessCodes, ITrunk trunk)
        {
            DeleteTrunkAccessCodes(trunk.Id);
            var allAccessCodes = GetAccessCodesAndPriority(accessCodes);

            foreach (var allAccessCode in allAccessCodes)
            {
                if (trunk.TrunkHasIAccessCode(allAccessCode.Item1, allAccessCode.Item2)) continue;
                var accessCode = _modelRepository.Add<IAccessCode>();
                accessCode.Code = allAccessCode.Item1;
                accessCode.Priority = allAccessCode.Item2;
                accessCode.ParentTrunk = trunk;
                trunk.AddAccessCodes(accessCode);
            }
        }

        private IEnumerable<Tuple<string, int>> GetAccessCodesAndPriority(string accessCode)
        {
            var allCode = string.IsNullOrEmpty(accessCode) ? new List<string>() : accessCode.Split(',').ToList();

            return allCode.Select(c => c.Split(':')).Select(cNd => (new Tuple<string, int>(cNd[0], int.Parse(cNd[1])))).ToList();
        }

        private void DeleteTrunkAccessCodes(int parentTrunkId)
        {
            var requiredCodes = _modelRepository.GetList<IAccessCode>().Where(a => a.ParentTrunk.Id == parentTrunkId);
            foreach (var ac in requiredCodes )
            {
                ac.Delete();
            }
        }

    }
}