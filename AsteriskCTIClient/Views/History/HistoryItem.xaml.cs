using System.Windows;
using System.Windows.Controls;

namespace AsteriskCTIClient.Views.History
{
  /// <summary>
  ///   Interaction logic for HistoryItem.xaml
  /// </summary>
  public partial class HistoryItem : UserControl
  {
    public HistoryItem()
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