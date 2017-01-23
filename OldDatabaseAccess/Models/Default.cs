using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
    public class Default : IDefault
    {
        private readonly FuDefaults _under;
        private readonly ISessionWrapper _session;

        public Default(FuDefaults under, ISessionWrapper session, IRepository repository)
        {
            _under = under;
            _session = session;
        }

        public Default(ISessionWrapper session, IRepository repository)
        {
            _session = session;
            _under=new FuDefaults();
        }

        public string Type { get { return _under.Type; } set { _under.Type = value; } }

        public int Index { get { return _under.ColumnIndex; } set { _under.ColumnIndex = value; } }

        public string ColumnTitle { get { return _under.ColumnTitle; } set { _under.ColumnTitle = value; } }

        public string JavascriptColumnType { get { return _under.ColumnType; } set { _under.ColumnType = value; } }

        public string DefaultValue { get { return _under.Default; } set { _under.Default = value; } }

        public string Picker { get { return _under.Picker; } set { _under.Picker = value; } }

        public string JavascriptProperty { get { return _under.Property; } set { _under.Property = value; } }

        public int Id { get { return _under.Id; } set { _under.Id = value; } }

        object IModel.Under
        {
            get { return _under; }
        }

        ISessionWrapper IModel.Session
        {
            get { return _session; }
        }

        public void ExtraUpdate()
        {
        }

        public void ExtraDelete()
        {
        }
    }
}
