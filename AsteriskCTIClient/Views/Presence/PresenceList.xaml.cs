using System.Windows;
using System.Windows.Controls;

namespace AsteriskCTIClient.Views.Presence
{
  /// <summary>
  ///   Interaction logic for PresenceList.xaml
  /// </summary>
  public partial class PresenceList : UserControl
  {
    public static readonly DependencyProperty EmptyMessageProperty =
      DependencyProperty.Register("EmptyMessage", typeof (string), typeof (PresenceList));


    public PresenceList()
    {
      InitializeComponent();
    }

    public string EmptyMessage
    {
      get { return (string) GetValue(EmptyMessageProperty); }
      set { SetValue(EmptyMessageProperty, value); }
    }
  }
}