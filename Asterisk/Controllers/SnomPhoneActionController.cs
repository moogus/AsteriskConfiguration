using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
  public class SnomPhoneActionController : Controller
  {
    private readonly IRepository _modelRepository;

    public SnomPhoneActionController(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
    }

    public void SetDoNotDialState(string ip)
    {
      /*the following string should be put in the ActionURL 'Setup Finished' of the snom phone 
      http://10.10.20.63/Asterisk/SnomPhoneAction/SetDoNotDialState?ip=$phone_ip*/

      if (!_modelRepository.GetList<IExtension>().First(e => e.IpAddress == ip).DND) return;

      //pause for the phone to finish loading
      Thread.Sleep(2000);

      var webClient = new WebClient();
      using (webClient)
      {
        using (webClient.OpenRead(string.Format(@"http://{0}/command.htm?key=DND", ip)))
        {
        }
      }
    }
  }
}