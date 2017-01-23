using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using CTIServer;
using CTIServer.ExposedInterfaces;
using DatabaseAccess;

namespace CTIServerTest
{
  class Program
  {
    private static Stopwatch sw;
    static void Main(string[] args)
    {
      sw = new Stopwatch();
      sw.Start();

      Thread.Sleep(1000);

      Console.WriteLine("");
      Console.WriteLine("");

      var exts = new Repository().GetList<IExtension>().Select(e => new PhoneInformation(e.Number, e.FirstName + " " + e.LastName, e.Department)).ToList();

      Thread.Sleep(1000);

      Console.WriteLine("");
      Console.WriteLine("");

      Setup(exts);

      
      Console.ReadKey();
    }

    private static void Setup(IEnumerable<PhoneInformation> exts)
    {
      var manager = new AsteriskManager(exts, "10.10.20.188");

      var presence2001 = manager.GetPhone("2001");
      var presence2003 = manager.GetPhone("2003");
      
      presence2001.PropertyChanged += (sender, a) =>
        {
          if (a.PropertyName == "State")
          {
            Console.WriteLine("{1}: 2001 {0}", presence2001.State, sw.ElapsedMilliseconds);
          }
        };

      presence2003.PropertyChanged += (sender, a) =>
        {
          if (a.PropertyName == "State")
          {
            Console.WriteLine("{1}: 2003 {0}", presence2003.State, sw.ElapsedMilliseconds);
          }
        };
    }
  }
}
