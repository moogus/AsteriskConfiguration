using DatabaseAccess;
using PhoneApps.Services.Interfaces;
using System.Linq;

namespace PhoneApps.Services
{
  public  class GetExtensionFromIp : IGetExtensionFromIp
  {
    private readonly IRepository _repository;

    public GetExtensionFromIp(IRepository repository)
    {
      _repository = repository;
    }

    public string GetExtension(string ip)
    {
      return _repository.GetList<IExtension>().First(e => e.IpAddress == ip).Number;
    }
  }
}