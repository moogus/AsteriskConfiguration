using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.Model.Models
{
  public class ClipboardCopyEventArgs
  {
    public string ClipboardContents { get; private set; }

    public ClipboardCopyEventArgs(string clipboardContents)
    {
      ClipboardContents = clipboardContents;
    }
  }

  public class ClipboardWatcher : IClipboardWatcherModel
  {
    private static IntPtr _nextClipboardViewer;
    private readonly Window _parent;
    private HwndSource _source;

    public delegate void ClipboardCopyEventHandler(object sender, ClipboardCopyEventArgs e);
    public event ClipboardCopyEventHandler ClipBoardCopyEvent;

    protected virtual void OnClipBoardCopyEvent(string clipBoardString)
    {
      ClipboardCopyEventHandler handler = ClipBoardCopyEvent;
      if (handler != null) handler(this, new ClipboardCopyEventArgs(clipBoardString));
    }

    public ClipboardWatcher(Window parent)
    {
      _parent = parent;

      parent.SourceInitialized += (s, e) => Initialise();
      parent.Closed += (s, e) => Close();
    }

    private IntPtr Handle
    {
      get { return new WindowInteropHelper(_parent).Handle; }
    }

    [DllImport("User32.dll")]
    private static extern int SetClipboardViewer(int hWndNewViewer);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    private void Initialise()
    {
      _nextClipboardViewer = (IntPtr)SetClipboardViewer((int)Handle);

      _source = PresentationSource.FromVisual(_parent) as HwndSource;
      if (_source != null)
        _source.AddHook(WndProc);
    }

    private static bool ClipboardSearch(IDataObject data)
    {
      return data.GetDataPresent(DataFormats.Text);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      const int WM_DRAWCLIPBOARD = 0x308;
      const int WM_CHANGECBCHAIN = 0x030D;
      switch (msg)
      {
        case WM_DRAWCLIPBOARD:
          var clipBoardData = Clipboard.GetDataObject();
         
          if (ClipboardSearch(clipBoardData))
          {
            var clipBoardString = string.Empty;
              if (clipBoardData != null) clipBoardString = (string)clipBoardData.GetData(DataFormats.Text);
              OnClipBoardCopyEvent(clipBoardString);
          }

          SendMessage(_nextClipboardViewer, msg, wParam, lParam);
          return IntPtr.Zero;

        case WM_CHANGECBCHAIN:
          if (wParam == _nextClipboardViewer)
          {
            _nextClipboardViewer = lParam;
          }
          return IntPtr.Zero;

        default:
          return IntPtr.Zero;
      }
    }

    private void Close()
    {
      ChangeClipboardChain(Handle, _nextClipboardViewer);
      if (null != _source)
        _source.RemoveHook(WndProc);
    }
  }
}
