using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AsteriskCTIClient.Annotations;
using AsteriskCTIClient.Model.ModelInterfaces;
using AsteriskCTIClient.ViewModel.VMInterfaces;
using CTIServer.Call;

namespace AsteriskCTIClient.ViewModel.ViewModels
{
  public class HistoryWindowVM : INotifyPropertyChanged
  {
    private readonly ICallerIdModel _callerIdFromDB;
    private readonly IDialerModel _dialerModel;

    public HistoryWindowVM(IDialerModel dialerModel, ICallerIdModel callerIdFromDB)
    {
      AllCalls = new ObservableCollection<IHistoryContactVM>();
      _dialerModel = dialerModel;
      _callerIdFromDB = callerIdFromDB;
    }

    public ObservableCollection<IHistoryContactVM> AllCalls { get; set; }

    public IEnumerable<IHistoryContactVM> MissedCalls
    {
      get { return AllCalls.Where(c => c.Missed); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void AddCall(ICall call)
    {
      call.PropertyChanged += (sender, args) =>
        {
          if (args.PropertyName == "DialStatus")
          {
            OrganiseCalls(call);
          }
        };

      Application.Current.Dispatcher.Invoke(
        (Action) (() => AllCalls.Add(new HistoryContactVM(call, _dialerModel.DialNumber, _callerIdFromDB))));
    }

    private void OrganiseCalls(ICall call)
    {
      IHistoryContactVM historyContactVM = AllCalls.FirstOrDefault(c => c.Id == call.Id);
      if (historyContactVM == null) return;

      historyContactVM.Missed = call.IsMissed;

      if (historyContactVM.Missed)
        OnPropertyChanged("MissedCalls");
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}