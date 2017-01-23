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
    public class FederationController : Controller
    {
        private readonly IRepository _modelRepository;
        private readonly ITrunkHelper _trunkHelper;

        public FederationController(IRepository modelRepository)
        {
            _modelRepository = modelRepository;
            _trunkHelper = new TrunkHelper(_modelRepository);
        }

        public ActionResult Index()
        {
            return View();
        }

        public string Add(string name, string description, string fedType, string accessCode, string info)
        {
            //TODO: ensure trunk names are unique !!!! otherwise will not work for SIP in asterisk
            if (!string.IsNullOrEmpty(name) && _modelRepository.GetFromName<IFederation>(name) == null &&
                !string.IsNullOrEmpty(accessCode) && !string.IsNullOrEmpty(fedType) && !string.IsNullOrEmpty(info))
            {
                var transaction = _modelRepository.ModelTransaction();
                using (transaction)
                {
                    var trunk = GetTrunkOnAdd(name, fedType, accessCode, info);

                    bool hasValues;
                    switch (SetFederationType(fedType))
                    {
                        case FederationType.FourCom:
                            var fed4Com = _modelRepository.Add<IFourComFederatedLink>();
                            fed4Com.Trunk = trunk;
                            fed4Com.Name = name;
                            fed4Com.Description = description;
                            hasValues = fed4Com.SetFederationValues(name, accessCode, info.Split(',')[2].Trim());
                            break;

                        case FederationType.Samsung:
                            var fedSamsung = _modelRepository.Add<ISamsungFederatedLink>();
                            fedSamsung.Trunk = trunk;
                            fedSamsung.Name = name;
                            fedSamsung.Description = description;
                            hasValues = fedSamsung.SetFederationValues(name, accessCode, info.Split(',')[2].Trim());
                            break;

                        default:
                            var federation = _modelRepository.Add<IFederation>();
                            hasValues = false;
                            break;
                    }

                    if (hasValues)
                    {
                        return transaction.Commit() ? string.Format("added federation link {0}", name) : "something went wrong";
                    }
                }
            }

            return "something went wrong";
        }

        public string Update(int id, string name, string description, string fedType, string accessCode, string info)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(accessCode) &&
                !string.IsNullOrEmpty(fedType) && !string.IsNullOrEmpty(info))
            {
                var transaction = _modelRepository.ModelTransaction();
                using (transaction)
                {
                    bool hasValues;
                    switch (SetFederationType(fedType))
                    {
                        case FederationType.FourCom:
                            var fed4Com = _modelRepository.GetFromId<IFourComFederatedLink>(id);
                            fed4Com.Trunk = SetTrunkValuesOnupdate(fedType, info, fed4Com.Trunk.Id, accessCode, name);
                            fed4Com.Name = name;
                            fed4Com.Description = description;
                            hasValues = fed4Com.SetFederationValues(name, accessCode, info.Split(',')[2].Trim());
                            break;

                        case FederationType.Samsung:
                            var fedSamsung = _modelRepository.GetFromId<ISamsungFederatedLink>(id);
                            fedSamsung.Trunk = SetTrunkValuesOnupdate(fedType, info, fedSamsung.Trunk.Id, accessCode, name);
                            fedSamsung.Name = name;
                            fedSamsung.Description = description;
                            hasValues = fedSamsung.SetFederationValues(name, accessCode, info.Split(',')[2].Trim());
                            break;

                        default:
                            var federation = _modelRepository.Add<IFederation>();
                            hasValues = false;
                            break;
                    }

                    if (hasValues)
                    {
                        return transaction.Commit() ? string.Format("added federation link {0}", name) : "something went wrong";
                    }
                }
            }

            return "something went wrong";
        }

        public string Delete(int id)
        {
            var transaction = _modelRepository.ModelTransaction();
            using (transaction)
            {
                var comFed = _modelRepository.GetFromId<IFourComFederatedLink>(id);
                var samFed = _modelRepository.GetFromId<ISamsungFederatedLink>(id);
                string fedName = string.Empty;

                if (samFed == null)
                {
                    DeleteFourComFederation(comFed);
                    fedName = comFed.Name;
                }

                if (comFed == null)
                {
                    DeleteSamsungFederation(samFed);
                    fedName = samFed.Name;
                }

                return transaction.Commit() ? string.Format("deleted federation link {0}", fedName) : "something went wrong";
            }
        }

        public JsonResult FederationData()
        {
            var fourComFederationData = _modelRepository.GetList<IFourComFederatedLink>();
            var sansumgFederationData = _modelRepository.GetList<ISamsungFederatedLink>();
            return Json(new FederationJsonViewModel(fourComFederationData, sansumgFederationData, new TrunkValueGenerator(_modelRepository)),
                        JsonRequestBehavior.AllowGet);
        }

        private static TrunkType SetTrunkType(string fedType)
        {
            //TODO: this needs to be more extensible
            return fedType.Equals("Samsung") ? TrunkType.Sip : TrunkType.Iax;
        }

        private static FederationType SetFederationType(string fedType)
        {
            return fedType.Equals("Samsung") ? FederationType.Samsung : FederationType.FourCom;
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
            foreach (var ac in _modelRepository.GetList<IAccessCode>().Where(a => a.ParentTrunk.Id == parentTrunkId))
            {
                ac.Delete();
            }
        }

        private ITrunk GetTrunkOnAdd(string name, string fedType, string accessCode, string info)
        {
            ITrunk trunk;
            switch (SetTrunkType(fedType))
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
            trunk.DefaultDestination = string.Empty;
            SetAccessCodesOnTheTrunk(accessCode, trunk);
            return trunk;
        }

        private ITrunk SetTrunkValuesOnupdate(string fedType, string info, int trunkId, string accessCode, string name)
        {
            ITrunk trunk;
            switch (SetTrunkType(fedType))
            {
                case TrunkType.Sip:
                    trunk = _trunkHelper.GetSipTrunkValues(info, trunkId);
                    trunk.TrunkType = TrunkType.Sip;
                    break;
                case TrunkType.Bri:
                    trunk = _trunkHelper.GetBriTrunkValues(info, trunkId);
                    trunk.TrunkType = TrunkType.Bri;
                    break;
                case TrunkType.Iax:
                    trunk = _trunkHelper.GetIaxTrunkValues(info, trunkId);
                    trunk.TrunkType = TrunkType.Iax;
                    break;
                default:
                    trunk = _modelRepository.GetFromId<ITrunk>(trunkId);
                    break;
            }
            trunk.Name = name;
            trunk.DefaultDestination = string.Empty;
            SetAccessCodesOnTheTrunk(accessCode, trunk);

            return trunk;
        }

        private void DeleteFourComFederation(IFourComFederatedLink comFed)
        {
            ITrunk trunk;
            switch (comFed.Trunk.TrunkType)
            {
                case TrunkType.Sip:
                    trunk = _modelRepository.GetFromId<ISipTrunk>(comFed.Trunk.Id);

                    break;
                case TrunkType.Bri:
                    trunk = _modelRepository.GetFromId<IBriTrunk>(comFed.Trunk.Id);

                    break;
                case TrunkType.Iax:
                    trunk = _modelRepository.GetFromId<IIaxTrunk>(comFed.Trunk.Id);

                    break;
                default:
                    trunk = _modelRepository.GetFromId<ITrunk>(comFed.Trunk.Id);
                    break;
            }
            comFed.Delete();

            DeleteTrunkAccessCodes(trunk.Id);
            trunk.Delete();
        }

        private void DeleteSamsungFederation(ISamsungFederatedLink samFed)
        {
            ITrunk trunk;
            switch (samFed.Trunk.TrunkType)
            {
                case TrunkType.Sip:
                    trunk = _modelRepository.GetFromId<ISipTrunk>(samFed.Trunk.Id);

                    break;
                case TrunkType.Bri:
                    trunk = _modelRepository.GetFromId<IBriTrunk>(samFed.Trunk.Id);

                    break;
                case TrunkType.Iax:
                    trunk = _modelRepository.GetFromId<IIaxTrunk>(samFed.Trunk.Id);

                    break;
                default:
                    trunk = _modelRepository.GetFromId<ITrunk>(samFed.Trunk.Id);
                    break;
            }

            DeleteTrunkAccessCodes(trunk.Id);
            trunk.Delete();
            samFed.Delete();
        }

    }
}