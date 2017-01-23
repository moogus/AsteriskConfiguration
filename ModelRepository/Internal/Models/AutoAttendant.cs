using System.Collections.Generic;
using System.Linq;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class AutoAttendant : IAutoAttendant
  {
    private readonly IFuAutoAttendant _under;
    private readonly IRepositoryWithDelete _modelRepository;
    private List<IAutoAttendantRules> _rules;

    public AutoAttendant(IFuAutoAttendant fuAutoAttendant, IRepositoryWithDelete modelRepository)
    {
      _under = fuAutoAttendant;
      _modelRepository = modelRepository;
      SetRulesLazy();
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
    public string Name
    {
      get { return _under.FuAutoAttendantName; }
      set { _under.FuAutoAttendantName = value; }
    }

    public string Announcement
    {
      get { return _under.Announcement; }
      set { _under.Announcement = value; }
    }

    public int Timeout
    {
      get { return _under.Timeout; }
      set { _under.Timeout = value; }
    }

    public IEnumerable<IAutoAttendantRules> Rules
    {
      get { return _rules; }
    }

    private void SetRulesLazy()
    {
      var rule = _modelRepository.GetList<IAutoAttendantRules>().Where(r => r.AaName == _under.Name).ToList();
      _rules = rule.Count == 0 ? new List<IAutoAttendantRules>() : rule;
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}