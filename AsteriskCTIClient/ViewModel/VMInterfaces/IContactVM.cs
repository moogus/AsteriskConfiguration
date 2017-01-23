using System.Windows.Input;
using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.ViewModel.VMInterfaces
{
  public interface IContactVM : IContactModel
  {
    IPresenceItem Presence { get; set; }
    ICommand DialExtensionCommand { get; set; }
    ICommand FavouriteOperationCommand { get; }
    bool IsHidden { get; set; }
    bool StartOfGroup { get; set; }
  }
}