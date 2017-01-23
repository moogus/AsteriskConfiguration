using System.Collections.Generic;
using System.Linq;
using ModelRepository;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ViewModels
{
  public class RouteControllerViewModel
  {
    private readonly IRepository _modelRepository;
    public int SelectedDialplan;

    //ToDo: resolve dependancies.....Get rid of this massive viewmodel

    public RouteControllerViewModel(int dialplan, IRepository modelRepository)
    {
      SelectedDialplan = dialplan;
      _modelRepository = modelRepository;
      Dialplans =
        _modelRepository.GetList<IDialplan>().Where(d => !d.Name.StartsWith("uncondition") && !d.Name.StartsWith("onBusy")
                                                    && !d.Name.StartsWith("noAnwser") && !d.Name.Equals("ddiDefault")
                                                    && !d.Name.Equals("fallThrough"));
    }

    public IEnumerable<IDialplan> Dialplans { get; set; }

    public string GetCurrentDialPlanName()
    {
      return _modelRepository.GetFromId<IDialplan>(SelectedDialplan).Name;
    }
  }
}