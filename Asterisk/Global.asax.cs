using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Asterisk
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  //TODO: remove the commented code below:::::::::

  public class MvcApplication : HttpApplication
  {
    private static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }

    private static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
        "Default", // Route name
        "{controller}/{action}/{id}", // URL with parameters
        new {controller = "ExtensionAdmin", action = "Index", id = UrlParameter.Optional} // Parameter defaults
        );
    }

    private void Application_Error(object sender, EventArgs e)
    {
#if (RELEASE)
      EmailService.SendError(Server.GetLastError());
#endif
    }


    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);
    }
  }

  internal class EmailService
  {
    public static void SendError(Exception error)
    {
      var client = new SmtpClient("online2")
        {
          Credentials = new NetworkCredential("SALISBURY\administrator", "r0-sw=as")
        };
      string message = "Error occurred at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "\n";
      message += "Error: " + error.Message + "\n";
      message += "\n";
      message += "Stack: " + error.StackTrace + "\n";
      if (error.InnerException != null)
      {
        message += GetInnerExceptionDetails(error.InnerException);
      }
      var mail = new MailMessage("PhoneSystem@4com.co.uk", "darren.corbett@4com.co.uk",
                                 "Website Error (Web Phone Interface)", message);
      try
      {
        client.Send(mail);
      }
      catch (Exception exception)
      {
        //need to do something here. perhaps log out to file??
      }
    }

    private static string GetInnerExceptionDetails(Exception innerException)
    {
      string result = string.Empty;
      result += "\n";
      result += "Inner Error: " + innerException.Message;
      result += "\n";
      if (innerException.InnerException != null)
      {
        result += GetInnerExceptionDetails(innerException.InnerException);
      }
      return result;
    }
  }
}