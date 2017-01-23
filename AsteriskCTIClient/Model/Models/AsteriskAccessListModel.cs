using System.Collections.Generic;
using System.Linq;
using AsteriskCTIClient.Model.ModelInterfaces;
using DatabaseAccess;

namespace AsteriskCTIClient.Model.Models
{
  public class AsteriskAccessListModel : IAccessListModel
  {
    private readonly IRepository _repository;
    public List<string> AccessCodes { get; private set; }

    public AsteriskAccessListModel(IRepository repository)
    {
      _repository = repository;
      AccessCodes = new List<string>();
      SetAccessCodes();
    }

    private void SetAccessCodes()
    {
      var trunkList = _repository.GetList<ITrunk>().ToList();
      foreach (var a in trunkList.Select(t => t.AccessCodes).ToList().SelectMany(codeList => codeList))
      {
        AccessCodes.Add(a.AccessCode);
      }
    }
  }
}