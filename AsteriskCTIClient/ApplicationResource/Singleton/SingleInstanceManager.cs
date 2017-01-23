using System;
using System.Threading;
using System.Windows;

namespace AsteriskCTIClient.ApplicationResource.Singleton
{
  public class SingleInstanceManager
  {
    public delegate void SingleApplicationHandler(object sender, OurSingleArgs e);

    private readonly string _appIdentifier;
    private readonly Mutex _singleInstanceMutex;

    public SingleInstanceManager(IHasStartUpEvent app, string appIdentifier)
    {
      _appIdentifier = appIdentifier;
      _singleInstanceMutex = new Mutex(true, _appIdentifier);
      app.Startup += AppStartup;
    }

    public bool FirstInstance { get; private set; }

    private void AppStartup(object sender, StartupEventArgs e)
    {
      FirstInstance = _singleInstanceMutex.WaitOne(TimeSpan.Zero, true);
    }
  }
}

