using CTIServer.Call;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface ICtiCall : ICall
  {
    string CallerNumber { get; }
    string CallerName { get; }
    string Department { get; }
    bool IsvalidAsteriskExtension(string number);
  }
}