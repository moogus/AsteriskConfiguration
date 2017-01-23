namespace DatabaseAccess.DatabaseTables
{
    internal class AstVoicemail : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Context { get; set; }
        public virtual string Mailbox { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Email { get; set; }
        public virtual int MaxSecs { get; set; }
        public virtual int MinSecs { get; set; }
        public virtual int MaxMessages { get; set; }
        public virtual int HeldMaxMessages { get; set; }
        public virtual string Attach { get; set; }
    }
}
