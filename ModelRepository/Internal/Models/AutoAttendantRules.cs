using System;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class AutoAttendantRules : IAutoAttendantRules
  {
    private readonly IFuAutoAttendantRules _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public AutoAttendantRules(IFuAutoAttendantRules fuAutoAttendantRules, IRepositoryWithDelete modelRepository)
    {
      _under = fuAutoAttendantRules;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string AaName
    {
      get { return _under.AaName; }
      set { _under.AaName = value; }
    }

    //used by the modelRepository for get by name
    public string Entry
    {
      get { return _under.Entry; }
      set { _under.Entry = value; }
    }

    public string DestinationNumber
    {
      get { return _under.Destination; }
      set { _under.Destination = value; }
    }

    public RoutingRuleDestination DestinationType
    {
      get { return (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), _under.DestinationType); }
      set { _under.DestinationType = value.ToString(); }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}