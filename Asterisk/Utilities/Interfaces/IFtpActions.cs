using System.Web;

namespace Asterisk.Utilities.Interfaces
{
  public interface IFtpActions
  {
    bool DownLoad(string clientLocation, string selectedFile);
    bool Upload(HttpPostedFileBase fileInput);
  }
}