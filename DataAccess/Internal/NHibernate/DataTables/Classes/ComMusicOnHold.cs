using DataAccess.TableInterfaces;

namespace DataAccess.Internal.NHibernate.DataTables.Classes
{
    internal class ComMusicOnHold : IComMusicOnHold
    {
        public ComMusicOnHold()
        {
            ComMusicOnHoldName = "";
            Directory = "";
            Application = "";
            Mode = "files";
            Digit = '0';
            Sort = "";
            Format = "";
        }

        public virtual int Id { get; set; }
        public virtual string Name { get { return ComMusicOnHoldName; } }
        public virtual string Directory { get; set; }
        public virtual string Application { get; set; }
        public virtual string ComMusicOnHoldName { get; set; }
        public virtual string Mode { get; set; }
        public virtual char Digit { get; set; }
        public virtual string Sort { get; set; }
        public virtual string Format { get; set; }
        public virtual string Random { get; set; }
    }
}