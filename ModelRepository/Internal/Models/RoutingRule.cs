using System;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class RoutingRule : IRoutingRule
  {
    private readonly IComRoutingRule _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public RoutingRule(IComRoutingRule routingRule, IRepositoryWithDelete modelRepository)
    {
      _under = routingRule;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public IDialplan Dialplan
    {
      get { return _modelRepository.GetFromId<IDialplan>(_under.DialplanId); }
      set { _under.DialplanId = value == null ? 0 : value.Id; }
    }

    public string Number
    {
      get { return _under.Number; }
      set { _under.Number = value; }
    }

    public int Time
    {
      get { return _under.Time; }
      set { _under.Time = value; }
    }

    public int Order
    {
      get { return _under.Order; }
      set { _under.Order = value; }
    }

    public string DestinationNumber
    {
      get { return _under.DestinationNumber; }
      set { _under.DestinationNumber = value; }
    }

    public RoutingRuleDestination DestinationType
    {
      get
      {
        if (string.IsNullOrEmpty(_under.DestinationType))
        {
          return RoutingRuleDestination.Error;
        }
        return (RoutingRuleDestination) Enum.Parse(typeof (RoutingRuleDestination), _under.DestinationType);
      }
      set { _under.DestinationType = value.ToString(); }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}