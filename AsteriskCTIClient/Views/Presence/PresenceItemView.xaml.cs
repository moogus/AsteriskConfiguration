using System.Windows;
using System.Windows.Controls;

namespace AsteriskCTIClient.Views.Presence
{
  /// <summary>
  ///   Interaction logic for PresenceItemView.xaml
  /// </summary>
  public partial class PresenceItemView : UserControl
  {
    public PresenceItemView()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      userContextMenu.PlacementTarget = this;
      userContextMenu.IsOpen = true;
    }
  }
}