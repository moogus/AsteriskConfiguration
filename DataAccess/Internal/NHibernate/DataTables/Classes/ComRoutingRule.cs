using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class ComRoutingRule : IComRoutingRule
    {
        public ComRoutingRule()
        {
            DestinationType = "";
        }

        public virtual int Id { get; set; }
        public virtual string Name { get { return Number; } }
        public virtual int DialplanId { get; set; }
        public virtual string Number { get; set; }
        public virtual int Time { get; set; }
        public virtual int Order { get; set; }
        public virtual string DestinationType { get; set; }
        public virtual string DestinationNumber { get; set; }
    }
}