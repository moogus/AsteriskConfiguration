using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using AsteriskCTIClient.Model.ModelInterfaces;
using AsteriskCTIClient.ViewModel.VMInterfaces;
using AsteriskCTIClient.ViewModel.ViewModels;

namespace AsteriskCTIClient.Model.Models
{
  public class ContactCollectionsModel : IAddressContactsModel, IFavouriteContactsManager
  {
    private readonly Action<string> _dialNumber;
    private readonly IPresenceManagerModel _presenceManager;
    private readonly IContactListModel _csvContactListModel;

    public ContactCollectionsModel(Action<string> dialNumber, IPresenceManagerModel presenceManager, IContactListModel csvContactListModel)
    {
      SelectByDepartment = "Group by Department";
      SelectByName = "Group by Name";
      _dialNumber = dialNumber;
      _presenceManager = presenceManager;
      _csvContactListModel = csvContactListModel;
      CreateAllContactsAndFavourites();
    }

    public string SelectByDepartment { get; private set; }
    public string SelectByName { get; private set; }
    public List<IContactVM> Contacts { get; set; }
    public ObservableCollection<IContactVM> ContactsResults { get; set; }
    public ObservableCollection<IContactVM> FavouriteContacts { get; set; }

    public void SortContactsResultsByDepartment(string selectedGroup)
    {
      ContactsResults.Clear();
      List<IContactVM> tempFavs = FavouriteContacts.ToList();
      FavouriteContacts.Clear();
      string olddept;

      if (selectedGroup.Equals(SelectByName))
      {
        olddept = string.Empty;
        tempFavs.OrderBy(t => t.UserName)
                .ToList()
                .ForEach(contact => { olddept = GroupCollection(FavouriteContacts, contact, olddept, false); });

        olddept = string.Empty;
        Contacts.ToList().ForEach(contact => { olddept = GroupCollection(ContactsResults, contact, olddept, false); });
      }

      if (!selectedGroup.Equals(SelectByDepartment)) return;
      olddept = string.Empty;
      tempFavs.OrderBy(t => t.Department)
              .ToList()
              .ForEach(contact => { olddept = GroupCollection(FavouriteContacts, contact, olddept, true); });

      olddept = string.Empty;
      Contacts.OrderBy(c => c.Department)
              .ToList()
              .ForEach(contact => { olddept = GroupCollection(ContactsResults, contact, olddept, true); });
    }

    public void SaveFavourites()
    {
      Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      config.AppSettings.Settings.Remove("Favorites");
      config.AppSettings.Settings.Add("Favorites", AggregateFavouritesToString());
      config.Save(ConfigurationSaveMode.Modified);
      ConfigurationManager.RefreshSection("appSettings");
    }

    public void AddContact(IContactVM contact)
    {
      if (FavouriteContacts.Contains(contact)) return;
      FavouriteContacts.Add(contact);
      SetPresenceOnContact(FavouriteContacts.First(c => c == contact));
    }

    public void RemoveContact(IContactVM contact)
    {
      FavouriteContacts.Remove(contact);
    }

    private void CreateAllContactsAndFavourites()
    {
      CreateContacts();
      ContactsResults = new ObservableCollection<IContactVM>(Contacts);
      SetPresenceOnCollection(ContactsResults);
      FavouriteContacts = new ObservableCollection<IContactVM>(GetFavourites());
      SetPresenceOnCollection(FavouriteContacts);
    }

    private void CreateContacts()
    {
      IEnumerable<ContactVM> allContacts =
        from c in _csvContactListModel.ContactModels
       
        select new ContactVM(() => _dialNumber(c.Extension), AddContact)
          {
            UserName = string.Format("{0} {1}", c.FirstName, c.LastName),
            FirstName = c.FirstName,
            LastName = c.LastName,
            Department = c.Department,
            Position =c.Position,
            Email = c.Email,
            Extension = c.Extension,
            DDI = c.DDI,
            Mobile = c.Mobile,
            IsAsteriskContact = c.IsAsteriskContact,
          };

      List<ContactVM> contactsWithVoiceMail = allContacts.ToList();
      contactsWithVoiceMail.Add(new ContactVM(() => _dialNumber("555"), AddContact)
        {
          FirstName = "",
          LastName = "",
          Department = "",
          Position = "",
          Email = "",
          Extension = "555",
          DDI = "",
          Mobile = "",
          IsAsteriskContact = false,
          UserName = "Your Voicemail",
          IsHidden = false
        });
      Contacts = new List<IContactVM>(contactsWithVoiceMail);
    }

    private IEnumerable<IContactVM> GetFavourites()
    {
      var favList = new List<IContactVM>();
      string favourites = ConfigurationManager.AppSettings.Get("Favorites");

      if (favourites.Length == 0) return favList;

      favList.AddRange(favourites.Split(',').Select(f => Contacts.First(c => c.Extension == f)));
      return favList;
    }

    private void SetPresenceOnCollection(IEnumerable<IContactVM> contactVms)
    {
      foreach (IContactVM currentContact in contactVms)
      {
        SetPresenceOnContact(currentContact);
      }
    }

    private void SetPresenceOnContact(IContactVM currentContact)
    {
      currentContact.Presence = currentContact.IsAsteriskContact
                                  ? _presenceManager.GetAsteriskPresenceItem(currentContact.Extension)
                                  : null;
    }

    private static string GroupCollection(ObservableCollection<IContactVM> collection, IContactVM contact,
                                          string olddept, bool isToGroup)
    {
      if (olddept != contact.Department)
        contact.StartOfGroup = isToGroup;
      collection.Add(contact);
      return contact.Department;
    }

    private string AggregateFavouritesToString()
    {
      return FavouriteContacts.Aggregate(string.Empty,
                                         (current, contact) =>
                                         current == string.Empty
                                           ? contact.Extension
                                           : string.Format("{0},{1}", current, contact.Extension));
    }
  }
}