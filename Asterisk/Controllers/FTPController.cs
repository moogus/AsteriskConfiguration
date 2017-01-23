using System.Web;
using System.Web.Mvc;
using Asterisk.Utilities;
using Asterisk.Utilities.Interfaces;
using Asterisk.ViewModels;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class FTPController : Controller
  {
    private readonly IFtpActions _ftpActions;
    private readonly IRepository _modelRepository;

    public FTPController(IRepository modelRepository)
    {
      _modelRepository = modelRepository;
      //TODO move into constructor.....
      _ftpActions = new FtpSoundFileAction(_modelRepository);
    }

    public string Download(string id, string location)
    {
      return _ftpActions.DownLoad(location,
                                  string.Format("{0}.gsm", _modelRepository.GetFromId<IAutoAttendant>(int.Parse(id)).Name))
               ? "<p  style='font-size: 20px;color: #027384; margin-left:90px; '>File&nbspdownloaded&nbspsuccessfully....</p>"
               : "<p  style='font-size: 20px;color: #027384; margin-left:90px; '>Something&nbspwent&nbspwrong....</p>";
    }

    [HttpGet]
    public ActionResult Uploader()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Uploader(HttpPostedFileBase file, string id)
    {
      string autoFile = !string.IsNullOrEmpty(id) ? _modelRepository.GetFromId<IAutoAttendant>(int.Parse(id)).Name : "";
      bool sucess = false;
      if (file != null && file.FileName.Equals(autoFile + ".gsm"))
      {
        sucess = _ftpActions.Upload(file);
      }
      return RedirectToAction("FtpResult", new {file = file != null ? file.FileName : "", isSucess = sucess});
    }

    [HttpGet]
    public ActionResult FtpResult(string file, bool isSucess)
    {
      var ftpVm = new FtpResultViewModel {File = file, IsSucess = isSucess};
      return View(ftpVm);
    }

    [HttpPost]
    public ActionResult FtpResult()
    {
      return RedirectToAction("Uploader");
    }
  }
}