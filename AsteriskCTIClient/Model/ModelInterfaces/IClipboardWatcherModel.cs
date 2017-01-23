using AsteriskCTIClient.Model.Models;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IClipboardWatcherModel
  {
    event ClipboardWatcher.ClipboardCopyEventHandler ClipBoardCopyEvent;
  }
}