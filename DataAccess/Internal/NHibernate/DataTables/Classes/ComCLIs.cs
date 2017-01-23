using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class ComCLI : IComCLI
    {
        public ComCLI()
        {
            TrunkId = 1;
        }

        public virtual int Id { get; set; }
        public virtual string Name { get { return CLINumber; } }
        public virtual string CLIName { get; set; }
        public virtual string CLINumber { get; set; }
        public virtual int TrunkId { get; set; }
    }
}