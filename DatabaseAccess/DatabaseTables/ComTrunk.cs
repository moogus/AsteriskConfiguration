namespace DatabaseAccess.DatabaseTables
{
    internal class ComTrunk : IDatabaseTable
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string DefaultDestination { get; set; }
        public virtual string CLIPresentationType1 { get; set; }
        public virtual string CLIPresentationValue1 { get; set; }
        public virtual string CLIPresentationType2 { get; set; }
        public virtual string CLIPresentationValue2 { get; set; }
    }
}