using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  public class SnomPhoneActionsController : Controller
  {
     private readonly IRepository _repository;

     public SnomPhoneActionsController(IRepository repository)
    {
      _repository = repository;
    }

    public void SetDoNotDialState(string extension)
    {
      var getIp = _repository.GetFromName<IExtension>(extension).IpAddress;

      var url = string.Format(@"http://{0}/command.htm?key=DND", getIp);

      var webClient = new WebClient();
      using (webClient)
      {
        using (webClient.OpenRead(url))
        {
        }
      }

    }

  }
}
