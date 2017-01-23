using System;
using System.Collections.Generic;

using System.Text;

namespace AutoDialTest
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine(CallForAudio());
      Console.ReadKey();
    }
    public static string CallForAudio()
    {
      var makeCall = new Connector("10.10.20.188");
      makeCall.Connect();
      var rtn = makeCall.CallAsterisk("*33", "", "", "LocalSets") ? "calling"  : "";
      makeCall.Disconnect();

      return rtn;
    }
  }
}
