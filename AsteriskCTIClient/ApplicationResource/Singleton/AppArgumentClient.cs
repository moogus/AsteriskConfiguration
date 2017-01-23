using System.IO;
using System.IO.Pipes;

namespace AsteriskCTIClient.ApplicationResource.Singleton
{
  public class AppArgumentClient
  {
    private readonly string _appInstanceId;

    public AppArgumentClient(string appInstanceId)
    {
      _appInstanceId = appInstanceId;
    }

    public void Send(string arg)
    {
      using (var client = new NamedPipeClientStream(_appInstanceId))
      using (var writer = new StreamWriter(client))
      {
        client.Connect(200);
        writer.WriteLine(arg);
      }
    }
  }
}
