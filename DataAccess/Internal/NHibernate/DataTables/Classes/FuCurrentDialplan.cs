using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuCurrentDialplan :  IFuCurrentDialplan
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return ""; } }
    public virtual int CurrentDialplan { get; set; }
    public virtual bool AutomaticallyChange { get; set; }
  }
}
