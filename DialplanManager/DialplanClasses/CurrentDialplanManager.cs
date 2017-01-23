using System.Linq;
using DatabaseAccess;
using DialplanManager.Interfaces;

namespace DialplanManager.DialplanClasses
{
  public class CurrentDialplanManager : ICurrentDialplanManager
  {
    private readonly Repository _repository;
     private readonly ICurrentDialPlan _currentDialplan;
    private readonly DialplanFilters _dialplanfilters;

    public CurrentDialplanManager()
    {
      _repository = new Repository();
      _currentDialplan = _repository.GetList<ICurrentDialPlan>().First();
      _dialplanfilters = new DialplanFilters();
    }

    public void SetCurrentDialplan()
    {
      if (_currentDialplan.AutomaticallyChange)
      {
        DoSetCurrentDialplan();
      }
    }

    private void DoSetCurrentDialplan()
    {
      var dialplan =  _dialplanfilters.FilterBasedOnDate(_repository.GetList<IDialplanDate>())??
                      _dialplanfilters.FilterBasedOnRange(_repository.GetList<IDialplanRange>());

      if (dialplan != null) { SetCurrentDialPlanTo(dialplan); }

    }

    private void SetCurrentDialPlanTo(IDialplan dialplan)
    {
      _currentDialplan.Dialplan = dialplan;
      _currentDialplan.Update();
    }
  }
}
