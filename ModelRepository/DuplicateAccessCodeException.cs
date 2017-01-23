using System;
using ModelRepository.ModelInterfaces;

namespace ModelRepository
{
  public abstract class DuplicateAccessCodeException : Exception
  {
    protected DuplicateAccessCodeException(string message) : base(message) { }

    public ITrunk Trunk { get; set; }
    public IAccessCode AccessCode { get; set; }
  }
}
