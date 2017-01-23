using CTIServer.Call;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface ICallerIdModel
  {
    string CallerNumber { get; }
    string CallerName { get; }
    string Department { get; }
    bool IsvalidAsteriskExtension(string number);
    void SetNumberAndName(ICall call);
  }
}