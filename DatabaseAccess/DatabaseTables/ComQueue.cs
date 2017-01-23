namespace DatabaseAccess.DatabaseTables
{
    internal class ComQueue : IDatabaseTable
    {
        public ComQueue()
        {
            Strategy = "Ringall";
            CLIId = 0;
            ComMusicOnHoldId = 0;
            IncludeInDirectory = true;
        }

        public virtual int Id { get; set; }
        public virtual string Number { get; set; }
        public virtual string Notes { get; set; }
        public virtual string QueueName { get; set; }
        public virtual string Department { get; set; }
        public virtual string Strategy { get; set; }
        public virtual bool RingOnBusy { get; set; }
        public virtual string DDINumber { get; set; }
        public virtual int VoiceMailId { get; set; }
        public virtual int VoicemailDelay { get; set; }
        public virtual int CLIId { get; set; }
        public virtual int ComMusicOnHoldId { get; set; }
        public virtual bool IncludeInDirectory { get; set; }
    }
}