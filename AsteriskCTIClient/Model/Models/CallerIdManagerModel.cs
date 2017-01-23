using System.ServiceModel;
using AsteriskCTIClient.GetComapnyDetails;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Call;
using DatabaseAccess;

namespace AsteriskCTIClient.Model.Models
{
  public class CallerIdManagerModel
  {
    private readonly ChannelFactory<ICompanyDetail> _channelFactory;
    private readonly IRepository _repository;

    public CallerIdManagerModel(IRepository repository, ChannelFactory<ICompanyDetail> channelFactory)
    {
      _repository = repository;
      _channelFactory = channelFactory;
    }

    private static bool IsExternalDialer(ICall call)
    {
      return call.Incoming && !string.IsNullOrEmpty(call.OtherEndNumber) && call.OtherEndNumber.Length > 5;
    }

    public ICallerIdModel GetCallerIdModel(ICall call)
    {
      if (IsExternalDialer(call))
      {
        return new ExternalKnownCompanyModel(_channelFactory);
      }

      return new AsteriskCallerIdModel(_repository);
    }
  }
}