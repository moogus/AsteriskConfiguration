using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Asterisk.Utilities.Interfaces;
using DatabaseAccess;

namespace Asterisk.Utilities
{
  public class FtpSoundFileAction : IFtpActions
  {
    private const string File = "//usr//share//asterisk//sounds//en//";
    private readonly IRepository _repository;
    private readonly string _serverDirectory;
    private const int Length = 1024;
    private readonly IServer _server;

    public FtpSoundFileAction(IRepository repository)
    {
      _repository = repository;
      _server = _repository.GetList<IServer>().First();
      _serverDirectory = string.Format("ftp://{0}{1}", _server.IpAddress, File);
    }

    public bool DownLoad(string clientLocation, string selectedFile)
    {
      var request = new WebClient {Credentials = _server.Credentials};

      if (!Directory.Exists(clientLocation))
      {
        Directory.CreateDirectory(clientLocation);
      }

      var fileData = request.DownloadData(string.Format("{0}{1}", _serverDirectory, selectedFile));
      var file = System.IO.File.Create(string.Format("{0}\\{1}", clientLocation, selectedFile));

      using (file)
      {
        file.Write(fileData, 0, fileData.Length);
        file.Close();
        return true;
      }
    }

    public bool Upload(HttpPostedFileBase fileInput)
    {
      var request = (FtpWebRequest) WebRequest.Create(new Uri(_serverDirectory + fileInput.FileName));

      request.Method = WebRequestMethods.Ftp.UploadFile;
      request.Credentials = _server.Credentials;

      var ftpStream = request.GetRequestStream();

      using (ftpStream)
      {
        var file = fileInput.InputStream;
        var buffer = new byte[Length];
        var bytesread = 0;
        do
        {
          bytesread = file.Read(buffer, 0, Length);
          ftpStream.Write(buffer, 0, bytesread);
        } while (bytesread != 0);
        file.Close();
        ftpStream.Close();

        return true;
      }
    }
  }
}