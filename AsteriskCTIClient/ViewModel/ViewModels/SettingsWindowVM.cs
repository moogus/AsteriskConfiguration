using System.ComponentModel;
using System.Configuration;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class SettingsWindowVM : INotifyPropertyChanged
  {
    public string ExtensionNumber
    {
      get { return ConfigurationManager.AppSettings.Get("Extension"); }
      set { SetApplicationSetting("Extension", value); }
    }

    public string ServerAddress
    {
      get { return ConfigurationManager.AppSettings.Get("Server"); }
      set { SetApplicationSetting("Server", value); }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    public void SetApplicationSetting(string setting, string value)
    {
      Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      config.AppSettings.Settings.Remove(setting);
      config.AppSettings.Settings.Add(setting, value);
      config.Save(ConfigurationSaveMode.Modified);
      ConfigurationManager.RefreshSection("appSettings");
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}