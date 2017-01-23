using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using AsteriskCTIClient.ApplicationResource.Singleton;
using AsteriskCTIClient.GetComapnyDetails;
using AsteriskCTIClient.Model.Models;
using AsteriskCTIClient.ViewModel.VMUtilities;
using AsteriskCTIClient.ViewModel.ViewModels;
using AsteriskCTIClient.Views;
using AsteriskCTIClient.Views.History;
using AsteriskCTIClient.Views.Presence;
using CTIServer.Call;
using CTIServer.CallHandler.CallHandlers;
using CTIServer.ConnectionManger.ConnectionManagers;
using CTIServer.Phone;
using CTIServer.Phone.Phones;
using CTIServer.PhoneManager.PhoneManagers;
using DatabaseAccess;

namespace AsteriskCTIClient
{
  /// <summary>
  ///   Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application, IHasStartUpEvent
  {
    private NotifyIconVM _notifyIconVM;
    private PresenceWindow _phoneWindow;
    private HistoryWindow _historyWindow;
    private readonly SingleInstanceManager _singleApplication;

    private Action<string> _doDial;

    private readonly string _appInstanceId;
    public App()
    {
      _appInstanceId = "ThisIsUniquetoThisApp";
      _singleApplication = new SingleInstanceManager(this,_appInstanceId );
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      if (_singleApplication.FirstInstance)
      {
        //set up server to listen for new args from other instances
        new AppArgumentServer(_appInstanceId, n=>_doDial(n));

        FirstInstanceOnStartup();

        if (e.Args.Length > 0)
          _doDial(e.Args[0]);
      }
      else
      {
        //send args to first instance
        if (e.Args.Length > 0)
           new AppArgumentClient(_appInstanceId).Send(e.Args[0]);
          
        // exit
        Application.Current.Shutdown();
      }
    }

    private void FirstInstanceOnStartup()
    {
      //Create models resources
      var repository = new Repository();

      //create CTIServer dependancy
      var exts =
        repository.GetList<IExtension>()
                  .Select(
                    ext => new AsteriskPhone(ext.Number, ext.DDINumber, ext.FirstName + " " + ext.LastName, ext.Department))
                  .ToList();
      var managerConnection = new AsteriskConnectionManager(ConfigurationManager.AppSettings.Get("Server"), 5038, "admin",
                                                            "31994");
      var callHandler = new AsteriskCallHandler(managerConnection);
      var manager = new AsteriskPhoneManager(exts, managerConnection, callHandler);

      //create the required models
      var csvListContactsModel = new CSVListContactsModel();
      var asteriskAccessListModel = new AsteriskAccessListModel(repository);
      var validatePhoneNumberModel = new ValidPhoneNumberModel(asteriskAccessListModel, csvListContactsModel);
      var callerIdManagerModel = new CallerIdManagerModel(repository,
                                                          new ChannelFactory<ICompanyDetail>("WSHttpBinding_ICompanyDetail"));
      
      //TODO: this dialer needs to be changed to accomodate the new AutoDialer context....the SNomDialer will not work with Gigasets
      //var dialerModel = new SnomDialerModel(repository, validatePhoneNumberModel);

      var dialerModel = new AsteriskDialerModel(managerConnection.Dialer, validatePhoneNumberModel);

      var presenceManagerModel = new PresenceManagerModel(manager, repository);
      var contactCollectionsModel = new ContactCollectionsModel(dialerModel.DialNumber, presenceManagerModel,
                                                                csvListContactsModel);
      var presentValidNumberModel = new NumberPresentationModel(asteriskAccessListModel);

      //create VM's
      var settingsWindowVM = new SettingsWindowVM();
      var phoneWindowVM = new PhoneWindowVM(dialerModel, contactCollectionsModel);
      var historyVM = new HistoryWindowVM(dialerModel, new AsteriskCallerIdModel(repository)); //todo remove dependancy
      var ctiBalloonPopupVM = new CtiBalloonPopupVM();

      //register phone
      IPhone myPhone = manager.GetPhone(ConfigurationManager.AppSettings.Get("Extension"));

      //create windows...
      SettingsWindow settingsWindow = null;
      var showSettingsWindowCommand =
        new SimpleCommand(() => settingsWindow = ShowSettingsWindow(settingsWindow, settingsWindowVM));

      _phoneWindow = null;
      var showPhoneWindowCommand = new SimpleCommand(() => ShowPhoneWindow(phoneWindowVM));

      _historyWindow = null;
      var showHistoryWindowCommand = new SimpleCommand(() => ShowHistoryWindow(historyVM));


      //Create NotifyIcon
      _notifyIconVM = new NotifyIconVM(manager, showSettingsWindowCommand, showPhoneWindowCommand,
                                       showHistoryWindowCommand, presentValidNumberModel, ctiBalloonPopupVM);

      var notiyIcon = new NotifyIcon(_notifyIconVM);

      myPhone.Calls.CollectionChanged += (sender, args) =>
        {
          if (args.Action == NotifyCollectionChangedAction.Add)
            foreach (ICall call in args.NewItems)
            {
              var ctiCall = new CtiCallModel(call, callerIdManagerModel.GetCallerIdModel(call));
              historyVM.AddCall(ctiCall);

              //TODO:this is currently spoofed sort it out son!!!--should pass in action to allow db look up on contact details
              var openCrmForcaller = new Action(_notifyIconVM.OpenCrmForCompanyNumber);

              new AsteriskCallManagerModel(ctiCall, _notifyIconVM, openCrmForcaller, presentValidNumberModel);
            }
        };

      //create clipboard watcher 
      var clipBoardWatcher = new ClipboardWatcher(notiyIcon);
      var copyWatcher = new CopyWatcher(clipBoardWatcher, _notifyIconVM, dialerModel);

      _doDial = number =>
        {
          // this should be passed in
          var thread = new Thread(() => dialerModel.DialNumber(number));
          thread.Start();
        };

      notiyIcon.Show();
      _notifyIconVM.ShowStartUpConnectedState();
    }

    public SettingsWindow ShowSettingsWindow(SettingsWindow settingsWindow,SettingsWindowVM settingsWindowVM)
    {
      if (settingsWindow == null || settingsWindow.IsLoaded == false)
      {
        settingsWindow = new SettingsWindow { DataContext = settingsWindowVM };
      }

      if (settingsWindow.WindowState == WindowState.Minimized || !settingsWindow.IsActive)
      {
        settingsWindow.Show();
      }

      if (string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("Extension")) ||
          string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("Server")))
      {
        settingsWindow.Show();
      }
      return settingsWindow;
    }

    public void ShowPhoneWindow(PhoneWindowVM phoneWindowVM)
    {
      if (_phoneWindow == null)
        _phoneWindow = new PresenceWindow(phoneWindowVM);
      _phoneWindow.Show();
    }

    private void ShowHistoryWindow(HistoryWindowVM historyVM)
    {
      if (_historyWindow == null)
        _historyWindow = new HistoryWindow(historyVM);
      _historyWindow.Show();
    }

    protected override void OnExit(ExitEventArgs exitEventArgs)
    {
      base.OnExit(exitEventArgs);
    }

  }

  public interface IHasStartUpEvent
  {
    event StartupEventHandler Startup;
  }

}