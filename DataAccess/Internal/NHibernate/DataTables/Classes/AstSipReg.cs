using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class AstSipReg :  IAstSipReg
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return Number ?? ""; } }
    public virtual string IpAddress { get; set; }
    public virtual string Number { get; set; }
    public virtual double StatusTime { get; set; }
    public virtual string Model { get; set; }
  }
}
