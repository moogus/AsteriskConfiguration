using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;

namespace Asterisk.ViewModels
{
  public class RouteControllerViewModel
  {
    public int SelectedDialplan;
    public IEnumerable<IDialplan> Dialplans { get; set; }

    private readonly IRepository _repository;

    //ToDo: resolve dependancies.....Get rid of this massive viewmodel

    public RouteControllerViewModel(int dialplan, IRepository repository)
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