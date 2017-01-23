namespace DatabaseAccess.DatabaseTables
{
    internal class ComQueueMember : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual int ParentQueueId { get; set; }
        public virtual int Penalty { get; set; }
        public virtual int Paused { get; set; }
        public virtual int Type { get; set; }
        public virtual int ExtensionId { get; set; }
        public virtual int QueueId { get; set; }
    }
}