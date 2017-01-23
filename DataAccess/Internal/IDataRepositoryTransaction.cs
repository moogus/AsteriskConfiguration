using System;

namespace DataAccess.Internal
{
  public interface IDataRepositoryTransaction : IDisposable
  {
    bool Commit();
  }
}