using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess;

namespace PhoneApps.Controllers
{
    public class VoicemailController : Controller
    {
        //
        // GET: /Voicemail/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetFolders()
        {
            var repository = new Repository();
            var voicemail = repository.GetFromName<IVoiceMail>("2226");

            return Json(voicemail.MessageFolders.Select(v => v.FolderName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessages(string folder)
        {
            var repository = new Repository();
            var voicemail = repository.GetFromName<IVoiceMail>("2226");

            return Json(
                voicemail.MessageFolders.First(v=>v.FolderName==folder).FolderMessages.Select(m => new { FromNumber=m.CallerNumber, FromName=m.CallerId, Time=m.CalledAt.ToString("dd/MM/yyyy H:mm"), Duration=m.Duration, Id=m.Id}),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMessage(int id)
        {
            var repository = new Repository();
            
            var cd = new ContentDisposition
            {
                Inline = false,
                FileName = "message.mp3"
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            var infile = Path.GetTempFileName();
            var outfile = Path.GetTempFileName();
            using (var fs = new FileStream(infile, FileMode.OpenOrCreate))
            {
                repository.GetFromId<IVoiceMessage>(id).Audiostream.Stream.CopyTo(fs);
                fs.Close();
            }



            string lameEXE = @"C:\Lame\lame.exe";
            string lameArgs = "-V2";

            
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = lameEXE;
            process.StartInfo.Arguments = string.Format(
                "{0} {1} {2}",
                lameArgs,
                infile,
                outfile);

            process.Start();
            process.WaitForExit();

            int exitCode = process.ExitCode;



            return File(outfile, "audio/mpeg");
            //return File(repository.GetFromId<IVoiceMessage>(id).Audiostream.Stream, "audio/wav");
        }
    }
}
