using System;
using AsteriskCTIClient.Enums;
using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.ViewModel.VMUtilities
{
  class CopyWatcher
  {
    private readonly IMessageChanged _displayMessage;
    private readonly IDialerModel _dialerModel;

    public CopyWatcher(IClipboardWatcherModel clipboardWatcher, IMessageChanged displayMessage, IDialerModel dialerModel)
    {
      _displayMessage = displayMessage;
      _dialerModel = dialerModel;
      clipboardWatcher.ClipBoardCopyEvent += (sender, args) => ParseClipboardContents(args.ClipboardContents);
    }

    private void ParseClipboardContents(string clipboardContents)
    {
        Action action = () => _dialerModel.DialNumber(clipboardContents);
        _displayMessage.NotifyChanged(MessageTypeEnum.NumberToDial, "", clipboardContents, action);
    }
  }
}
