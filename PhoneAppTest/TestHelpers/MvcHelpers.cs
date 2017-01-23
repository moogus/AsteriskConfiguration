using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace PhoneAppTests.TestHelpers
{
  internal static class MvcHelpers
  {
    internal static NameValueCollection AddServerVarsTo(Controller controller)
    {
      var request = new Mock<HttpRequestBase>();
      var serverVariables = new NameValueCollection();
      request.Setup(r => r.ServerVariables).Returns(serverVariables);
      var httpContext = new Mock<HttpContextBase>();
      httpContext.Setup(h => h.Request).Returns(request.Object);
      controller.ControllerContext = new ControllerContext(new RequestContext(httpContext.Object, new RouteData()),
                                                           controller);
      return serverVariables;
    }
  }
}
