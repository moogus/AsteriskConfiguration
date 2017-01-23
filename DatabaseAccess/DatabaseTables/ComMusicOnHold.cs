namespace DatabaseAccess.DatabaseTables
{
  internal class ComMusicOnHold : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Directory { get; set; }
        public virtual string Application { get; set; }
        public virtual string Mode { get; set; }
        public virtual char Digit { get; set; }
        public virtual string Sort { get; set; }
        public virtual string Format { get; set; }
        public virtual string Random { get; set; }
    }
}