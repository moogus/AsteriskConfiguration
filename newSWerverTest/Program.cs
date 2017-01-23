using System;
using System.Linq;
using CTIServer.CallHandler.CallHandlers;
using CTIServer.ConnectionManger.ConnectionManagers;
using CTIServer.Phone.Phones;
using CTIServer.PhoneManager.PhoneManagers;
using DatabaseAccess;

namespace newServerTest
{
  class Program
  {
    static void Main(string[] args)
    {
      var exts = new Repository().GetList<IExtension>().Select(e => new AsteriskPhone(e.Number,e.DDINumber, e.FirstName + " " + e.LastName, e.Department)).ToList();

      var managerConnection = new AsteriskConnectionManager("10.10.20.188", 5038, "admin", "31994");
      var callHandler = new AsteriskCallHandler(managerConnection);
      var manager = new AsteriskPhoneManager(exts,managerConnection ,callHandler);

      var p = manager.GetPhone("2003");
      Console.WriteLine();
      Console.WriteLine();
      Console.WriteLine();

      //p.PropertyChanged += (o, a) =>
      //  {
      //    if (a.PropertyName == "State")
      //    {
      //      Console.WriteLine(p.CurrentCall.State);
      //      Console.WriteLine(p.CurrentCall.OtherEndNumber ?? "missing number");
      //    }
      //    if (a.PropertyName == "HasMissedCall")
      //    {
      //      Console.WriteLine(p.HasMissedCall);
      //    }
      //  };

      var p2 = manager.GetPresence("2000");


      p2.PropertyChanged += (o, a) =>
      {
          if (a.PropertyName == "State")
          {
              Console.WriteLine(p2.State);
          }
      };


      Console.ReadLine();
    }
  }
}
