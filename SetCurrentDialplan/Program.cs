using System;
using DialplanManager.DialplanClasses;
using DialplanManager.Interfaces;
using StructureMap;

namespace SetCurrentDialplan
{
  class Program
  {
    static void Main(string[] args)
    {
      var c = ConfigureDependencies().GetInstance<ICurrentDialplanManager>();
      c.SetCurrentDialplan();
      Console.WriteLine("Done");
    }

    private static IContainer ConfigureDependencies()
    {
      return new Container(x =>
        {
          x.For<ICurrentDialplanManager>().Use<CurrentDialplanManager>();
          x.For<DialplanFilters>().Use<DialplanFilters>();
        });
    }
  }
}
