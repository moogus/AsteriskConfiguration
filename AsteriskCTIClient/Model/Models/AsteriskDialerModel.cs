using System.Configuration;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Dial;

namespace AsteriskCTIClient.Model.Models
{
  public class AsteriskDialerModel : IDialerModel
  {
    private readonly IDialer _dialer;
    private readonly string _extension;

    private readonly IValidNumber _validPhoneNumber;

    public AsteriskDialerModel(IDialer dialer, IValidNumber validPhoneNumber)
    {
      _dialer = dialer;
      _validPhoneNumber = validPhoneNumber;
      _extension = ConfigurationManager.AppSettings.Get("Extension");
    }

    public void DialNumber(string numberToDial)
    {
      if (!_validPhoneNumber.IsValidNumber(numberToDial)) return;
      _dialer.AutoDial(_extension,numberToDial);
    }
  }
}
