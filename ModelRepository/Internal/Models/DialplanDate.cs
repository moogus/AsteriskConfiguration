using System;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class DialplanDate : IDialplanDate
  {
    private readonly IFuDialplanDate _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public DialplanDate(IFuDialplanDate fuDpdate, IRepositoryWithDelete modelRepository)
    {
      _under = fuDpdate;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    //used by the modelRepository for get by name
    public DateTime StartDate
    {
      get { return _under.StartDate; }
      set { _under.StartDate = value; }
    }

    public DateTime EndDate
    {
      get { return _under.EndDate; }
      set { _under.EndDate = value; }
    }

    public IDialplan Dialplan
    {
      get
      {
        var dialplan = _modelRepository.GetFromId<IDialplan>(_under.FuDialplanId);
        return dialplan;
      }
      set
      {
        var dialplan = value;
        _under.FuDialplanId = dialplan == null ? 0 : dialplan.Id;
      }
    }
    
    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}