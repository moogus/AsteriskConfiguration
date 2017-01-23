using System;

namespace DataAccess.TableInterfaces
{
  public interface IFuDialplanDate : IDatabaseTable
  {
    int FuDialplanId { get; set; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
  }
}