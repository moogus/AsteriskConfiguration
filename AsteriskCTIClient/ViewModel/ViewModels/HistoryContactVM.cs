using System;
using System.ComponentModel;
using System.Windows.Input;
using AsteriskCTIClient.Annotations;
using AsteriskCTIClient.Model.ModelInterfaces;
using AsteriskCTIClient.ViewModel.VMInterfaces;
using AsteriskCTIClient.ViewModel.VMUtilities;
using CTIServer.Call;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class HistoryContactVM : INotifyPropertyChanged, IHistoryContactVM
  {
    private readonly ICallerIdModel _callerIdFromDatabaseService;
    private readonly Action<string> _dialNumber;

    public HistoryContactVM(ICall call, Action<string> dialNumber, ICallerIdModel callerIdFromDB)
    {
      _callerIdFromDatabaseService = callerIdFromDB;
      _dialNumber = dialNumber;
      DialExtensionCommand =
        new SimpleCommand(() => _dialNumber(Number.Trim().Length > 5 ? string.Format("9{0}", Number) : Number));
      SetValues(call);
    }

    public bool Missed { get; set; }
    public string TimeOfCall { get; private set; }
    public string UserName { get; private set; }
    public string Department { get; private set; }
    public string Number { get; private set; }
    public string Id { get; private set; }
    public ICommand DialExtensionCommand { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    private void SetValues(ICall call)
    {
      if (call.Incoming)
        _callerIdFromDatabaseService.SetNumberAndName(call);

      if (!call.Incoming && _callerIdFromDatabaseService.IsvalidAsteriskExtension(call.OtherEndNumber))
        _callerIdFromDatabaseService.SetNumberAndName(call);

      Id = call.Id;
      UserName = _callerIdFromDatabaseService.CallerName;
      Number = _callerIdFromDatabaseService.CallerNumber;
      Department = _callerIdFromDatabaseService.Department;
      SetCallTime(call.StartTime);
    }

    private void SetCallTime(DateTime timeOfCall)
    {
      TimeOfCall = timeOfCall.Date == DateTime.Now.Date ? timeOfCall.ToShortTimeString() : timeOfCall.ToLongDateString();
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}