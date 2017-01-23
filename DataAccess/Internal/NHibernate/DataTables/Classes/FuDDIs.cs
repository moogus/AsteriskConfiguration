using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class FuDDI : IFuDDI
    {
        public FuDDI()
        {
            UsedOn = "";
        }

        public virtual int Id { get; set; }
        public virtual string Name { get { return DDI; } }
        public virtual string DDI { get; set; }
        public virtual int TrunkId { get; set; }
        public virtual string UsedOn { get; set; }
    }
}