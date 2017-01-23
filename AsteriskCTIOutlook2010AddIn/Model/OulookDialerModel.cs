using System.Diagnostics;
using System.Net;

namespace AsteriskCTIOutlook2010AddIn.Model
{
  public class OutlookDialerModel
  {
    public void Dial(string number)
    {
     Process.Start(string.Format( @"dial:{0}", number));
    }
  }
}
