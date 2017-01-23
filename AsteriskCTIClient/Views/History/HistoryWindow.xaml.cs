using System.ComponentModel;
using AsteriskCTIClient.ViewModel.ViewModels;
using AsteriskCTIClient.Views.ViewUtilities;

namespace AsteriskCTIClient.Views.History
{
  /// <summary>
  ///   Interaction logic for HistoryWindow.xaml
  /// </summary>
  public partial class HistoryWindow : TransparentWindow
  {
    private readonly HistoryWindowVM _vM;

    public HistoryWindow(object dataContext)
    {
      DataContext = dataContext;
      InitializeComponent();
      _vM = (HistoryWindowVM) DataContext;
    }


    protected override void OnClosing(CancelEventArgs e)
    {
      //TODO Maybe save the call list???
    }
  }
}