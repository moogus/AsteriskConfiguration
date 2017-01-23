using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace AsteriskCTIClient.Views.ViewUtilities
{
  [StructLayout(LayoutKind.Sequential)]
  public class MARGINS
  {
    public int cxLeftWidth,
               cxRightWidth,
               cyTopHeight,
               cyBottomHeight;
  }

  internal class NonClientRegionAPI
  {
    public enum WINDOWTHEMEATTRIBUTETYPE : uint
    {
      /// <summary>Non-client area window attributes will be set.</summary>
      WTA_NONCLIENT = 1,
    }

    [Flags]
    public enum WTNCA : uint
    {
      NODRAWCAPTION = 1,
      NODRAWICON = 2,
      NOSYSMENU = 4,
      NOMIRRORHELP = 8,
      VALIDBITS = NODRAWCAPTION | NODRAWICON | NOSYSMENU | NOMIRRORHELP
    }

    [DllImport("DwmApi.dll")]
    public static extern void DwmIsCompositionEnabled(ref bool pfEnabled);

    [DllImport("uxtheme.dll")]
    public static extern int SetWindowThemeAttribute(
      IntPtr hWnd,
      WINDOWTHEMEATTRIBUTETYPE wtype,
      ref WTA_OPTIONS attributes,
      uint size);

    [StructLayout(LayoutKind.Sequential)]
    public struct WTA_OPTIONS
    {
      public WTNCA dwFlags;
      public WTNCA dwMask;
    }
  }

  public class TransparentWindow : Window
  {
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_DLGMODALFRAME = 0x0001;
    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOZORDER = 0x0004;
    private const int SWP_FRAMECHANGED = 0x0020;
    private const uint WM_SETICON = 0x0080;

    public TransparentWindow()
    {
      Loaded += Window_Loaded;
      MouseDown += Window_MouseDown;
    }

    [DllImport("dwmapi.dll")]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern bool DwmIsCompositionEnabled();

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height,
                                            uint flags);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    protected override void OnSourceInitialized(EventArgs e)
    {
      //SetWindowThemeAttribute(this, true, false);
      IntPtr hwnd = new WindowInteropHelper(this).Handle;

      // Change the extended window style to not show a window icon
      int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
      SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

      // Update the window's non-client area to reflect the changes
      SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    private void SetWindowThemeAttribute(Window window, bool showCaption, bool showIcon)
    {
      bool isGlassEnabled = DwmIsCompositionEnabled();

      IntPtr hwnd = new WindowInteropHelper(window).Handle;

      var options = new NonClientRegionAPI.WTA_OPTIONS
        {
          dwMask = (NonClientRegionAPI.WTNCA.NODRAWCAPTION | NonClientRegionAPI.WTNCA.NODRAWICON)
        };
      if (isGlassEnabled)
      {
        if (!showCaption)
        {
          options.dwFlags |= NonClientRegionAPI.WTNCA.NODRAWCAPTION;
        }
        if (!showIcon)
        {
          options.dwFlags |= NonClientRegionAPI.WTNCA.NODRAWICON;
        }
      }

      NonClientRegionAPI.SetWindowThemeAttribute(hwnd, NonClientRegionAPI.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
                                                 ref options,
                                                 (uint) Marshal.SizeOf(typeof (NonClientRegionAPI.WTA_OPTIONS)));
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (Environment.OSVersion.Version.Major >= 6 && DwmIsCompositionEnabled())
      {
        // Get the current window handle
        IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
        HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
        mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;

        Background = Brushes.Transparent;

        // Set the proper margins for the extended glass part
        var margins = new MARGINS();
        margins.cxLeftWidth = -1;
        margins.cxRightWidth = -1;
        margins.cyTopHeight = 50;
        margins.cyBottomHeight = 70;

        int result = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);

        if (result < 0)
        {
          MessageBox.Show("An error occured while extending the glass unit.");
        }
      }
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == MouseButton.Left)
        DragMove();
    }
  }
}