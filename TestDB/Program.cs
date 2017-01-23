using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess;
using DatabaseAccess.DatabaseTables;
using NHibernate.Linq;

namespace TestDB
{
  class Program
  {
    static void Main(string[] args)
    {
      var x = new DatabaseTester();
    var all =   x.SessionWrapper.Query<FuUserConfig>();
      foreach (var fuUserConfig in all)
      {
       Console.WriteLine(fuUserConfig.ExtensionNumber);
      }
      Console.ReadKey();
    }
  }
}
