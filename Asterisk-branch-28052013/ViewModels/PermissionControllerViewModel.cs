using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseAccess;

namespace Asterisk.ViewModels
{
  public class PermissionControllerViewModel
  {
    public int SelectedDialplan { get; private set; }
    public IEnumerable<IDialplan> Dialplans { get; set; }
    private readonly IRepository _repository;

    public PermissionControllerViewModel(int dialplan, IRepository repository)
    {
      SelectedDialplan = dialplan;
      _repository = repository;
      Dialplans =
        _repository.GetList<IDialplan>().Where(d => !d.Name.StartsWith("uncondition") && !d.Name.StartsWith("onBusy")
                                                    && !d.Name.StartsWith("noAnwser") && !d.Name.Equals("ddiDefault")
                                                    && !d.Name.Equals("fallThrough"));
    }

    public string GetCurrentDialPlanName()
    {
      return _repository.GetFromId<IDialplan>(SelectedDialplan).Name;
    }
  }
}