using System;
using System.Web;
using System.Web.Mvc;
using Asterisk.Utilities;
using Asterisk.Utilities.Interfaces;
using Asterisk.ViewModels;
using DatabaseAccess;

namespace Asterisk.Controllers
{
  [Authorize(Roles = "admin")]
  public class FTPController : Controller
  {
    private readonly IRepository _repository;
    private readonly IFtpActions _ftpActions;

    public FTPController(IRepository repository)
    {
      _repository = repository;
      //TODO move into constructor.....
      _ftpActions = new FtpSoundFileAction(_repository);
    }

    public string Download(string id, string location)
    {
      return _ftpActions.DownLoad(location,
                                  string.Format("{0}.gsm", _repository.GetFromId<IAutoAttendant>(int.Parse(id)).Name))
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
      var autoFile = !string.IsNullOrEmpty(id) ? _repository.GetFromId<IAutoAttendant>(int.Parse(id)).Name : "";
      var sucess = false;
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