using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Outlook;

namespace AsteriskCTIOutlook2010AddIn.Model
{
  public interface IPhoneContactMenu
  {
    string ContextMenuItemXml { get; }
    Dictionary<string, string> HeldContactNumbers { get; }
    string MakeDynamicMenu(Microsoft.Office.Core.IRibbonControl control);
  }

  public class AsteriskContactMenuModel : IPhoneContactMenu
  {
    public string ContextMenuItemXml { get; private set; }
    public Dictionary<string, string> HeldContactNumbers { get; private set; }

    public AsteriskContactMenuModel()
    {
      ContextMenuItemXml = CreateBaseContactDial();
    }

    private static string CreateBaseContactDial()
    {
      var sb = new StringBuilder();

      sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
      sb.Append("<customUI xmlns=\"http://schemas.microsoft.com/office/2009/07/customui\" onLoad=\"RibbonLoad\" >");
      sb.Append("<contextMenus>");
      sb.Append("<contextMenu idMso=\"ContextMenuContactItem\">");
      sb.Append("<dynamicMenu id=\"dynamicMenu\" imageMso=\"QuickSearchHasMobilePhone\"  label=\"4Com Dial\" getContent=\"MakeMenu\"/>");
      sb.Append("</contextMenu>");
      sb.Append("</contextMenus>");
      sb.Append("</customUI>");

      return sb.ToString();
    }

    public string MakeDynamicMenu(Microsoft.Office.Core.IRibbonControl control)
    {
      var currentContact = GetContactItem(control);

      var sb = new StringBuilder();
      sb.Append("<menu xmlns=\"http://schemas.microsoft.com/office/2009/07/customui\">");
      sb.Append(CreateMenuItems(currentContact));
      sb.Append("</menu> ");

      return sb.ToString();
    }

    private static ContactItem GetContactItem(Microsoft.Office.Core.IRibbonControl control)
    {
      ContactItem rtn = null;
      if (control.Context is Selection)
      {
        var selection = control.Context as Selection;
        if (selection.Count == 1)
        {
          rtn = selection[1] as ContactItem;
        }
      }
      return rtn;
    }

    private string CreateMenuItems(ContactItem currentContact)
    {
      var sb = new StringBuilder();
      var addSeporator = false;

      HeldContactNumbers = GetContactNumbers(currentContact);

      foreach (var numberType in HeldContactNumbers)
      {
        sb.Append(CreateMenuItem(numberType.Key, addSeporator));
        addSeporator = true;
      }
      return sb.ToString();
    }

    private static string CreateMenuItem(string description, bool addSeporator)
    {
      var sb = new StringBuilder();
      sb.Append(addSeporator ? "<menuSeparator  />" : "");
      sb.Append(String.Format("<button id=\"{0}\"  ", description));
      sb.Append(String.Format(" label=\"{0}\"", description));
      sb.Append(" enabled=\"true\" imageMso=\"QuickSearchPhoneMenu\" ");
      sb.Append(string.Format(" onAction=\"{0}\" ", description));
      sb.Append(" />");
      return sb.ToString();
    }

    private static Dictionary<string, string> GetContactNumbers(ContactItem currentContact)
    {
      var rtn = new Dictionary<string, string>();

      if (IsValidNumber(currentContact.BusinessTelephoneNumber))
        rtn.Add("Business", currentContact.BusinessTelephoneNumber);


      if (IsValidNumber(currentContact.HomeTelephoneNumber))
        rtn.Add("Home", currentContact.HomeTelephoneNumber);


      if (IsValidNumber(currentContact.MobileTelephoneNumber))
        rtn.Add("Mobile", currentContact.MobileTelephoneNumber);


      if (IsValidNumber(currentContact.CompanyMainTelephoneNumber))
        rtn.Add("Company", currentContact.CompanyMainTelephoneNumber);


      if (IsValidNumber(currentContact.AssistantTelephoneNumber))
        rtn.Add("Assistant", currentContact.AssistantTelephoneNumber);


      if (IsValidNumber(currentContact.Business2TelephoneNumber))
        rtn.Add("Business2", currentContact.Business2TelephoneNumber);


      if (IsValidNumber(currentContact.Home2TelephoneNumber))
        rtn.Add("Home2", currentContact.Home2TelephoneNumber);


      if (IsValidNumber(currentContact.OtherTelephoneNumber))
        rtn.Add("Other", currentContact.OtherTelephoneNumber);


      if (IsValidNumber(currentContact.PrimaryTelephoneNumber))
        rtn.Add("Primary", currentContact.PrimaryTelephoneNumber);

      return rtn;
    }

    private static bool IsValidNumber(string phoneNumber)
    {
      return !string.IsNullOrEmpty(phoneNumber);
    }

  }
}
