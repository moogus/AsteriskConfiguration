using System;
using System.ComponentModel;
using System.Windows.Input;
using AsteriskCTIClient.Annotations;
using AsteriskCTIClient.Model.ModelInterfaces;
using AsteriskCTIClient.ViewModel.VMInterfaces;
using AsteriskCTIClient.ViewModel.VMUtilities;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class ContactVM : INotifyPropertyChanged, IContactVM
  {
    private readonly Action<ContactVM> _addContact;
    private readonly Action _dialNumber;
    private bool _isHidden;

    public ContactVM(Action dialNumber, Action<ContactVM> addContact)
    {
      _dialNumber = dialNumber;
      _addContact = addContact;
      StartOfGroup = false;
      DialExtensionCommand = new SimpleCommand(_dialNumber);
      FavouriteOperationCommand = new SimpleCommand(() => _addContact(this));
    }

    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public string Extension { get; set; }
    public string DDI { get; set; }
    public string Mobile { get; set; }
    public bool StartOfGroup { get; set; }
    public bool IsAsteriskContact { get; set; }
    public IPresenceItem Presence { get; set; }
    public ICommand DialExtensionCommand { get; set; }
    public ICommand FavouriteOperationCommand { get; private set; }

    public bool IsHidden
    {
      get { return _isHidden; }
      set
      {
        _isHidden = value;
        OnPropertyChanged("IsHidden");
      }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}