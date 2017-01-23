using System.Web.Mvc;
using Asterisk.ControllerHelpers.Trunk;
using Asterisk.ControllerHelpers.Trunk.Interfaces;
using Asterisk.JsonViewModels;
using Asterisk.Utilities;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class TrunkController : Controller
  {
    private readonly Repository _repository;
    private readonly ITrunkHelper _trunkHelper;

    public TrunkController(Repository repository)
    {
      _repository = repository;
      _trunkHelper = new TrunkHelper(_repository);
    }

    public ActionResult Index()
    {
      return View();
    }

    public string Add(string name, string accessCodes, string info, string destination)
    {
      //TODO: ensure trunk names are unique !!!! otherwise will not work for SIP in asterisk
      if (name != "" && _repository.GetFromName<ITrunk>(name) == null && !string.IsNullOrEmpty(accessCodes) &&
          !string.IsNullOrEmpty(info))
      {
        ITrunk trunk;
        var trunkType = _trunkHelper.GetTrunkType(info);
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
        trunk.DefaultDestination = destination == "undefined" ? string.Empty : destination;

        return trunk.SetAccessCodes(accessCodes) && trunk.Update()
                 ? string.Format("added Trunk {0}", name)
                 : "something went wrong";
      }
      return "something went wrong";
    }

    public string Update(int id, string name, string accessCodes, string info, string destination)
    {
      if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(accessCodes) && !string.IsNullOrEmpty(info))
      {
        ITrunk trunk;
        var trunkType = _trunkHelper.GetTrunkType(info);
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
            trunk = _repository.GetFromId<ITrunk>(id);
            break;
        }
        trunk.Name = name;
        trunk.DefaultDestination = destination == "undefined" ? string.Empty : destination;


        return trunk.SetAccessCodes(accessCodes) && trunk.Update()
                 ? string.Format("Updated Trunk {0}", name)
                 : "something went wrong";
      }
      return "something went wrong";
    }

    public string Delete(int id)
    {
      var trunk = _repository.GetFromId<ITrunk>(id);

      switch (trunk.TrunkType)
      {
        case TrunkType.Sip:
          var sipTrunk = _repository.GetFromId<ISipTrunk>(id);
          return sipTrunk.Delete() ? "Deleted trunk " + sipTrunk.Name : "something went wrong";
        case TrunkType.Bri:
          var briTrunk = _repository.GetFromId<IBriTrunk>(id);
          return briTrunk.Delete() ? "Deleted trunk " + briTrunk.Name : "something went wrong";
        case TrunkType.Iax:
          var iaxTrunk = _repository.GetFromId<IIaxTrunk>(id);
          return iaxTrunk.Delete() ? "Deleted trunk " + iaxTrunk.Name : "something went wrong";

        default:
          return trunk.Delete() ? "Deleted trunk " + trunk.Name : "something went wrong";
      }
    }

    public JsonResult TrunkData()
    {
      return Json(new TrunkJsonViewModel(_trunkHelper.GetValidTrunks(), new TrunkValueGenerator(_repository)),
                  JsonRequestBehavior.AllowGet);
    }
  }
}