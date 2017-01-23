using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using AsteriskCTIClient.Enums;
using AsteriskCTIClient.ViewModel.ViewModels;
using AsteriskCTIClient.Views.CtiBalloon;
using Hardcodet.Wpf.TaskbarNotification;


namespace AsteriskCTIClient.Views
{
  public partial class NotifyIcon : Window
  {
    private bool _popupisVisable;
    private CtiBalloonPopup _ctiBallonPopup;
    public NotifyIcon(object dataContext)
    {
      DataContext = dataContext;
      InitializeComponent();
      var vM = (NotifyIconVM)DataContext;
      _popupisVisable = false;

      vM.PropertyChanged += (s, e) =>
        {
          if (e.PropertyName == "Message")
          {
            switch (vM.Message.MessageType)
            {
              case MessageTypeEnum.DialExternalRing:
              case MessageTypeEnum.DialInternalRing:
                if (!_popupisVisable)
                {
                  _popupisVisable = true;

                  Application.Current.Dispatcher.Invoke((Action)(() => CreateCtiBalloon(vM.CtiBalloonVm)));
                  tbIcon.ShowCustomBalloon(_ctiBallonPopup, PopupAnimation.Slide, 100000);
                }
                break;

              case MessageTypeEnum.DialExternalHangup:
              case MessageTypeEnum.DialInternalHangup:
                Thread.Sleep(500);
                tbIcon.CloseBalloon();
                _popupisVisable = false;
                break;

              case MessageTypeEnum.IncommingRing:
                if (!_popupisVisable)
                {
                  _popupisVisable = true;
                  Application.Current.Dispatcher.Invoke((Action)(() => CreateCtiBalloon(vM.CtiBalloonVm)));
                  tbIcon.ShowCustomBalloon(_ctiBallonPopup, PopupAnimation.Slide, 100000);
                }
                break;

              case MessageTypeEnum.IncomingHangup:
                Thread.Sleep(500);
                tbIcon.CloseBalloon();
                _popupisVisable = false;
                break;

              case MessageTypeEnum.NumberToDial:
                tbIcon.TrayBalloonTipClicked += (sender, args) => vM.Message.ActionAgainstMessage();
                tbIcon.ShowBalloonTip(vM.CtiBalloonVm.BalloonTitle, vM.CtiBalloonVm.BalloonText, BalloonIcon.Info);
                break;

            }
          }
        };
    }

    private void CreateCtiBalloon(CtiBalloonPopupVM ctiBalloonVm)
    {
      _ctiBallonPopup = new CtiBalloonPopup(ctiBalloonVm);
    }


  }
}