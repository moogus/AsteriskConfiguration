using System;

namespace ModelRepository.ModelInterfaces
{
  public interface IDDI : IModel, IEquatable<IDDI>
  {
    int Id { get; }
    string DDINumber { get; set; }
    ITrunk Trunk { get; set; }
    DDIUsedOn UsedOn { get; set; }
    string Used { get; }
  }
}