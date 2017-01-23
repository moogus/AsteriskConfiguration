using System.ServiceModel;
using AsteriskCTIClient.GetComapnyDetails;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Call;

namespace AsteriskCTIClient.Model.Models
{
  public class ExternalKnownCompanyModel : ICallerIdModel
  {
    private readonly ChannelFactory<ICompanyDetail> _channelFactory;

    public ExternalKnownCompanyModel(ChannelFactory<ICompanyDetail> channelFactory)
    {
      _channelFactory = channelFactory;
    }

    public string CallerNumber { get; private set; }
    public string CallerName { get; private set; }
    public string Department { get; private set; }

    public bool IsvalidAsteriskExtension(string number)
    {
      //TODO this needs to come out into CallerIdManager..I think 
      return true;
    }

    public void SetNumberAndName(ICall call)
    {
      CallerNumber = call.OtherEndNumber;
      Department = string.Empty;
      using (_channelFactory)
      {
        ICompanyDetail channel = _channelFactory.CreateChannel();

        CallerName = channel.GetCompanyFromNumber(call.OtherEndNumber);
      }
    }
  }
}