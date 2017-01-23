using System;

namespace ModelRepository.Internal
{
  public interface IRepositoryTransaction : IDisposable
  {
    bool Commit();
  }
}