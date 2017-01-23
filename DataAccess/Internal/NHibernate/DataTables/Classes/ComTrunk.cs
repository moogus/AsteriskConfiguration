using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
  internal class ComTrunk :  IComTrunk
  {
      public ComTrunk()
      {
          DefaultDestination = string.Empty;
          CLIPresentationType1 = "None";
          CLIPresentationValue1 = "";
          CLIPresentationType2 = "None";
          CLIPresentationValue2 = "";
      }
    public virtual int Id { get; set; }
    public virtual string Name {get { return ComTrunkName; }}
    public virtual string ComTrunkName { get; set; }
    public virtual string Type { get; set; }
    public virtual string DefaultDestination { get; set; }
    public virtual string CLIPresentationType1 { get; set; }
    public virtual string CLIPresentationValue1 { get; set; }
    public virtual string CLIPresentationType2 { get; set; }
    public virtual string CLIPresentationValue2 { get; set; }
  }
}