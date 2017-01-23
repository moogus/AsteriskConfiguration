using PhoneApps.Services.Interfaces;

namespace PhoneApps.Models.Interfaces
{
  public interface IForwardingControllerModel
  {
    IGetExtensionFromIp GetExtensionFromIp { get; }
    IGetForwardingFromExtension GetForwardingFromExtension { get; }
    IGetForwardingModelList GetForwardingModelList { get; }
    ISaveForwardingType SaveForwardingType { get; }
  }
}