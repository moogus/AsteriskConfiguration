using System.Collections.Generic;

namespace ModelRepository.ModelInterfaces
{
  public interface ITrunk : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string DefaultDestination { get; set; }
    TrunkType TrunkType { get; set; }
    TrunkInPresentationType TrunkInPresentationType1 { get; set; }
    string TrunkInPresentationValue1 { get; set; }
    TrunkInPresentationType TrunkInPresentationType2 { get; set; }
    string TrunkInPresentationValue2 { get; set; }
    IEnumerable<IDDI> DDIs { get; }
    List<IAccessCode> AccessCodes { get; }
    bool TrunkHasIAccessCode(string code, int priority);
    void AddAccessCodes(IAccessCode accessCode);
  }
}