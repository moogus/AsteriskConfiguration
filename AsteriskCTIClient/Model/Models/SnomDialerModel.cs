using System.ComponentModel;
using System.Configuration;
using System.Net;
using AsteriskCTIClient.Annotations;
using AsteriskCTIClient.Model.ModelInterfaces;
using DatabaseAccess;

namespace AsteriskCTIClient.Model.Models
{
  public class SnomDialerModel : INotifyPropertyChanged, IDialerModel
  {
    private readonly IExtension _myExtension;
    private readonly IRepository _repository;
    private readonly IValidNumber _validate;

    public SnomDialerModel(IRepository repository, IValidNumber validPhoneNumber)
    {
      _repository = repository;
      _validate = validPhoneNumber;

      string extension = ConfigurationManager.AppSettings.Get("Extension");
      if (!string.IsNullOrEmpty(extension))
      {
        _myExtension = _repository.GetFromName<IExtension>(extension);
      }
    }

    public void DialNumber(string numberToDial)
    {
      if (!_validate.IsValidNumber(numberToDial))return;
    
        FireAndForgetSnomHtmlEvent(string.Format(@"http://{0}/command.htm?number={1}&outgoing_uri=URI",
                                                _myExtension.IpAddress, _validate.Number)); 
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private static void FireAndForgetSnomHtmlEvent(string url)
    {
      //creates a fire and forget disposed web request
      var webClient = new WebClient();
      using (webClient)
      {
        using (webClient.OpenRead(url))
        {
        }
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}