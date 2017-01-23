using System.Web.Mvc;
using Asterisk.ControllerHelpers.Trunk;
using Asterisk.ControllerHelpers.Trunk.Interfaces;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class FederationController : Controller
  {
    private readonly IRepository _repository;
    private readonly ITrunkHelper _trunkHelper;

    public FederationController(IRepository repository)
    {
      _repository = repository;
      _trunkHelper = new TrunkHelper(_repository);
    }

    public ActionResult Index()
    {
      return View();
    }

    public string Add(string name, string description, string fedType, string accessCode, string info)
    {
      //TODO: ensure trunk names are unique !!!! otherwise will not work for SIP in asterisk


      if (!string.IsNullOrEmpty(name) && _repository.GetFromName<IFederation>(name) == null &&
          !string.IsNullOrEmpty(accessCode) && !string.IsNullOrEmpty(fedType) && !string.IsNullOrEmpty(info))
      {
        var trunkType = SetTrunkType(fedType);
        var federation = _repository.Add<IFederation>();
        ITrunk trunk;
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
            trunk = _repository.Add<ITrunk>();
            break;
        }
        trunk.Name = name;
        trunk.DefaultDestination = string.Empty;
        federation.Name = name;
        federation.Description = description;

        if (trunk.SetAccessCodes(accessCode) && trunk.Update())
        {
          // return UpdateFederation(name, fedType, accessCode, info, federation, trunk) ? string.Format("added federation link {0}", name) : "something went wrong";
          return UpdateFederation(info.Split(',')[1].Trim(), fedType, accessCode, info.Split(',')[2].Trim(), federation,
                                  trunk)
                   ? string.Format("added federation link {0}", name)
                   : "something went wrong";
        }
      }
      return "something went wrong";
    }

    private static bool UpdateFederation(string name, string fedType, string accessCode, string password,
                                         IFederation federation, ITrunk trunk)
    {
      federation.Type = SetFederationType(fedType);
      // var rtn = federation.SetFederationValues(name, accessCode, info.Split(',')[2].Trim(), trunk);
      var rtn = federation.SetFederationValues(name, accessCode, password, trunk);
      if (rtn)
      {
        rtn = federation.Update();
      }
      return rtn;
    }

    public string Update(int id, string name, string description, string fedType, string accessCode, string info)
    {
      if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(accessCode) &&
          !string.IsNullOrEmpty(fedType) && !string.IsNullOrEmpty(info))
      {
        var trunkType = SetTrunkType(fedType);

        var federation = _repository.GetFromId<IFederation>(id);
        ITrunk trunk;
        switch (trunkType)
        {
          case TrunkType.Sip:
            trunk = _trunkHelper.GetSipTrunkValues(info, federation.Trunk.Id);
            trunk.TrunkType = TrunkType.Sip;
            break;
          case TrunkType.Bri:
            trunk = _trunkHelper.GetBriTrunkValues(info, federation.Trunk.Id);
            trunk.TrunkType = TrunkType.Bri;
            break;
          case TrunkType.Iax:
            trunk = _trunkHelper.GetIaxTrunkValues(info, federation.Trunk.Id);
            trunk.TrunkType = TrunkType.Iax;
            break;
          default:
            trunk = _repository.GetFromId<ITrunk>(federation.Trunk.Id);
            break;
        }
        trunk.Name = name;
        trunk.DefaultDestination = string.Empty;
        federation.Name = name;
        federation.Description = description;

        if (trunk.SetAccessCodes(accessCode) && trunk.Update())
        {
          //return UpdateFederation(name, fedType, accessCode, info, federation, trunk) ? string.Format("added federation link {0}", name) : "something went wrong";
          return UpdateFederation(info.Split(',')[1].Trim(), fedType, accessCode, info.Split(',')[2].Trim(), federation,
                                  trunk)
                   ? string.Format("added federation link {0}", name)
                   : "something went wrong";
        }
      }
      return "something went wrong";
    }

    public string Delete(int id)
    {
      var fed = _repository.GetFromId<IFederation>(id);
      ITrunk trunk;
      switch (fed.Trunk.TrunkType)
      {
        case TrunkType.Sip:
          trunk = _repository.GetFromId<ISipTrunk>(fed.Trunk.Id);

          break;
        case TrunkType.Bri:
          trunk = _repository.GetFromId<IBriTrunk>(fed.Trunk.Id);

          break;
        case TrunkType.Iax:
          trunk = _repository.GetFromId<IIaxTrunk>(fed.Trunk.Id);

          break;
        default:
          trunk = _repository.GetFromId<ITrunk>(fed.Trunk.Id);
          break;
      }

      var rtn = true;

      if (trunk != null)
      {
        rtn = trunk.Delete();
      }

      if (fed.Extension != null && rtn)
      {
        rtn = fed.Extension.Delete();
      }

      if (fed.RoutingRule != null && rtn)
      {
        rtn = fed.RoutingRule.Delete();
      }

      return rtn && fed.Delete() ? "Deleted federation link " + fed.Name : "something went wrong";
    }

    public JsonResult FederationData()
    {
      var federationData = _repository.GetList<IFederation>();
      return Json(new FederationJsonViewModel(federationData, new TrunkValueGenerator(_repository)),
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
  }
}