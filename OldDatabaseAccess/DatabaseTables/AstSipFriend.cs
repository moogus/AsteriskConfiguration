namespace DatabaseAccess.DatabaseTables
{
    public class AstSipFriend : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Number { get; set; }
        public virtual string Password { get; set; }
        public virtual string Context { get; set; }
        public virtual string Host { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual string Model { get; set; }
        public virtual string DefaultUser { get; set; }
        public virtual double StatusTime { get; set; }
        public virtual string Insecure { get; set; }
        public virtual string SubscribeContext { get; set; }
        public virtual int CallGroup { get; set; }
        public virtual int PickupGroup { get; set; }
        public virtual string AllowSubscribe { get; set; }
        public virtual string NotifyRinging { get; set; }
        public virtual string NotifyHold { get; set; }
        public virtual string NotifyCid { get; set; }
        public virtual int CallLimit { get; set; }
    
    }
}