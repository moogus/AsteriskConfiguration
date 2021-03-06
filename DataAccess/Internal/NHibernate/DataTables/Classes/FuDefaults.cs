using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class FuDefaults :  IFuDefaults
  {
    public virtual int Id { get; set; }
    public virtual string Name { get { return ColumnTitle; } }
    public virtual string Type { get; set; }
    public virtual int ColumnIndex { get; set; }
    public virtual string ColumnType { get; set; }
    public virtual string ColumnTitle { get; set; }
    public virtual string Property { get; set; }
    public virtual string Default { get; set; }
    public virtual string Picker { get; set; }
  }
}