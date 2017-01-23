namespace DatabaseAccess.DatabaseTables
{
    public class AstExtension : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Context { get; set; }
        public virtual string ExtensionNumber { get; set; }
        public virtual int  Priority { get; set; }
        public virtual string App { get; set; }
        public virtual string Appdata { get; set; }
        public virtual int Order { get; set; }
        public virtual string DestinationType { get; set; }
        public virtual string DestinationNumber { get; set; }
        public virtual int ForwardingType { get; set; }
    }
}