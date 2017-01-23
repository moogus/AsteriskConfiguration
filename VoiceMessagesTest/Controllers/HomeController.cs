using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess;

namespace VoiceMessagesTest.Controllers
{
  public class HomeController : Controller
  {

    private readonly IRepository _repository;

    public HomeController()
    {
      _repository = new Repository();
    }

    public ActionResult Index()
    {
      var allVoiceMails = _repository.GetList<IVoiceMessage>().ToList();
      return View(allVoiceMails);
    }

   public ActionResult Download(int id)
    {
      var x = _repository.GetFromId<IVoiceMessage>(id);

      var cd = new System.Net.Mime.ContentDisposition
        {
          Inline=true,
          FileName="message.wav"
        };
      Response.AppendHeader("Content-Disposition", cd.ToString());

      return File(x.AudioStream, "audio/wav");
    }

    public ActionResult About()
    {
      return View();
    }
  }
}
