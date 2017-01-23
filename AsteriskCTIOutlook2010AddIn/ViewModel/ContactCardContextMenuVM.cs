using System.Runtime.InteropServices;
using AsteriskCTIOutlook2010AddIn.Model;
using Microsoft.Office.Core;

namespace AsteriskCTIOutlook2010AddIn.ViewModel
{
  [ComVisible(true)]
  public class ContactCardContextMenuVM : IRibbonExtensibility
  {
    private readonly OutlookDialerModel _outlookDialer;
    private readonly IPhoneContactMenu _asteriskContactMenu;

    public ContactCardContextMenuVM(OutlookDialerModel outlookDialer, IPhoneContactMenu asteriskContactMenu)
    {
      _asteriskContactMenu = asteriskContactMenu;
      _outlookDialer = outlookDialer;
    }

    public string GetCustomUI(string ribbonId)
    {
      return _asteriskContactMenu.ContextMenuItemXml;
    }

    public string MakeMenu(Microsoft.Office.Core.IRibbonControl control)
    {
      return _asteriskContactMenu.MakeDynamicMenu(control);
    }

    public void Business(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Business"]);
    }

    public void Home(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Home"]);
    }

    public void Mobile(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Mobile"]);
    }

    public void Company(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Company"]);
    }

    public void Assistant(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Assistant"]);
    }

    public void Business2(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Business2"]);
    }

    public void Home2(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Home2"]);
    }

    public void Other(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Other"]);
    }

    public void Primary(Microsoft.Office.Core.IRibbonControl control)
    {
      _outlookDialer.Dial(_asteriskContactMenu.HeldContactNumbers["Primary"]);
    }
  }
}
