using AsteriskCTIOutlook2010AddIn.Model;
using AsteriskCTIOutlook2010AddIn.ViewModel;
using Office = Microsoft.Office.Core;

namespace AsteriskCTIOutlook2010AddIn
{
    public partial class ThisAddIn
    {
        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
          var outlookDial = new OutlookDialerModel();
          var asteriskContextMenu = new AsteriskContactMenuModel();
          return new ContactCardContextMenuVM(outlookDial, asteriskContextMenu);
        }

        #region VSTO generated code

        private void InternalStartup()
        {
        }
        
        #endregion
    }
}
