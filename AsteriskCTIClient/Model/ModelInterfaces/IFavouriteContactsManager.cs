using AsteriskCTIClient.ViewModel.VMInterfaces;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IFavouriteContactsManager
  {
    void AddContact(IContactVM contact);
    void RemoveContact(IContactVM contact);
  }
}