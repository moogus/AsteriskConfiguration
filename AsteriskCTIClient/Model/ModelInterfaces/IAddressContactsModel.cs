using System.Collections.Generic;
using System.Collections.ObjectModel;
using AsteriskCTIClient.ViewModel.VMInterfaces;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IAddressContactsModel
  {
    List<IContactVM> Contacts { get; set; }
    ObservableCollection<IContactVM> ContactsResults { get; set; }
    ObservableCollection<IContactVM> FavouriteContacts { get; set; }
    string SelectByDepartment { get; }
    string SelectByName { get; }
    void SortContactsResultsByDepartment(string selectedGroup);
    void SaveFavourites();
  }
}