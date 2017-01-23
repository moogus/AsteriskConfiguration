using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuDialplan :IFuDialplan
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return FuDialPlanName; } }
    public virtual string FuDialPlanName { get; set; }
  }
}