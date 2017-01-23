using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class FuPermissionPattern : IFuPermissionPattern
    {
        public FuPermissionPattern()
        {
            Pattern = "";
            FuPatternName = "";
        }

        public virtual int Id { get; set; }
        public virtual string Name { get { return FuPatternName; } }
        public virtual string Pattern { get; set; }
        public virtual string FuPatternName { get; set; }
    }
}
