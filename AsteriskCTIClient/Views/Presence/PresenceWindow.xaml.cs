using System.ComponentModel;
using System.Windows;
using AsteriskCTIClient.ViewModel.ViewModels;
using AsteriskCTIClient.Views.ViewUtilities;

namespace AsteriskCTIClient.Views.Presence
{
  /// <summary>
  ///   Interaction logic for PresenceWindow.xaml
  /// </summary>
  public partial class PresenceWindow : TransparentWindow
  {
    private readonly PhoneWindowVM _vM;

    public PresenceWindow(object dataContext)
    {
      DataContext = dataContext;
      InitializeComponent();
      _vM = (PhoneWindowVM) DataContext;
    }

    private void TransparentWindowLoaded(object sender, RoutedEventArgs roe)
    {
      searchBox.TextChanged += (s, e) => tabs.SelectedIndex = 1;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      _vM.SaveFavourites();
    }
  }
}