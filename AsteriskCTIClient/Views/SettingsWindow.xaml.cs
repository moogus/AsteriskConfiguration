using System.ComponentModel;
using System.Windows;

namespace AsteriskCTIClient.Views
{
  /// <summary>
  ///   Interaction logic for SettingsWindow.xaml
  /// </summary>
  public partial class SettingsWindow : Window
  {
    public SettingsWindow()
    {
      InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      e.Cancel = true;
      Visibility = Visibility.Hidden;
      base.OnClosing(e);
    }
  }
}