using System.Windows.Input;

namespace AsteriskCTIClient.ViewModel.VMInterfaces
{
  public interface IHistoryContactVM
  {
    bool Missed { get; set; }
    string TimeOfCall { get; }
    string UserName { get; }
    string Department { get; }
    string Number { get; }
    string Id { get; }
    ICommand DialExtensionCommand { get; set; }
  }
}