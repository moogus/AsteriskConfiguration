using System.Windows;

namespace AsteriskCTIClient.ApplicationResource.Singleton
{
  public class OurSingleArgs
  {
    public string[] Args { get; private set; }

    public OurSingleArgs(string s)
    {
      Args = new string[1];
      Args[0] = s;
    }

    public OurSingleArgs(StartupEventArgs e)
    {
      Args = e.Args;
    }
  }
}