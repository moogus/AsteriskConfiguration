using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.ModelUtilities.TrunkManager
{
  internal class TrunkManager : ITrunkManager
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;

    internal TrunkManager(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
    }

    public void RemoveAccessCodes(int  trunkId)
    {
      var currentCodes = _session.Query<ComAccessCode>().ToList().Where(d => d.TrunkId ==trunkId ).ToList();
      foreach (var oldCode in currentCodes)
      {
        _session.Delete(oldCode);
      }
    }

    public bool CreateDefaults(string defaultDestination, int trunkId)
    {
      var allDDIs = _repository.GetList<IDDI>().Where(d => d.UsedOn == DDIUsedOn.NotUsed || d.UsedOn == DDIUsedOn.Default && d.Trunk.Id == trunkId).ToList();
      return  allDDIs.Select(d => SetDefaults(d, defaultDestination, GetDestination(defaultDestination))).Any(b => b);
    }

    public void UpdateAccessCodes(List<AccessCodeAndPriority> accessCodes, int trunkId)
    {
      foreach (var accessCode in accessCodes.Select(code => new ComAccessCode { Code = code.AccessCode, Priority = code.Priority, TrunkId = trunkId }))
      {
        _session.SaveOrUpdate(accessCode);
      }
    }

    public void ResetDdiUsedValue( int trunkId)
    {
      var ddis = _repository.GetList<IDDI>().Where(d => d.Trunk.Id == trunkId);
      foreach (var ddi in ddis)
      {
        ddi.Trunk = _repository.Add<ITrunk>();
        ddi.UsedOn = DDIUsedOn.NotUsed;
        ddi.Update();
      }
    }

    public List<AccessCodeAndPriority> GetAccessCodes(List<ComAccessCode> underAccessCodes)
    {
      return underAccessCodes.Select(ac => new AccessCodeAndPriority { AccessCode = ac.Code, Priority = ac.Priority }).ToList();
    }

    public List<ComAccessCode> SetAccessCodes(string accessCodes, int trunkId)
    {
      if (!AccesCodesAreUnique(accessCodes)) return null;
      var allCode = string.IsNullOrEmpty(accessCodes) ? new List<string>() : accessCodes.Split(',').ToList();
      
      var allCodesAndPriorities=
        allCode.Select(c => c.Split(':'))
               .Select(
                 codeAndPriority =>
                 new AccessCodeAndPriority { AccessCode = codeAndPriority[0], Priority = int.Parse(codeAndPriority[1]) });

      return SetCodes(allCodesAndPriorities, trunkId);
    }

    private static List<ComAccessCode> SetCodes(IEnumerable<AccessCodeAndPriority> codes, int trunkId)
    {
      return codes.Select(c => new ComAccessCode { Code = c.AccessCode, Priority = c.Priority, TrunkId = trunkId }).ToList();
    }

    private bool AccesCodesAreUnique(string accessCodes)
    {
      var newCodes = accessCodes.Split(',').ToList();
      var newCodeAndPriority =
        newCodes.Select(code => code.Split(':'))
                .Select(cP => new AccessCodeAndPriority { AccessCode = cP[0], Priority = int.Parse(cP[1]) })
                .ToList();
      var rtn = true;

      if (newCodes.Count == newCodes.Distinct().Count())
      {
        var listOfHeldCodes = new List<AccessCodeAndPriority>();
        foreach (var x in _repository.GetList<ITrunk>().Select(u => u.AccessCodes))
        {
          listOfHeldCodes.AddRange(
            x.Select(a => new AccessCodeAndPriority { AccessCode = a.AccessCode, Priority = a.Priority }));
        }

        foreach (var c in listOfHeldCodes)
        {
          foreach (var n in newCodeAndPriority)
          {
            var count = 0;
            if (c.AccessCode.Equals(n.AccessCode) && c.Priority == n.Priority)
            {
              count++;
            }
            rtn = count < 2;
          }
        }
      }
      return rtn;
    }

    private  bool SetDefaults(IDDI ddi, string defaultDestination, string[] dest)
    {
      bool rtn;
      if (!string.IsNullOrEmpty(defaultDestination))
      {
        RemoveDefaultDDIRoute(ddi);
        rtn = AddDefaultDDIRoute(ddi, dest);
        ddi.UsedOn = DDIUsedOn.Default;
        ddi.Update();
      }
      else
      {
        RemoveDefaultDDIRoute(ddi);
        ddi.UsedOn = DDIUsedOn.NotUsed;
        rtn = ddi.Update();
      }
      return rtn;
    }

    private static string[] GetDestination(string destination)
    {
      return destination.Split(',');
    }

    private  void RemoveDefaultDDIRoute(IDDI ddi)
    {
      var num = ddi.DDINumber;

      foreach (var source in _repository.GetList<IRoutingRule>().Where(r => r.Number == num && r.Dialplan.Id == 12))
      {
        source.Delete();
      }
    }

    private  bool AddDefaultDDIRoute(IDDI ddi, string[] dest)
    {
      var rule = _repository.Add<IRoutingRule>();
      rule.Dialplan = _repository.GetFromId<IDialplan>(12);
      rule.Number = ddi.DDINumber;
      rule.DestinationType = (RoutingRuleDestination)Enum.Parse(typeof(RoutingRuleDestination), dest[0].Trim());
      rule.DestinationNumber = dest[1].Trim();
      rule.Time = 0;
      return rule.Update();
    }

  }
}