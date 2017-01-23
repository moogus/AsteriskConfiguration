using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using AsteriskCTIClient.Model.ModelInterfaces;
using AsteriskCTIClient.ViewModel.VMInterfaces;
using AsteriskCTIClient.ViewModel.VMUtilities;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class PhoneWindowVM : INotifyPropertyChanged
  {
    private readonly IAddressContactsModel _addressContactModel;
    private readonly IDialerModel _dialerModel;
    private string _searchString;
    private string _selectedGroup;

    public PhoneWindowVM(IDialerModel dialerModel, IAddressContactsModel addressContactsModel)
    {
      _dialerModel = dialerModel;
      _addressContactModel = addressContactsModel;

      Grouping = new List<string> {_addressContactModel.SelectByName, _addressContactModel.SelectByDepartment};
      SelectedGroup = _addressContactModel.SelectByName;

      CreateICommands();
    }

    public ObservableCollection<IContactVM> AllContacts
    {
      get { return _addressContactModel.ContactsResults; }
    }

    public ObservableCollection<IContactVM> FavouriteContacts
    {
      get { return _addressContactModel.FavouriteContacts; }
    }

    public string SearchString
    {
      get { return _searchString; }
      set
      {
        SearchFromValue(value);
        _searchString = value;
      }
    }

    public string SelectedGroup
    {
      get { return _selectedGroup; }
      set
      {
        _addressContactModel.SortContactsResultsByDepartment(value);
        _selectedGroup = value;
        OnPropertyChanged("SelectedGroup");
      }
    }

    public List<string> Grouping { get; set; }
    public string ConnectionStatus { get; set; }
    public ICommand DialNumberCommand { get; set; }
    public ICommand SearchCommand { get; set; }


    private void CreateICommands()
    {
      DialNumberCommand = new SimpleCommand(DialNumber);
      SearchCommand = new SimpleCommand(() => SearchFromValue(SearchString));
    }

    private void DialNumber()
    {
      if (!IsKnownNumber(SearchString) && !IsPosibleExternalNumber(SearchString)) return;

      var numberToDial = SearchString;
      _dialerModel.DialNumber(numberToDial);
    }

    private bool IsKnownNumber(string numberToDial)
    {
      return
        _addressContactModel.Contacts.Select(c => c.Extension)
                            .Concat(_addressContactModel.Contacts.Select(c => c.Mobile))
                            .Concat(
                              _addressContactModel.Contacts.Select(c => c.DDI)).Any(number => number == numberToDial);
    }

    private static bool IsPosibleExternalNumber(string numberToDial)
    {
      return numberToDial.All(Char.IsNumber) && numberToDial.Count() > 8;
    }

    public void SaveFavourites()
    {
      _addressContactModel.SaveFavourites();
    }

    private void SearchFromValue(string value)
    {
      foreach (IContactVM c in _addressContactModel.Contacts)
      {
        c.IsHidden = IsNotValidContact(c, value);
        if (c.Extension == "555")
        {
          c.IsHidden = value != "555";
        }
      }

      _addressContactModel.ContactsResults.Clear();

      List<IContactVM> l = _addressContactModel.Contacts.Where(c => !c.IsHidden).ToList();

      l.ForEach(_addressContactModel.ContactsResults.Add);
    }

    private static bool IsNotValidContact(IContactVM contact, string value)
    {
      return !contact.Extension.StartsWith(value) && !contact.DDI.StartsWith(value) &&
             !contact.Mobile.StartsWith(value) &&
             !contact.FirstName.ToLower().StartsWith(value.ToLower()) &&
             !contact.LastName.ToLower().StartsWith(value.ToLower()) &&
             !contact.Department.ToLower().StartsWith(value.ToLower());
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}