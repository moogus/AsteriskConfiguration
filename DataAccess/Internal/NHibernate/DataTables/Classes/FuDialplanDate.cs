using System;
using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuDialplanDate :  IFuDialplanDate
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return StartDate.ToShortDateString(); } }
    public virtual int FuDialplanId { get; set; }
    public virtual DateTime StartDate { get; set; }
    public virtual DateTime EndDate { get; set; }
  }
}
