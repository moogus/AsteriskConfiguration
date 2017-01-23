using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DatabaseAccess;
using PhoneApps.Models.Interfaces;
using PhoneApps.Services.Interfaces;
using PhoneApps.ViewModels.Interfaces;

namespace PhoneApps.Controllers
{
  public class ForwardingController : Controller
  {
    private readonly IForwardingControllerModel _forwardingControllerModel;
    private readonly IGetExtensionFromIp _getExtensionFromIp;
    private readonly IGetForwardingFromExtension _getForwardingFromExtension;
    private readonly IGetForwardingModelList _getForwardingModelList;
    private string _serverIpAddress;
    private string _extension;
    private string _forwardingDialpan;

    public ForwardingController(IForwardingControllerModel forwardingControllerModel)
    {
      _forwardingControllerModel = forwardingControllerModel;
      _getExtensionFromIp = _forwardingControllerModel.GetExtensionFromIp;
      _getForwardingFromExtension = _forwardingControllerModel.GetForwardingFromExtension;
      _getForwardingModelList = _forwardingControllerModel.GetForwardingModelList;
    }

    public ActionResult Index()
    {
      GetExt();
      _forwardingDialpan = _getForwardingFromExtension.GetCurrentForwardingDialplan("unconditional");
      return View("Index", new ForwardingVM(_extension,
                                          _getForwardingFromExtension.GetForwardingType(_extension, _forwardingDialpan),
                                          _getForwardingFromExtension.GetForwardingNumber(_extension, _forwardingDialpan)));
    }

    public ActionResult GetForwardingRoutes(string type)
    {
      GetExt();
      _forwardingDialpan = _getForwardingFromExtension.GetCurrentForwardingDialplan("unconditional");
      return View("GetForwardingRoutes", new ForwardingRouteVM(_extension,
                                                              _getForwardingFromExtension.IsForwardingEnabled(_extension, _forwardingDialpan),
                                                              _getForwardingModelList.GetForwadingModelsFromType(GetTypefromValue(type)), type));
    }

    public ActionResult SaveForwardingRoute(string value)
    {
      GetExt();
      _forwardingControllerModel.SaveForwardingType.SaveForwardingType(value);
      _forwardingDialpan = _getForwardingFromExtension.GetCurrentForwardingDialplan("unconditional");
      var currentType = GetTypefromValue(value.Split(',')[1]);
      return View("SaveForwardingRoute", new ForwardingRouteVM(_extension,
                                                              _getForwardingFromExtension.IsForwardingEnabled(_extension, _forwardingDialpan),
                                                              _getForwardingModelList.GetForwadingModelsFromType(currentType), currentType.ToString()){ActionMessage = "Updated....."});
    }

    private ForwardingDestination GetTypefromValue(string value)
    {
      return (ForwardingDestination)Enum.Parse(typeof(ForwardingDestination), value);
    }

    private void GetExt()
    {
#if DEBUG
      _serverIpAddress = "10.10.20.2";
#else
        _serverIpAddress =Request.ServerVariables["REMOTE_ADDR"];
#endif
      _extension = _getExtensionFromIp.GetExtension(_serverIpAddress);
      _getForwardingFromExtension.SetBaseForwarding(_extension);
    }

    #region Private ViewModels

    private class ForwardingVM : IForwardingVM
    {
      public ForwardingVM(string currentExtension, ForwardingDestination currentRuleType, string currentRuleDestination)
      {
        CurrentExtension = currentExtension;
        RouteTypes = Enum.GetNames(typeof(ForwardingDestination)).ToList().Where(t => !t.Equals("Error"));
        CurrentRuleType = currentRuleType;
        CurrentRuleDestination = currentRuleDestination;
      }

      public ForwardingDestination CurrentRuleType { get; private set; }
      public string CurrentRuleDestination { get; private set; }
      public IEnumerable<string> RouteTypes { get; private set; }
      public string CurrentExtension { get; private set; }
    }

    private class ForwardingRouteVM : IForwardingRouteVM
    {
      public ForwardingRouteVM(string currentExtension, bool isEnabled, IEnumerable<IForwardingModel> routesInformation, string forwardingType)
      {
        CurrentExtension = currentExtension;
        IsEnabled = isEnabled;
        RoutesInformation = routesInformation;
        ForwardingType = (ForwardingDestination)Enum.Parse(typeof(ForwardingDestination), forwardingType);
      }

      public string CurrentExtension { get; private set; }
      public bool IsEnabled { get; private set; }
      public IEnumerable<IForwardingModel> RoutesInformation { get; private set; }
      public ForwardingDestination ForwardingType { get; private set; }
      public string ActionMessage { get;  set; }
    }

    #endregion

  }
}
