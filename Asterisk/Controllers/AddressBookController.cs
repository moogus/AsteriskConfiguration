using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Asterisk.ControllerHelpers.Directory.Manger;
using Asterisk.ControllerHelpers.Directory.Manger.Interfaces;
using Asterisk.ControllerHelpers.Directory.Source;
using Asterisk.ControllerHelpers.Directory.Source.Interfaces;
using ModelRepository;

namespace Asterisk.Controllers
{
  public class AddressBookController : Controller
  {
    private readonly IDirectoryManager _directoryManager;

    public AddressBookController(IRepository modelRepository)
    {
      var listOfSources = new List<IDirectorySource>
        {
          new AsteriskDirectorySource(modelRepository),
          new KnownNumberDirectorySource(modelRepository)
        };
      _directoryManager = new DirectoryManager(listOfSources);
    }

    public ActionResult FourSight()
    {
      Response.ContentType = "text/xml";
      return View(_directoryManager.GetDirectoryEntries(true).OrderBy(e => e.LastName));
    }
  }
}