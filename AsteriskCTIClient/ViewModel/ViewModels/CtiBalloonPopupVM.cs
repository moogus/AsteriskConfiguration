using System;
using System.ComponentModel;
using System.Windows.Input;
using AsteriskCTIClient.Annotations;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class CtiBalloonPopupVM :INotifyPropertyChanged
  {
    private string _balloonText;
    private string _balloonTitle;

    public Action DoAction { get; set; }

    public string BalloonText
    {
      get { return _balloonText; }
      set { _balloonText = value;OnPropertyChanged("BalloonText"); }
    }

    public string BalloonTitle
    {
      get { return _balloonTitle; }
      set { _balloonTitle = value;OnPropertyChanged("BalloonTitle"); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
