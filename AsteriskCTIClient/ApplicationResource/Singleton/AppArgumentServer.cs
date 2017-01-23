using System;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;

namespace AsteriskCTIClient.ApplicationResource.Singleton
{
 public  class AppArgumentServer
  {
   private readonly string _appInstanceId;
   private readonly Action<string> _doDial;

   public AppArgumentServer(string appInstanceId, Action<string> doDial)
   {
     _appInstanceId = appInstanceId;
     _doDial = doDial;
     StartArgsOnNewThread();
   }

   private void StartArgsOnNewThread()
   {
     var bgWorker = new BackgroundWorker();
     bgWorker.DoWork += (s, ee) => ListenForNewArgs();
     bgWorker.RunWorkerAsync();
   }

   private void ListenForNewArgs()
   {
     using (var server = new NamedPipeServerStream(_appInstanceId))
     {
       using (var reader = new StreamReader(server))
       {
         while (true)
         {
           server.WaitForConnection();

           var argument = string.Empty;
           while (server.IsConnected)
           {
             argument += reader.ReadLine();
           }

           _doDial(argument);

           server.Disconnect();
         }
       }
     }
   }

  }
}
