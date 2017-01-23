using System;
using System.ComponentModel;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Call;
using CTIServer.Phone;

namespace AsteriskCTIClient.Model.Models
{
  public class CtiCallModel : ICtiCall
  {
    private readonly ICall _call;
    private readonly ICallerIdModel _getCallerIdModel;


    public CtiCallModel(ICall call, ICallerIdModel getCallerIdModel)
    {
      _call = call;
      _getCallerIdModel = getCallerIdModel;
      _getCallerIdModel.SetNumberAndName(this);
      _call.PropertyChanged += (s, e) => { if (PropertyChanged != null) PropertyChanged(this, e); };
    }

    public event PropertyChangedEventHandler PropertyChanged;


    public string Id
    {
      get { return _call.Id; }
    }

    public string OtherEndNumber
    {
      get { return _call.OtherEndNumber; }
    }

    public string Channel
    {
      get { return _call.Channel; }
    }

    public bool Incoming
    {
      get { return _call.Incoming; }
    }

    public bool IsMissed
    {
      get { return _call.IsMissed; }
    }

    public DateTime StartTime
    {
      get { return _call.StartTime; }
    }

    public DateTime EndTime
    {
      get { return _call.EndTime; }
    }

    public CallStateEnum State
    {
      get { return _call.State; }
    }

    public void Pickup(IPhone phone)
    {
      _call.Pickup(phone);
    }

    public string CallerNumber
    {
      get { return _getCallerIdModel.CallerNumber; }
    }

    public string CallerName
    {
      get { return _getCallerIdModel.CallerName; }
    }

    public string Department
    {
      get { return _getCallerIdModel.Department; }
    }

    public bool IsvalidAsteriskExtension(string number)
    {
      return _getCallerIdModel.IsvalidAsteriskExtension(number);
    }
  }
}