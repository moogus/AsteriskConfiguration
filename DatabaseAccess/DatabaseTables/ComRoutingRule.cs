namespace DatabaseAccess.DatabaseTables
{
    internal class ComRoutingRule : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual int DialplanId { get; set; }
        public virtual string Number { get; set; }
        public virtual int Time { get; set; }
        public virtual int Order { get; set; }
        public virtual string DestinationType { get; set; }
        public virtual string DestinationNumber { get; set; }
    }
}