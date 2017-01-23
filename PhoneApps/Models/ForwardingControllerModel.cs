using PhoneApps.Models.Interfaces;
using PhoneApps.Services.Interfaces;

namespace PhoneApps.Models
{
  public class ForwardingControllerModel : IForwardingControllerModel
  {
    public ForwardingControllerModel(IGetExtensionFromIp getExtensionFromIp, IGetForwardingFromExtension getForwardingFromExtension, 
      IGetForwardingModelList getForwardingModelList, ISaveForwardingType saveForwardingType)
    {
      GetExtensionFromIp = getExtensionFromIp;
      GetForwardingFromExtension = getForwardingFromExtension;
      GetForwardingModelList = getForwardingModelList;
      SaveForwardingType = saveForwardingType;   
    }

    public IGetExtensionFromIp GetExtensionFromIp { get; private set; }
    public IGetForwardingFromExtension GetForwardingFromExtension { get; private set; }
    public IGetForwardingModelList GetForwardingModelList { get; private set; }
    public ISaveForwardingType SaveForwardingType { get; private set; }
  }
}