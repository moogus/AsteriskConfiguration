using System.Collections.Generic;
using System.Linq;
using AsteriskCTIClient.Model.ModelInterfaces;
using CTIServer.Call;
using DatabaseAccess;

namespace AsteriskCTIClient.Model.Models
{
  public class AsteriskCallerIdModel : ICallerIdModel
  {
    private readonly IRepository _repository;

    public AsteriskCallerIdModel(IRepository repository)
    {
      _repository = repository;
    }

    public string CallerNumber { get; private set; }
    public string CallerName { get; private set; }
    public string Department { get; private set; }

    //TODO this needs to come out into CallerIdManager...I think
    public bool IsvalidAsteriskExtension(string number)
    {
      bool rtn = false;
      foreach (string e in _repository.GetList<IExtension>().Select(e => e.Number).Where(e => e == number))
      {
        rtn = true;
      }

      if (rtn) return true;

      if (number == "555" || number == "*77" || number == "*78" || number == "*79")
      {
        CallerName = "Invalid";
        CallerNumber = "Invalid";
      }
      else
      {
        rtn = true;
      }

      return rtn;
    }

    public void SetNumberAndName(ICall call)
    {
      bool isExternal = ExternalDial(call.OtherEndNumber);
      if (!call.Incoming && isExternal)
      {
        CallerNumber = call.OtherEndNumber;
        CallerName = string.Empty;
        Department = string.Empty;
      }

      if (isExternal) return;

      SetNameAndNumber(_repository.GetFromName<IExtension>(call.OtherEndNumber));

      //todo: DON'T REPEAT YOURSELF
      if (call.Incoming)
      {
        SetIncomingValues(call.OtherEndNumber);
      }
      else
      {
        SetOutGoingValues(call.OtherEndNumber);
      }
    }

    //todo: DON'T REPEAT YOURSELF
    private void SetIncomingValues(string callersNumber)
    {
      SetNameAndNumber(_repository.GetFromName<IExtension>(callersNumber));
    }

    private void SetOutGoingValues(string dialedNumber)
    {
      SetNameAndNumber(_repository.GetFromName<IExtension>(dialedNumber));
    }

    private void SetNameAndNumber(IExtension extension)
    {
      if (extension == null) return;
      CallerNumber = extension.Number;
      CallerName = string.Format("{0} {1}", extension.FirstName, extension.LastName);
      Department = extension.Department;
    }

    private bool ExternalDial(string number)
    {
      IEnumerable<string> accesCodes =
        _repository.GetList<ITrunk>().SelectMany(t => t.AccessCodes).Select(c => c.AccessCode);
      bool rtn = false;
      foreach (string c in accesCodes.Where(number.StartsWith))
      {
        rtn = true;
      }
      return rtn;
    }
  }
}