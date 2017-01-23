using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using AsteriskCTIClient.ViewModel.ViewModels;
using Hardcodet.Wpf.TaskbarNotification;

namespace AsteriskCTIClient.Views.CtiBalloon
{
  /// <summary>
  /// Interaction logic for CtiBallon.xaml
  /// </summary>
  public partial class CtiBalloonPopup : UserControl
  {
    private readonly CtiBalloonPopupVM _dataContext;

    private bool _isClosing = false;

    public CtiBalloonPopup(object dataContext)
    {
      DataContext = dataContext;
      _dataContext = (CtiBalloonPopupVM)dataContext;
      InitializeComponent();
      TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
    }

    private void OnBalloonClosing(object sender, RoutedEventArgs e)
    {
      e.Handled = true;
      _isClosing = true;
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
      //TODO: if the call is internal should there be any functionality on the icon click?
      if(_dataContext.DoAction!=null)
      _dataContext.DoAction();
    }

    private void ImgCloseMouseDown(object sender, MouseButtonEventArgs e)
    {
      TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
      taskbarIcon.CloseBalloon();
    }

    private void GridMouseEnter(object sender, MouseEventArgs e)
    {
      if (_isClosing) return;

      TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
      taskbarIcon.ResetBalloonCloseTimer();
    }

    private void OnFadeOutCompleted(object sender, EventArgs e)
    {
      Popup pp = (Popup)Parent;
      pp.IsOpen = false;
    }

  }
}
