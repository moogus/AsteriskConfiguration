using System.Web.Mvc;
using PhoneApps.Controllers;
using PhoneApps.ViewModels.Interfaces;

namespace PhoneAppTests.TestHelpers
{
  internal static class GetActionResultForController
  {
    internal static IForwardingRouteVM GetGetForwardingRoutesViewModel(ForwardingController controller)
    {
      var view = controller.GetForwardingRoutes(new RandomGenerator().RandomString) as ViewResult;
      var model = view.Model as IForwardingRouteVM;
      return model;
    }

    internal static IForwardingVM GetIndexViewModel(ForwardingController controller)
    {
      var view = controller.Index() as ViewResult;
      var model = view.Model as IForwardingVM;
      return model;
    }

  }
}
